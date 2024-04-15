using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.Newsreel.Data;

public partial class Comment
{
    public int CommentId { get; set; }

    public string? Text { get; set; }

    public int? AuthorId { get; set; } = null!;

    public int? ArticleId { get; set; } = null!;

    public int Likes { get; set; } = 0;

    public int Dislikes { get; set; } = 0;

    public DateTime? PubDate { get; set; } = null!;

    public virtual Article? Article { get; set; } = null!;

    public virtual User? Author { get; set; } = null!;

    public virtual ICollection<Like> LikesNavigation { get; set; } = new List<Like>();
}
