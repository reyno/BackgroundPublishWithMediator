using Microsoft.AspNetCore.Mvc;

namespace BackgroundQueue.Controllers {
    public class HomeController : Controller {
        private readonly IBackgroundPublisher _backgroundPublisher;

        public HomeController(
            IBackgroundPublisher backgroundPublisher
            ) {
            _backgroundPublisher = backgroundPublisher;
        }

        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult QueueTask() {

            
            _backgroundPublisher.Publish(new MyNotification { Value = "something interesting" });

            return RedirectToAction("Index");

        }
    }
}
