using System;
using System.Linq;
using System.Collections.Generic;
	
namespace CodyMVC5HomeWork1.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public 客戶資料 Find (int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(客 => 客.是否刪除== false);
        }

        public override void Add(客戶資料 entity)
        {
            base.Add(entity);
        }

        public override void Delete(客戶資料 entity)
        {
            entity.是否刪除 = true;
        }

        //public SortedList<string, string> CatgoryList()
        //{
        //    SortedList<string, string> list = new SortedList<string, string>();
        //    list.Add("北區", "北區");
        //    list.Add("中區", "中區");
        //    list.Add("南區", "南區");

        //    return list;
        //}

        public List<客戶資料.客戶Category> CatgoryList()
        {
            var category = new List<客戶資料.客戶Category>();
            category.Add(new 客戶資料.客戶Category() { Cid = "", Cname = "選擇分類" });
            category.Add(new 客戶資料.客戶Category() { Cid = "1", Cname = "北區" });
            category.Add(new 客戶資料.客戶Category() { Cid = "2", Cname = "中區" });
            category.Add(new 客戶資料.客戶Category() { Cid = "3", Cname = "南區" });

            return category;
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}