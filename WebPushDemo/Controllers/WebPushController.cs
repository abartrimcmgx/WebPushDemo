using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPush;
using WebPushDemo.Data;

namespace WebPushDemo.Controllers
{
    public class WebPushController(WebPushDemoContext context, IConfiguration configuration) : Controller
    {
        public IActionResult Send(int? id)
        {
            return View();
        }

        [HttpPost, ActionName("Send")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(int id)
        {
            var payload = Request.Form["payload"];
            var device = await context.Devices.SingleOrDefaultAsync(m => m.Id == id);

            var vapidPublicKey = configuration.GetSection("VapidKeys")["PublicKey"];
            var vapidPrivateKey = configuration.GetSection("VapidKeys")["PrivateKey"];

            var pushSubscription = new PushSubscription(device?.PushEndpoint, device?.PushP256Dh, device?.PushAuth);
            var vapidDetails = new VapidDetails("mailto:example@example.com", vapidPublicKey, vapidPrivateKey);

            var webPushClient = new WebPushClient();
            await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);

            return View();
        }

        public IActionResult GenerateKeys()
        {
            var keys = VapidHelper.GenerateVapidKeys();
            ViewBag.PublicKey = keys.PublicKey;
            ViewBag.PrivateKey = keys.PrivateKey;
            return View();
        }
    }
}