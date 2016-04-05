using System;
using System.Linq;
using System.Collections.Generic;
	
namespace CodyMVC5HomeWork1.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        public 客戶銀行資訊 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(銀 => 銀.是否刪除 == false && 銀.客戶資料.是否刪除 == false);
        }

        public override void Add(客戶銀行資訊 entity)
        {
            base.Add(entity);
        }

        public override void Delete(客戶銀行資訊 entity)
        {
            entity.是否刪除 = true;
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}