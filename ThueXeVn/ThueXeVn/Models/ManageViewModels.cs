using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace ThueXeVn.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    public class Notification
    {
        [Required(ErrorMessage="Vui lòng chọn người gửi")]
        [Display(Name = "Chọn người gửi")]
        public int tobject { get; set; }
        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Tiêu đề gửi")]
        public string title { get; set; }
        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Nội dung gửi")]
        public string body { get; set; }
    }

    public class UpdatePassTaiXe
    {
        public long id_taixe { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} phải ít nhất {2} ký tự độ dài.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
    }

    public partial class bookingVM
    {
        public long id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public System.DateTime? date_time { get; set; }
        public System.DateTime? date_from { get; set; }
        public System.DateTime? date_to { get; set; }
        public string car_hire_type { get; set; }
        public string car_from { get; set; }
        public string car_to { get; set; }        
        public System.Byte? status { get; set; }
        public int? status2 { get; set; }
    }

    public partial class ctvVM
    {
        public long ctv_id { get; set; }
        public string ctv_fullname { get; set; }
        public string ctv_phone { get; set; }
        public string ctv_email { get; set; }
        public int? point_share { get; set; }
        public System.DateTime? date_create { get; set; }
        public bool? status { get; set; }
    }

    public class timbanggia
    {
        public string lat1 { get; set; }
        public string lng1 { get; set; }
        public string lat2 { get; set; }
        public string lng2 { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string loaixe { get; set; }
        public string tuyen { get; set; }
        public string kc { get; set; }
        public string gia { get; set; }
        public string gialuudem { get; set; }
    }

    public class getbanggia1
    {
        public int gialuudem { get; set; }
        public int giaxe { get; set; }        
    }

}