using System;
using System.Linq;
using System.Collections.Generic;


namespace CodyMVC5HomeWork1.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<客戶聯絡人> Query(string keyword ,string job)
        {
            var data = this.All();

            if (keyword != null && keyword != "")
                data = data.Where(聯 => 聯.姓名.Contains(keyword));


            if (job != null && job != "")
                data = data.Where(聯 => 聯.職稱 == job);


            return data;
        }


        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(聯 => 聯.是否刪除 == false && 聯.客戶資料.是否刪除 == false);
        }

        public override void Add(客戶聯絡人 entity)
        {
            base.Add(entity);
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否刪除 = true;
        }

        public List<string> JobList()
        {
            var joblist = new List<string>();
            //joblist.Add("");

            var contacts = (from 聯 in this.All() select 聯.職稱).Distinct().OrderBy(p => p);

            foreach (var  item in contacts)
            {
                joblist.Add(item);
            }

            return joblist;
        }
    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}