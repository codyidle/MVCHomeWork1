using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodyMVC5HomeWork1.Models;
using System.Collections;
using System.IO;
using NPOI.XSSF.UserModel;

namespace CodyMVC5HomeWork1.Controllers
{
    public class 客戶資料Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶資料
        [Action執行時間]
        [HandleError(View = "Error2")]
        public ActionResult Index(string newSort,string oldSort ,string sortDesc,string KeyWord, string Category,string IsExport)
        {


            newSort = string.IsNullOrEmpty(newSort) ? "客戶名稱" : newSort;

            var data = repo客戶資料.Query(KeyWord,Category).AsEnumerable();


            ViewBag.Category = new SelectList(repo客戶資料.CatgoryList(), "Cid", "Cname", Category);
            ViewData["KeyWord"] = KeyWord;


            var param = newSort;
            var pi = typeof(客戶資料).GetProperty(param);

            if (sortDesc == "Desc")
                data = data.OrderByDescending(x => pi.GetValue(x,null));
            else
                data = data.OrderBy(x => pi.GetValue(x, null));

            if (IsExport == "Y")
            {
                return Export(data);
            }
            else
                return View(data.ToList());
        }

        [Action執行時間]
        public ActionResult Export(IEnumerable<客戶資料> data)
        {
            var Workbook = new XSSFWorkbook();
            var sheet = Workbook.CreateSheet("結果");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("Id");
            sheet.GetRow(0).CreateCell(1).SetCellValue("客戶名稱");
            sheet.GetRow(0).CreateCell(2).SetCellValue("統一編號");
            sheet.GetRow(0).CreateCell(3).SetCellValue("電話");
            sheet.GetRow(0).CreateCell(4).SetCellValue("傳真");
            sheet.GetRow(0).CreateCell(5).SetCellValue("地址");
            sheet.GetRow(0).CreateCell(6).SetCellValue("Email");

            int i = 1;
            foreach (var item in data)
            {
                sheet.CreateRow(i).CreateCell(0).SetCellValue(item.Id);
                sheet.GetRow(i).CreateCell(1).SetCellValue(item.客戶名稱);
                sheet.GetRow(i).CreateCell(2).SetCellValue(item.統一編號);
                sheet.GetRow(i).CreateCell(3).SetCellValue(item.電話);
                sheet.GetRow(i).CreateCell(4).SetCellValue(item.傳真);
                sheet.GetRow(i).CreateCell(5).SetCellValue(item.地址);
                sheet.GetRow(i).CreateCell(6).SetCellValue(item.Email);

                i++;
            }

            MemoryStream files = new MemoryStream();
            Workbook.Write(files);
            files.Close();

            return File(files.ToArray(), "application/vnd.ms-excel", "Export.xlsx");
        }

        [Action執行時間]
        [HandleError(View = "Error2")]
        [HttpPost]
        public ActionResult Index(FormCollection collection, string newSort, string oldSort, string sortDesc)
        {
            if (!string.IsNullOrEmpty(newSort) && newSort == oldSort)
            {
                if (sortDesc == "Desc")
                    sortDesc = "";
                else
                    sortDesc = "Desc";
            }
            else
                sortDesc = "";

            ViewBag.SortBy = string.IsNullOrEmpty(newSort) ? "客戶名稱" : newSort;
            ViewBag.SortDesc = sortDesc;


            var KeyWord = collection["KeyWord"];
            var Category = collection["客戶分類"];

            ViewBag.客戶分類 = new SelectList(repo客戶資料.CatgoryList(), "Cid", "Cname", Category);
            ViewData["KeyWord"] = KeyWord;
            ViewData["Category"] = Category;

            var data = repo客戶資料.All().AsEnumerable();

            if (KeyWord != null && KeyWord != "")
                data = data.Where(客 => 客.客戶名稱.Contains(KeyWord));

            if (Category != null && Category != "")
                data = data.Where(客 => 客.客戶分類== Category);


            var param = (string)ViewBag.SortBy;
            var pi = typeof(客戶資料).GetProperty(param);

            if (sortDesc == "Desc")
                data = data.OrderByDescending(x => pi.GetValue(x, null));
            else
                data = data.OrderBy(x => pi.GetValue(x, null));

            //if (KeyWord != null && KeyWord != "")
            //    return View(repo客戶資料.All().Where(客 => 客.客戶名稱.Contains(KeyWord)).ToList());
            //else
            //    return View(repo客戶資料.All().ToList());

            return View(data.ToList());
        }

        // GET: 客戶資料/Details/5
        [Action執行時間]
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error2")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new ArgumentException("沒有帶入差數");
            }
            var customer = repo客戶資料.Find(id.Value);
            var contacts = repo客戶聯絡人.All().Where(p => p.客戶Id == id.Value);
            客戶聯絡人ViewModel contactsdetail = new 客戶聯絡人ViewModel();
            contactsdetail.CustomerData = customer;
            contactsdetail.ContactsData = contacts;

            return View(contactsdetail);
        }

        [Action執行時間]
        [HttpPost]
        public ActionResult Details(IList<客戶聯絡人ViewModel> data)
        {
            if (ModelState.IsValid )
            {
                foreach (var item in data)
                {

                    var Contact = repo客戶聯絡人.Find(item.Id);

                    Contact.職稱 = item.職稱;

                    Contact.手機 = item.手機;

                    Contact.電話 = item.電話;

                }
                repo客戶聯絡人.UnitOfWork.Commit();

                return RedirectToAction("Index", "客戶資料");
            }


            var customer = repo客戶資料.Find(data[0].客戶Id);
            var contacts = repo客戶聯絡人.All().Where(p => p.客戶Id == data[0].客戶Id);
            客戶聯絡人ViewModel contactsdetail = new 客戶聯絡人ViewModel();
            contactsdetail.CustomerData = customer;
            contactsdetail.ContactsData = contacts;

            return View(contactsdetail);
        }

        // GET: 客戶資料/Create
        [Action執行時間]
        public ActionResult Create()
        {


            ViewBag.客戶分類 = new SelectList(repo客戶資料.CatgoryList(), "Cid","Cname");
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [Action執行時間]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo客戶資料.Add(客戶資料);
                repo客戶資料.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶分類 = new SelectList(repo客戶資料.CatgoryList(), "Cid", "Cname", 客戶資料.客戶分類);
            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        [Action執行時間]
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error2")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new ArgumentException("沒有帶入差數");
            }
            客戶資料 客戶資料 = repo客戶資料.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶分類 = new SelectList(repo客戶資料.CatgoryList(), "Cid", "Cname", 客戶資料.客戶分類);
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [Action執行時間]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var db客戶資料 = (客戶資料Entities) repo客戶資料.UnitOfWork.Context;
                db客戶資料.Entry(客戶資料).State = EntityState.Modified;
                db客戶資料.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶分類 = new SelectList(repo客戶資料.CatgoryList(), "Cid", "Cname", 客戶資料.客戶分類);
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        [Action執行時間]
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error2")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo客戶資料.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [Action執行時間]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo客戶資料.Find(id);
            repo客戶資料.Delete(客戶資料);
            repo客戶資料.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo客戶資料.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }





    }
}
