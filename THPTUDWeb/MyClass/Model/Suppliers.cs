﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
	[Table("Suppliers")]
	public class Suppliers
	{
		[Key]
		public int Id { get; set; }
        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
        [Display(Name = "Tên nhà cung cấp")]
        public string Name { get; set; }
        [Display(Name = "Hình ảnh nhà cung cấp")]
        public string Image {  get; set; }
        [Display(Name = "Liên kết")]
        public string Slug { get; set; }
        [Display(Name = "Sắp xếp")]
        public int? Order { get; set; }
        [Display(Name = "Tên đầy đủ")]
        public string Fullname {  get; set; }
        [Display(Name = "Số điện thoại")]
        public string Phone {  get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mô tả không được để trống")]
        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }
        [Required(ErrorMessage = "Từ khoá không được để trống")]
        [Display(Name = "Từ khoá")]
        public string MetaKey { get; set; }
        [Required(ErrorMessage = "Người tạo không được để trống")]
        [Display(Name = "Người tạo")]
        public int CreateBy { get; set; }
        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }
        [Required(ErrorMessage = "Người cập nhật không được để trống")]
        [Display(Name = "Người cập nhật")]
        public int UpdateBy { get; set; }
        [Required(ErrorMessage = "Ngày cập nhật không được để trống")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày cập nhật")]
        public DateTime UpdateAt { get; set; }
        [Display(Name = "Trạng thái")]
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public int Status { get; set; }
	}
}
