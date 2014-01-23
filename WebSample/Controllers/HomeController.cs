using System;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using TheRing.EventSourced.Application;
using WebSample.Commanding;

namespace WebSample.Controllers
{
    using WebSample.ReadModel;

    public class HomeController : Controller
    {
        private readonly IDispatch commandDispatcher;

        public HomeController(IDispatch commandDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
            if (Database.Adresses == null)
            {
                var command = new CreateUserCommand
                {
                    FirstName = "Toto",
                    LastName = "Titi"
                };
                commandDispatcher.Dispatch(command);
                Database.UserId = command.AggregateRootId;
            }   
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

         [HttpPost]
        public ActionResult AddAdress(string adress)
         {
             var command = new AddAdressCommand
             {
                 Address = adress,
                 AggregateRootId = Database.UserId
             };
             var result = commandDispatcher.Dispatch(command);

             if (!result.Ok)
             {
                 return View("MaxErrorOccuredView");
             }
             return View("Index");
        }
    }
}

