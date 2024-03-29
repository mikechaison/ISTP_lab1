﻿using System;
using System.Collections.Generic;

namespace MVC.Newsreel.Models;

public partial class Like
{
    public int LikeId { get; set; }

    public int? UserId { get; set; } = null!;

    public int? ArticleId { get; set; } = null!;

    public int? CommentId { get; set; } = null!;

    public bool IsDis { get; set; } = false;

    public virtual Article? Article { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual User? User { get; set; } = null!;
}
