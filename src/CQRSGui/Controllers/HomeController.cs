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
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(string name)
        {
            _bus.Send(new CreateInventoryItem(Guid.NewGuid(), name));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ChangeName(Guid id) => View(_readmodel.GetInventoryItemDetails(id));

        [HttpPost]
        public IActionResult ChangeName(Guid id, string name, int version)
        {
            _bus.Send(new RenameInventoryItem(id, name, version));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CheckIn(Guid id) => View(_readmodel.GetInventoryItemDetails(id));

        [HttpPost]
        public IActionResult CheckIn(Guid id, int number, int version)
        {
            _bus.Send(new CheckInItemsToInventory(id, number, version));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Deactivate(Guid id) => View(_readmodel.GetInventoryItemDetails(id));

        [HttpPost]
        public IActionResult Deactivate(Guid id, int version)
        {
            _bus.Send(new DeactivateInventoryItem(id, version));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Remove(Guid id) => View(_readmodel.GetInventoryItemDetails(id));

        [HttpPost]
        public IActionResult Remove(Guid id, int number, int version)
        {
            _bus.Send(new RemoveItemsFromInventory(id, number, version));
            return RedirectToAction("Index");
        }
    }
}
