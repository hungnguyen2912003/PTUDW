using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
	[Table("Categories")]
	public class Categories
	{
		[Key]
		public int Id { get; set; }
		[Display(Name = "Tên loại sản phẩm")]
		[Required(ErrorMessage = "Tên loại sản phẩm không được để trống")]
		public string Name { get; set; }
		[Display(Name = "Liên kết")]
		public string Slug { get; set; }
		[Display(Name = "Cấp cha")]
		public int? ParentID { get; set; }
		[Display(Name = "Sắp xếp")]
		public int? Order { get; set; }
		[Required(ErrorMessage = "Phần mô tả không được để trống")]
		[Display(Name = "Mô tả")]
		public string MetaDesc { get; set; }
		[Required(ErrorMessage = "Phần từ khóa không được để trống")]
		[Display(Name = "Từ khoá")]
		public string MetaKey { get; set; }
		[Display(Name = "Người tạo")]
        [Required(ErrorMessage = "Người tạo không được để trống")]
        public int CreateBy { get; set; }
		[Display(Name = "Ngày tạo")]
        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreateAt { get; set; }
		[Display(Name = "Người cập nhật")]
		public int? UpdateBy { get; set; }
		[Display(Name = "Ngày cập nhật")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? UpdateAt { get; set; }
		[Display(Name = "Trạng thái")]
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public int Status { get; set; }
	}
}
