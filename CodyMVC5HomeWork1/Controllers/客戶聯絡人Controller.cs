using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodyMVC5HomeWork1.Models;
using NPOI.XSSF.UserModel;
using System.IO;

namespace CodyMVC5HomeWork1.Controllers
{
    public class 客戶聯絡人Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶聯絡人
        public ActionResult Index()
        {
            ViewBag.客戶職稱List = new SelectList(repo客戶聯絡人.JobList());
            var 客戶聯絡人 = repo客戶聯絡人.All().Include(客 => 客.客戶資料);
            return View(客戶聯絡人.ToList());
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var keyword= collection["KeyWord"];
            var joblist = collection["客戶職稱List"];

            ViewBag.客戶職稱List = new SelectList(repo客戶聯絡人.JobList(), joblist);

            ViewData["KeyWord"] = keyword;




            //    if (keywork != null && keywork != "")
            //    {
            //        var 客戶聯絡人 = repo客戶聯絡人.All().Where(聯 =>  聯.姓名.Contains(keywork) ).Include(客 => 客.客戶資料);
            //        return View(客戶聯絡人.ToList());
            //    }
            //    else
            //    {
            //        var 客戶聯絡人 = repo客戶聯絡人.All().Include(客 => 客.客戶資料);
            //        return View(客戶聯絡人.ToList());
            //    }

            return View(repo客戶聯絡人.Query(keyword, joblist).ToList());
        }

        // GET: 客戶聯絡人/Details/5
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error2")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new ArgumentException("沒有帶入差數");
            }
            客戶聯絡人 客戶聯絡人 = repo客戶聯絡人.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                repo客戶聯絡人.Add(客戶聯絡人);
                repo客戶聯絡人.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error2")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new ArgumentException("沒有帶入差數");
            }
            客戶聯絡人 客戶聯絡人 = repo客戶聯絡人.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var db客戶資料 = (客戶資料Entities)repo客戶資料.UnitOfWork.Context;
                db客戶資料.Entry(客戶聯絡人).State = EntityState.Modified;
                db客戶資料.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error2")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new ArgumentException("沒有帶入差數");
            }
            客戶聯絡人 客戶聯絡人 = repo客戶聯絡人.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = repo客戶聯絡人.Find(id);
            repo客戶聯絡人.Delete(客戶聯絡人);
            repo客戶聯絡人.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo客戶聯絡人.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Export()
        {
            var Workbook = new XSSFWorkbook();
            var sheet = Workbook.CreateSheet("結果");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("Id");
            sheet.GetRow(0).CreateCell(1).SetCellValue("客戶名稱");
            sheet.GetRow(0).CreateCell(2).SetCellValue("職稱");
            sheet.GetRow(0).CreateCell(3).SetCellValue("姓名");
            sheet.GetRow(0).CreateCell(4).SetCellValue("Email");
            sheet.GetRow(0).CreateCell(5).SetCellValue("手機");
            sheet.GetRow(0).CreateCell(6).SetCellValue("電話");

            var data = repo客戶聯絡人.All();
            int i = 1;
            foreach (var item in data)
            {
                sheet.CreateRow(i).CreateCell(0).SetCellValue(item.Id);
                sheet.GetRow(i).CreateCell(1).SetCellValue(item.客戶資料.客戶名稱);
                sheet.GetRow(i).CreateCell(2).SetCellValue(item.職稱);
                sheet.GetRow(i).CreateCell(3).SetCellValue(item.姓名);
                sheet.GetRow(i).CreateCell(4).SetCellValue(item.Email);
                sheet.GetRow(i).CreateCell(5).SetCellValue(item.手機);
                sheet.GetRow(i).CreateCell(6).SetCellValue(item.電話);

                i++;
            }

            MemoryStream files = new MemoryStream();
            Workbook.Write(files);
            files.Close();

            return File(files.ToArray(), "application/vnd.ms-excel", "Export.xlsx");
        }
    }
}
