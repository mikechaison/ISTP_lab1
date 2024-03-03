using System;
using System.Collections.Generic;

namespace MVC.Newsreel.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public string? Text { get; set; }

    public int AuthorId { get; set; }

    public int ArticleId { get; set; }

    public int? Likes { get; set; } = null!;

    public int? Dislikes { get; set; } = null!;

    public DateTime? PubDate { get; set; } = null!;

    public virtual Article? Article { get; set; } = null!;

    public virtual User? Author { get; set; } = null!;

    public virtual ICollection<Like> LikesNavigation { get; set; } = new List<Like>();
}
