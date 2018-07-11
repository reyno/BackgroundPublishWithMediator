using Microsoft.AspNetCore.Mvc;

namespace BackgroundQueue.Controllers {
    public class HomeController : Controller {
        private readonly IBackgroundNotificationQueue _backgroundNotificationQueue;

        public HomeController(
            IBackgroundNotificationQueue backgroundNotificationQueue
            ) {
            _backgroundNotificationQueue = backgroundNotificationQueue;
        }

        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult QueueTask() {

            
            _backgroundNotificationQueue.QueueNotification(new MyNotification { Value = "something interesting" });

            return RedirectToAction("Index");

        }
    }
}
