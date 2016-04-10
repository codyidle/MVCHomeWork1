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
    public class 客戶銀行資訊Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶銀行資訊
        public ActionResult Index()
        {
            var 客戶銀行資訊 = repo客戶銀行資訊.All().Include(客 => 客.客戶資料);
            return View(客戶銀行資訊.ToList());
        }

        [HttpPost]
        public ActionResult Index(string KeyWord)
        {
            ViewData["KeyWord"] = KeyWord;
            if (KeyWord != null && KeyWord != "")
            {
                var 客戶銀行資訊 = repo客戶銀行資訊.All().Where(銀 =>  銀.銀行名稱.Contains(KeyWord) ).Include(客 => 客.客戶資料);
                return View(客戶銀行資訊.ToList());
            }
            else
            {
                var 客戶銀行資訊 = repo客戶銀行資訊.All().Include(客 => 客.客戶資料);
                return View(客戶銀行資訊.ToList());
            }
            
        }

        // GET: 客戶銀行資訊/Details/5
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error2")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new ArgumentException("沒有帶入差數");
            }
            客戶銀行資訊 客戶銀行資訊 = repo客戶銀行資訊.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶銀行資訊/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                repo客戶銀行資訊.Add(客戶銀行資訊);
                repo客戶銀行資訊.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Edit/5
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error2")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new ArgumentException("沒有帶入差數");
            }
            客戶銀行資訊 客戶銀行資訊 = repo客戶銀行資訊.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                var db客戶資料 = (客戶資料Entities)repo客戶銀行資訊.UnitOfWork.Context;
                db客戶資料.Entry(客戶銀行資訊).State = EntityState.Modified;
                db客戶資料.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Delete/5
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error2")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new ArgumentException("沒有帶入差數");
            }
            客戶銀行資訊 客戶銀行資訊 = repo客戶銀行資訊.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = repo客戶銀行資訊.Find(id);
            repo客戶銀行資訊.Delete(客戶銀行資訊);
            repo客戶銀行資訊.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo客戶銀行資訊.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }



        public ActionResult Export()
        {
            var Workbook = new XSSFWorkbook();
            var sheet = Workbook.CreateSheet("結果");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("Id");
            sheet.GetRow(0).CreateCell(1).SetCellValue("客戶名稱");
            sheet.GetRow(0).CreateCell(2).SetCellValue("銀行名稱");
            sheet.GetRow(0).CreateCell(3).SetCellValue("銀行代碼");
            sheet.GetRow(0).CreateCell(4).SetCellValue("分行代碼");
            sheet.GetRow(0).CreateCell(5).SetCellValue("帳戶名稱");
            sheet.GetRow(0).CreateCell(6).SetCellValue("帳戶號碼");

            var data = repo客戶銀行資訊.All();
            int i = 1;
            foreach (var item in data)
            {
                sheet.CreateRow(i).CreateCell(0).SetCellValue(item.Id);
                sheet.GetRow(i).CreateCell(1).SetCellValue(item.客戶資料.客戶名稱);
                sheet.GetRow(i).CreateCell(2).SetCellValue(item.銀行名稱);
                sheet.GetRow(i).CreateCell(3).SetCellValue(item.銀行代碼);
                sheet.GetRow(i).CreateCell(4).SetCellValue(item.分行代碼.Value);
                sheet.GetRow(i).CreateCell(5).SetCellValue(item.帳戶名稱);
                sheet.GetRow(i).CreateCell(6).SetCellValue(item.帳戶號碼);

                i++;
            }

            MemoryStream files = new MemoryStream();
            Workbook.Write(files);
            files.Close();

            return File(files.ToArray(), "application/vnd.ms-excel", "Export.xlsx");
        }
    }
}
