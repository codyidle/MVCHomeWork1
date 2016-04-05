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
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}