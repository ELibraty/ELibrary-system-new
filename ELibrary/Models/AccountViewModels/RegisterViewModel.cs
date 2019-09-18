using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Моля въведете правилен email адрес!")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Моля въведете потребителско име!")]
        [Display(Name = "Потребителко име")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Дължината на паролата {0} трябва да бъде между {2} и {1} знака!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повтори паролата")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат!")]
        public string ConfirmPassword { get; set; }
        
        [BindProperty, Required]
        public string Type { get; set; }
    }
}
