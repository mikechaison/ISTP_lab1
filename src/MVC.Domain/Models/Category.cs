using System;
using System.Collections.Generic;

namespace MVC.Domain;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ArticleDraft> ArticleDrafts { get; set; } = new List<ArticleDraft>();

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
