using System;
using System.Collections.Generic;

namespace MVC.Newsreel.Models;

public partial class Article
{
    public int ArticleId { get; set; }

    public string Title { get; set; } = null!;

    public int? CategoryId { get; set; }

    public int? AuthorId { get; set; }

    public string Text { get; set; } = null!;

    public int? Likes { get; set; } = null;

    public int? Dislikes { get; set; } = null;

    public DateTime? PubDate { get; set; } = null;

    public virtual ICollection<ArticleRequest> ArticleRequests { get; set; } = new List<ArticleRequest>();

    public virtual User? Author { get; set; } = null!;

    public virtual Category? Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Dislike> DislikesNavigation { get; set; } = new List<Dislike>();

    public virtual ICollection<Like> LikesNavigation { get; set; } = new List<Like>();
}
