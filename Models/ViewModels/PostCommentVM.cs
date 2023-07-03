using System;
using System.ComponentModel.DataAnnotations;

namespace personalblog.Models.ViewModels
{
    public class PostCommentVM
    {
        public Post Post { get; set; }
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public string CommentText { get; set; }
    }
}