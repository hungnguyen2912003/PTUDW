using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
	[Table("Products")]
	public class Products
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Mã loại sản phẩm không được để trống!")]
		[Display(Name = "Mã loại SP")]
		public int CatID { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được để trống!")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Mã nhà cung cấp không được để trống!")]
        [Display(Name = "Mã NCC")]
        public string Supplier { get; set; }
        [Display(Name = "Liên kết")]
        public string Slug { get; set; }
        [Required(ErrorMessage = "Chi tiết sản phẩm không được để trống!")]
        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }
        [Display(Name = "Ảnh SP")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Giá sản phẩm không được để trống!")]
        [Display(Name = "Giá gốc")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Giá giảm sản phẩm không được để trống!")]
        [Display(Name = "Giá giảm")]
        public decimal SalePrice { get; set; }
        [Required(ErrorMessage = "Số lượng sản phẩm không được để trống!")]
        [Display(Name = "Số lượng")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Phần mô tả không được để trống!")]
        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }
		[Required(ErrorMessage = "Phần từ khoá không được để trống!")]
        [Display(Name = "Từ khoá")]
		public string MetaKey { get; set; }
        [Display(Name = "Người tạo")]
        public int CreateBy { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }
        [Display(Name = "Người cập nhật")]
        public int UpdateBy { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime UpdateAt { get; set; }
        [Required(ErrorMessage = "Trạng thái không được để trống!")]
        [Display(Name = "Trạng thái")]
        public int Status { get; set; }
	}
}
