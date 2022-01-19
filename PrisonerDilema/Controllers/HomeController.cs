using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using PrisonersDilema.messages;
using PrisonersDilema.Models;
using System.Diagnostics;
using static PrisonersDilema.Delegates;

namespace PrisonerDilema.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IActorRef _apiActor;

        public HomeController(ILogger<HomeController> logger, APIProvider actor)
        {
            _logger = logger;
            _apiActor = actor();
        }

        public async Task<IActionResult> Index()
        {
            var res = await _apiActor.Ask<GetStatistic>(new GetStatistic { });
            return View(res);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}