using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUEM.Web.Controllers
{
    [Authorize(Roles = "Admin, Club")]
    public class EventRegistrationController : Controller
    {
        // GET: EventRegistrationController
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventRegistrationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EventRegistrationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EventRegistrationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EventRegistrationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EventRegistrationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
