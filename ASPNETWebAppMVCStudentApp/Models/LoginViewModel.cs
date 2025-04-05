﻿//namespace ASPNETWebAppMVCStudentApp.Models
//{
//    public class LoginViewModel
//    {
//        public string Username { get; set; }
//        public string Password { get; set; }
//    }
//}


using System.ComponentModel.DataAnnotations;

namespace ASPNETWebAppMVCStudentApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
