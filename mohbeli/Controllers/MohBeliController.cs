using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mohbeli.Models;
using mohbeli.Repository;

namespace mohbeli.Controllers
{
    public class MohBeliController : Controller
    {
        // GET: mohbeli
        public ActionResult Index()
        {
            Data data = new Data();
            var list = data.GetAllDetail();
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(MohBeliDetail details)
        {
            Data data = new Data();
            data.SaveMohBeliDetails(details);
            ModelState.Clear();
            return View();
        }

        public ActionResult CreateItem(Items item)
        {
            return PartialView("_CreateItem",item);
        }

        public ActionResult ViewMohBeli(int Id)
        {
            Data data = new Data();
            var details = data.GetDetail(Id);
            return View(details);
        }
    }
}