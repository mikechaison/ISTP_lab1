using System;
using System.Collections.Generic;

namespace MVC.Newsreel.Data;

public partial class ArticleRequest
{
    public int ArticleRequestId { get; set; }

    public int? AuthorId { get; set; }

    public int? ArticleDraftId { get; set; }

    public int? ArticleId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Article? Article { get; set; }

    public virtual ArticleDraft? ArticleDraft { get; set; }

    public virtual User? Author { get; set; } = null!;
}
