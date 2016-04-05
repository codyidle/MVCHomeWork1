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
    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}