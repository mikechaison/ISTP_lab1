using System;
using System.Collections.Generic;

namespace MVC.Domain;

public partial class ArticleDraft
{
    public int ArticleDraftId { get; set; }

    public string Title { get; set; } = null!;

    public int AuthorId { get; set; }

    public int SuggestedCategoryId { get; set; }

    public string Text { get; set; } = null!;

    public virtual ICollection<ArticleRequest> ArticleRequests { get; set; } = new List<ArticleRequest>();

    public virtual User Author { get; set; } = null!;

    public virtual Category SuggestedCategory { get; set; } = null!;
}
