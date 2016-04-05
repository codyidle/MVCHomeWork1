using System;
using System.Linq;
using System.Collections.Generic;
	
namespace CodyMVC5HomeWork1.Models
{   
	public  class 客戶資料數清單Repository : EFRepository<客戶資料數清單>, I客戶資料數清單Repository
	{

	}

	public  interface I客戶資料數清單Repository : IRepository<客戶資料數清單>
	{

	}
}