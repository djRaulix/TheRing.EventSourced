namespace WebSample
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
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

    using Thering.EventSourced.Eventing.Aliaser;
    using Thering.EventSourced.Eventing.Handlers;
    using Thering.EventSourced.Eventing.Repositories;

    using TheRing.EventSourced.GetEventStore;
    using TheRing.EventSourced.GetEventStore.Serializers;
    using TheRing.EventSourced.Redis;

    using WebSample.Commanding;
    using WebSample.Domain.User;
    using WebSample.Domain.User.Events;
    using WebSample.ReadModel;

    using EventHandler = Thering.EventSourced.Eventing.Handlers.EventHandler;

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
            container.RegisterSingle(
                typeof(IGetSnapshotKeyFromAggregateRoot),
                typeof(SnapshotKeyFromAggregateRootGetter));
            container.RegisterSingle(typeof(ISnapshoter), typeof(RedisSnapshoter));
            container.RegisterSingle(typeof(IAggregateRootRepository), typeof(AggregateRootRepository));


            container.RegisterSingle(typeof(ISerializeEvent), typeof(EventSerializer));
            container.RegisterSingle(typeof(ISerialize), typeof(ServiceStackJsonSerializer));
            
            container.RegisterSingle(typeof(IRedisClient), new RedisClient());


            container.RegisterSingle<IHandleEvent<UserAddressAdded>>(new UserViewDenormalizer());


            container.Register(
                typeof(IEventStoreConnection),
                () =>
                {
                    var connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
                    connection.Connect();
                    return connection;
                });

            container.RegisterSingle(
                typeof(IHandle<AddAdressCommand>),
                typeof(UpdateAggregateHandler<User, AddAdressCommand>));
            container.RegisterSingle(
                typeof(IHandle<CreateUserCommand>),
                typeof(CreateAggregateHandler<User, CreateUserCommand>));
            var userCommandHandler = new UserCommandHandler();
            container.RegisterSingle<IRunCommand<User, AddAdressCommand>>(userCommandHandler);
            container.RegisterSingle<IRunCommand<User, CreateUserCommand>>(userCommandHandler);


            container.RegisterSingle<IDispatch>(new Dispatcher(container));

            container.Register<IRedisClientsManager>(() => new PooledRedisClientManager("localhost:6379"));
            container.RegisterSingle(() => container.GetInstance<IRedisClientsManager>().GetCacheClient());

            container.RegisterSingle<IEventPositionRepository, RedisEventPositionRepository>();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            var eventQueueFactory = new Dictionary<Type, ICollection<IEventQueue>>();
            var eventQueues = new Dictionary<Type, IEventQueue>();

            var denormalizerTypes =
                typeof(UserViewDenormalizer).Assembly.GetTypes()
                    .Where(
                        t =>
                            t.GetInterfaces()
                                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == (typeof(IHandleEvent<>))))
                    .ToList();

            container.RegisterSingle<IEventPositionManager, EventPositionManager>();

            container.RegisterSingle<ITypeAliaser>(() => 
                new TypeAliaser(
                    new EventStoreEventStreamRepository(
                        container.GetInstance<IEventStoreConnection>(),
                        new TypeSerializer(container.GetInstance<ISerialize>()))));

            var eventPositionRepository = container.GetInstance<IEventPositionRepository>();
            var eventPositionManager = container.GetInstance<EventPositionManager>();

            foreach (var denormalizerType in denormalizerTypes)
            {
                eventQueues.Add(
                    denormalizerType,
                    (new EventHandlerQueue(
                        new EventHandler(
                            Activator.CreateInstance(denormalizerType),
                            new RedisErrorHandler(container.GetInstance<IRedisClient>()), 
                            eventPositionManager))));
            }

            foreach (
                var eventType in
                    typeof(UserCreated).Assembly.GetTypes()
                        .Where(t => t.Namespace != null && t.Namespace.Equals("WebSample.Domain.User.Events")))
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
            new GetEventStoreEventPublisher(
                container.GetInstance<IEventStoreConnection>(),
                eventPositionRepository,
                container.GetInstance<ISerializeEvent>(),
                new EventPublisherQueue(t => eventQueueFactory[t], eventPositionManager));

            return container;
        }
    }
}