//Id
//CategoryName
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace personalblog.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cateid { get; set; }

        [Required(ErrorMessage = "Category Name field is required.")]
        [StringLength(50)]
        [Display(Name = "Category Name")]
        public string Categoryname { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        [Required]
        [StringLength(450)]
        public string AuthorId { get; set; }
        // public virtual User Author { get; set; }
    }
}