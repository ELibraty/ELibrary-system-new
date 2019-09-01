using ELibrary.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Models.ViewModels
{
    public class IndexViewModel
    {
        public RegisterViewModel RegisterViewModel { get; set; }

        public LoginViewModel LoginViewModel { get; set; }

    }
}
