using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Newsreel.Data;

public partial class Article
{
    public int ArticleId { get; set; }

    public string Title { get; set; } = null!;

    public int? CategoryId { get; set; }

    public int? AuthorId { get; set; }

    public string Text { get; set; } = null!;

    public int? Likes { get; set; } = 0;

    public int? Dislikes { get; set; } = 0;

    public DateTime? PubDate { get; set; } = null;

    public string? Image { get; set; } = null;

    [NotMapped]
    public IFormFile? ImageFile {get; set;} = null;

    public virtual ICollection<ArticleRequest> ArticleRequests { get; set; } = new List<ArticleRequest>();

    public virtual User? Author { get; set; } = null!;

    public virtual Category? Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Like> LikesNavigation { get; set; } = new List<Like>();
}
