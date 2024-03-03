using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Newsreel.Data;

public partial class ArticleDraft
{
    public int ArticleDraftId { get; set; }

    public string Title { get; set; } = null!;

    public int? AuthorId { get; set; } = null;

    public int? SuggestedCategoryId { get; set; } = null;

    public string Text { get; set; } = null!;
    
    public string? Image { get; set; } = null!;

    [NotMapped]
    [Required]
    public IFormFile? ImageFile {get; set;} = null;

    public virtual ICollection<ArticleRequest> ArticleRequests { get; set; } = new List<ArticleRequest>();

    public virtual User? Author { get; set; } = null!;

    public virtual Category? SuggestedCategory { get; set; } = null!;
}
