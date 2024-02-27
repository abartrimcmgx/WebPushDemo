using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPushDemo.Data;
using WebPushDemo.Models;

namespace WebPushDemo.Controllers;

public class DevicesController(WebPushDemoContext context, IConfiguration configuration) : Controller
{
    // GET: Devices
    public async Task<IActionResult> Index()
    {
        return View(await context.Devices.ToListAsync());
    }

    // GET: Devices/Create
    public IActionResult Create()
    {
        ViewBag.PublicKey = configuration.GetSection("VapidKeys")["PublicKey"] ?? string.Empty;

        return View();
    }

    // POST: Devices/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,PushEndpoint,PushP256DH,PushAuth")] Devices devices)
    {
        if (!ModelState.IsValid) return View(devices);
        context.Add(devices);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Devices/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var devices = await context.Devices
            .SingleOrDefaultAsync(m => m.Id == id);
        if (devices == null)
        {
            return NotFound();
        }

        return View(devices);
    }

    // POST: Devices/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var devices = await context.Devices.SingleOrDefaultAsync(m => m.Id == id);
        if (devices != null) context.Devices.Remove(devices);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}