using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodyMVC5HomeWork1.Models
{
    public class 客戶聯絡人ViewModel
    {
        
        public 客戶資料 CustomerData { get; set; }

        public IEnumerable<客戶聯絡人> ContactsData { get; set; }

        public int Id { get; set; }
        public int 客戶Id { get; set; }
        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }
        public string 姓名 { get; set; }
        public string Email { get; set; }
        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [手機格式驗證(ErrorMessage = "手機格式錯誤(e.g. 0911-111111)")]
        public string 手機 { get; set; }
        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
    }
}