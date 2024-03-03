using System;
using System.Collections.Generic;

namespace MVC.Newsreel.Data;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ArticleDraft> ArticleDrafts { get; set; } = new List<ArticleDraft>();

    public virtual ICollection<ArticleRequest> ArticleRequests { get; set; } = new List<ArticleRequest>();

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
}
