using System;
using System.Collections.Generic;

namespace MVC.Domain;

public partial class Comment
{
    public int CommentId { get; set; }

    public string? Text { get; set; }

    public int AuthorId { get; set; }

    public int ArticleId { get; set; }

    public int Likes { get; set; }

    public int Dislikes { get; set; }

    public DateTime PubDate { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual User Author { get; set; } = null!;

    public virtual ICollection<Dislike> DislikesNavigation { get; set; } = new List<Dislike>();

    public virtual ICollection<Like> LikesNavigation { get; set; } = new List<Like>();
}
