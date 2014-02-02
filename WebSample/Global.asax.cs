using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EventStore.ClientAPI;
using ServiceStack.Redis;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using TheRing.EventSourced.Application;
using TheRing.EventSourced.Core;
using TheRing.EventSourced.Domain.Repository;
using TheRing.EventSourced.Domain.Repository.Factory;
using TheRing.EventSourced.Domain.Repository.Snapshot;
using Thering.EventSourced.Eventing;
using TheRing.EventSourced.GetEventStore;
using TheRing.EventSourced.Redis;
using WebSample.Commanding;
using WebSample.Domain.User;
using WebSample.ReadModel;

namespace WebSample
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using WebSample.Domain.User.Events;
    using WebSample.Eventing;

    using EventHandler = Thering.EventSourced.Eventing.EventHandler;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = InitializeIoc();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static Container InitializeIoc()
        {
            var container = new Container();
            container.RegisterSingle(typeof(IAggregateRootFactory), typeof(AggregateRootFactory));
            container.RegisterSingle(typeof(IEventStreamRepository), typeof(EventStoreEventStreamRepository));
            container.RegisterSingle(typeof(IGetStreamNameFromAggregateRoot), typeof(StreamNameFromAggregateRootGetter));
            container.RegisterSingle(typeof(IGetSnapshotKeyFromAggregateRoot), typeof(SnapshotKeyFromAggregateRootGetter));
            container.RegisterSingle(typeof(ISnapshoter), typeof(RedisSnapshoter));
            container.RegisterSingle(typeof(IAggregateRootRepository), typeof(AggregateRootRepository));


            container.RegisterSingle(typeof(ISerializeEvent), typeof(EventSerializer));
            container.RegisterSingle(typeof(ISerialize), typeof(ServiceStackJsonSerializer));
            container.RegisterSingle(typeof(ITypeAliaser), typeof(TypeAliaser));
            container.RegisterSingle(typeof(IRedisClient), new RedisClient());

            
             
            var connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            connection.Connect();


            container.RegisterSingle<IHandleEvent<UserAddressAdded>>(new UserViewDenormalizer());

           

            container.RegisterSingle(typeof(IEventStoreConnection), connection);

            container.RegisterSingle(typeof(IHandle<AddAdressCommand>), typeof(UpdateAggregateHandler<User, AddAdressCommand>));
            container.RegisterSingle(typeof(IHandle<CreateUserCommand>), typeof(CreateAggregateHandler<User, CreateUserCommand>));
            var userCommandHandler = new UserCommandHandler();
            container.RegisterSingle<IRunCommand<User, AddAdressCommand>>(userCommandHandler);
            container.RegisterSingle<IRunCommand<User, CreateUserCommand>>(userCommandHandler);


            container.RegisterSingle<IDispatch>(new Dispatcher(container));

            container.Register<IEventPositionRepository, EventPositionRepository>();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            var eventQueueFactory = new Dictionary<Type, ICollection<IEventQueue>>();
            var eventQueues = new Dictionary<Type,IEventQueue>();

            var denormalizerTypes = typeof(UserViewDenormalizer).Assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == (typeof(IHandleEvent<>)))).ToList();

            foreach (var denormalizerType in denormalizerTypes)
            {
                eventQueues.Add(denormalizerType, (new EventQueue(new EventHandler(Activator.CreateInstance(denormalizerType), new ErrorHanlder(), container.GetInstance<IEventPositionRepository>())))); 
            }

            foreach (var eventType in typeof(UserCreated).Assembly.GetTypes().Where(t => t.Namespace != null && t.Namespace.Equals("WebSample.Domain.User.Events")))
            {
                if (!eventQueueFactory.ContainsKey(eventType))
                {
                    eventQueueFactory.Add(eventType, new Collection<IEventQueue>());
                }

                foreach (var denormalizerType in denormalizerTypes)
                {
                    if (!eventQueueFactory[eventType].Contains(eventQueues[denormalizerType]))
                    {
                        eventQueueFactory[eventType].Add(eventQueues[denormalizerType]);
                    }
                }
            }

            //eventPublisher will subsribe
            new EventPublisher(connection, container.GetInstance<ISerializeEvent>(),t => eventQueueFactory[t]);

            return container;
        }
    }
}
