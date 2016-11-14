using Microsoft.AspNetCore.Mvc;
using SimpleCQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSGui.Controllers
{
    public class HomeController : Controller
    {
        private FakeBus _bus;
        private ReadModelFacade _readmodel;

        public HomeController()
        {
            _bus = ServiceLocator.Bus;
            _readmodel = new ReadModelFacade();
        }

        [HttpGet]
        public IActionResult Index() => View(_readmodel.GetInventoryItems());

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            return View(_readmodel.GetInventoryItemDetails(id));
        }

        [HttpGet]
        public ActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(string name)
        {
            _bus.Send(new CreateInventoryItem(Guid.NewGuid(), name));
            return RedirectToAction("Index");
        }
    }
}
