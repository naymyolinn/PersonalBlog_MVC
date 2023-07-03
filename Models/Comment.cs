using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace personalblog.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Comid { get; set; }
        [Required]
        [Display(Name = "Comment Text")]
        public string CommentText { get; set; }
        [Required]
        public string Name { get; set; }

        public DateTime CommentDate { get; set; }

        public Guid Postid { get; set; }
        public virtual Post Posts { get; set; }

        // [Required]
        // [StringLength(450)]
        // public string AuthorId { get; set; }
        // public virtual IdentityUser Author { get; set; }


    }
}