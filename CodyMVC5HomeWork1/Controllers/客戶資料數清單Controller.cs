using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodyMVC5HomeWork1.Models;

namespace CodyMVC5HomeWork1.Controllers
{

    public class 客戶資料數清單Controller : Controller
    {
        private 客戶資料Entities db = new 客戶資料Entities();
        // GET: 客戶資料數清單
        public ActionResult Index()
        {
            return View(db.客戶資料數清單.ToList());
        }
    }
}