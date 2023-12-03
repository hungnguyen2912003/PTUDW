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
		[Display(Name = "Mã loại sản phẩm")]
		public int CatID { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được để trống!")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Mã nhà cung cấp không được để trống!")]
        [Display(Name = "Mã nhà cung cấp")]
        public int SupplierID { get; set; }
        [Display(Name = "Liên kết")]
        public string Slug { get; set; }
        [Display(Name = "Chi tiết sản phẩm")]
        [Required(ErrorMessage = "Chi tiết sản phẩm không được để trống!")]
        public string Detail { get; set; }
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Giá gốc sản phẩm không được để trống!")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Display(Name = "Giá gốc")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Giá bán sản phẩm không được để trống!")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Display(Name = "Giá giảm")]
        public decimal SalePrice { get; set; }
        [Required(ErrorMessage = "Số lượng sản phẩm không được để trống!")]
        [Display(Name = "Số lượng")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Mô tả không được để trống!")]
        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }
		[Required(ErrorMessage = "Từ khoá không được để trống!")]
        [Display(Name = "Từ khoá")]
		public string MetaKey { get; set; }
        [Required(ErrorMessage = "Người tạo không được để trống!")]
        [Display(Name = "Người tạo")]
        public int CreateBy { get; set; }
        [Required(ErrorMessage = "Ngày tạo không được để trống!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }
        [Display(Name = "Cập nhật bởi")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public int? UpdateBy { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdateAt { get; set; }
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [Display(Name = "Trạng thái")]
        public int Status { get; set; }
	}
}
