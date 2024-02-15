using System;
using System.Collections.Generic;

namespace MVC.Domain;

public partial class Article
{
    public int ArticleId { get; set; }

    public string Title { get; set; } = null!;

    public int CategoryId { get; set; }

    public int AuthorId { get; set; }

    public string Text { get; set; } = null!;

    public int Likes { get; set; }

    public int Dislikes { get; set; }

    public DateTime PubDate { get; set; }

    public virtual ICollection<ArticleRequest> ArticleRequests { get; set; } = new List<ArticleRequest>();

    public virtual User Author { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Dislike> DislikesNavigation { get; set; } = new List<Dislike>();

    public virtual ICollection<Like> LikesNavigation { get; set; } = new List<Like>();
}
