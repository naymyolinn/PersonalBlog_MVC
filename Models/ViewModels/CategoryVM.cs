using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace personalblog.Models.ViewModels
{
    public class CategoryVM
    {
        public int Cateid { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Categoryname { get; set; }
    }
}