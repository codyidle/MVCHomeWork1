namespace CodyMVC5HomeWork1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人 : IValidatableObject
    {
        客戶資料Entities db = new 客戶資料Entities();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            
            if (Email != null && !是否刪除)
            {
                if (Id != 0)  //Edit
                {
                    var data = db.Database.SqlQuery<客戶聯絡人>(@"SELECT * FROM 客戶聯絡人 WHERE Email=@p0 AND 客戶Id = @p1 AND Id<> @p2 AND 是否刪除=0 ", Email, 客戶Id, Id);
                    if (data.CountAsync().Result > 0)
                    {
                        yield return new ValidationResult("該客戶聯絡人已有相同Email存在", new string[] { "Email" });
                    }

                }
                else
                {
                    var data = db.Database.SqlQuery<客戶聯絡人>(@"SELECT * FROM 客戶聯絡人 WHERE Email=@p0 AND 客戶Id = @p1 AND 是否刪除=0 ", Email, 客戶Id);
                    if (data.CountAsync().Result > 0)
                    {
                        yield return new ValidationResult("該客戶聯絡人已有相同Email存在", new string[] { "Email" });
                    }


                }
            }
        }
    }

    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int 客戶Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 姓名 { get; set; }
        
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        [Required]
        [EmailAddress(ErrorMessage = "非電子郵件格式")]
        public string Email { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [手機格式驗證(ErrorMessage = "手機格式錯誤(e.g. 0911-111111)")]
        public string 手機 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
    
        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
