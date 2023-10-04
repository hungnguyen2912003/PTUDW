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
		[Display(Name = "Tên loại hàng")]
		[Required(ErrorMessage = "Tên loại sản phẩm không được để trống")]
		public string Name { get; set; }
		[Display(Name = "Tên rút gọn")]
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
		[Required(ErrorMessage = "Phần người tạo không được để trống")]
		[Display(Name = "Người tạo")]
		public int CreateBy { get; set; }
		[Required(ErrorMessage = "Phần ngày tạo không được để trống")]
		[Display(Name = "Ngày tạo")]
		public DateTime CreateAt { get; set; }
		[Required(ErrorMessage = "Phần người cập nhật không được để trống")]
		[Display(Name = "Người cập nhật")]
		public int UpdateBy { get; set; }
		[Required(ErrorMessage = "Phần ngày cập nhật không được để trống")]
		[Display(Name = "Ngày cập nhật")]
		public DateTime UpdateAt { get; set; }
		[Required(ErrorMessage = "Phần trạng thái không được để trống")]
		[Display(Name = "Trạng thái")]
		public int Status { get; set; }
	}
}
