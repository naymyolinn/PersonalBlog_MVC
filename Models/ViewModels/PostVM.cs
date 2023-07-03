using System;
using System.ComponentModel.DataAnnotations;


namespace personalblog.Models.ViewModels
{
    public class PostVM
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Subtitle { get; set; }
        [Required]
        public string Body { get; set; }

        [Display(Name = "Category")]
        public int Categoryid { get; set; }
        public virtual Category Category { get; set; }
    }
}