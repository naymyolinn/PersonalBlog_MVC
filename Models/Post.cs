using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace personalblog.Models
{
    public enum PostStatus
    {
        Draft = 0,
        Published = 1,
        Unpublished = 2
    }
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Subtitle { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Category")]
        public int Categoryid { get; set; }
        public byte Status { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }

        [Required]
        [StringLength(450)]
        public string AuthorId { get; set; }
        public virtual IdentityUser Author { get; set; }
    }
}