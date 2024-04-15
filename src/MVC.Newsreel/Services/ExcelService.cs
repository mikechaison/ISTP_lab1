using System.Globalization;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;
using MVC.Newsreel.Data;
namespace MVC.Newsreel.Services;

public interface IImportService<TEntity>
where TEntity : class
{
    Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
}

public interface IExportService<TEntity>
where TEntity : class
{
    Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
}

public interface IDataPortServiceFactory<TEntity>
where TEntity : class
{
    IImportService<TEntity> GetImportService(string contentType);
    IExportService<TEntity> GetExportService(string contentType);
}

public class ArticleImportService : IImportService<Article>
{
    private readonly Lab1dbContext context;
    public ArticleImportService(Lab1dbContext context)
    {
        this.context = context;
    }
    public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanRead)
        {
            throw new ArgumentException("Stream is not readable", nameof(stream));
        }
        using var workBook = new XLWorkbook(stream);
        var worksheet = workBook.Worksheets.FirstOrDefault();
        if (worksheet is null)
        {
            return;
        }
        foreach (var rows in worksheet.RowsUsed().Skip(1))
        {
            await AddArticleAsync(rows, cancellationToken);
        }
        await context.SaveChangesAsync(cancellationToken);
    }


    private async Task AddArticleAsync(IXLRow row, CancellationToken cancellationToken)
    {
        var articleTitle = GetArticleTitle(row);
        var articleText = GetArticleText(row);
        var article = await context.Articles.FirstOrDefaultAsync(article
        => article.Title.Contains(articleTitle), cancellationToken);
        if (article is null)
        {
            article = new Article
            {
                Title = articleTitle, 
                Text = articleText, 
                PubDate = DateTime.UtcNow
            };
            context.Add(article);
        }
        if (article.Category is null)
        {
            var category = await GetCategoryAsync(row, cancellationToken);
            article.Category = category;
            article.CategoryId = category.CategoryId;
        }
    }

    private static string GetArticleTitle(IXLRow row)
    {
        return row.Cell(1).GetValue<string>();
    }

    private static string GetArticleText(IXLRow row)
    {
        return row.Cell(2).GetValue<string>();
    }

    private async Task<Category> GetCategoryAsync(IXLRow row, CancellationToken cancellationToken)
    {
        var name = row.Cell(3).GetValue<string>();
        Category? category = await context.Categories.FirstOrDefaultAsync(category =>
        category.Name == name, cancellationToken);
        if (category is null)
        {
            category = new Category{
                Name = name
            };
            context.Add(category);
            return category;
        }
        return category;
    }

}

public class ArticleDataPortServiceFactory
: IDataPortServiceFactory<Article>
{
    private readonly Lab1dbContext lab1DbContext;
    public ArticleDataPortServiceFactory(Lab1dbContext lab1DbContext)
    {
        this.lab1DbContext = lab1DbContext;
    }
    public IExportService<Article> GetExportService(string contentType)
    {
        if (contentType is "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet")
        {
            return new ArticleExportService(lab1DbContext);
        }
        throw new NotImplementedException($"No export service implemented for articles with content type {contentType}");
    }
    public IImportService<Article> GetImportService(string contentType)
    {
        if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            return new ArticleImportService(lab1DbContext);
        }
        throw new NotImplementedException($"No import service implemented for articles with content type {contentType}");
    }
}

public class ArticleExportService : IExportService<Article>
{
    private const string RootWorksheetName = "Articles";
    private static readonly IReadOnlyList<string> HeaderNames = new string[]
    {
        "Назва",
        "Посилання",
        "Дата",
        "Автор",
        "Категорія"
    };
    private readonly Lab1dbContext context;
    private static void WriteHeader(IXLWorksheet worksheet)
    {
        for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
        {
            worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
        }
        worksheet.Row(1).Style.Font.Bold = true;
    }
    private static void WriteArticle(IXLWorksheet worksheet, Article article, int rowIndex)
    {
        var columnIndex = 1;
        worksheet.Cell(rowIndex, columnIndex++).Value = article.Title;
        worksheet.Cell(rowIndex, columnIndex).Value = "http://localhost:5166/Article/Details/"+article.ArticleId.ToString();
        worksheet.Cell(rowIndex, columnIndex).SetHyperlink(new XLHyperlink("http://localhost:5166/Article/Details/"+article.ArticleId.ToString()));
        columnIndex++;
        worksheet.Cell(rowIndex, columnIndex++).Value = article.PubDate;
        if (article.Author != null){
            worksheet.Cell(rowIndex, columnIndex).Value = article.Author.Name;
        }
        if (article.Category != null){
            worksheet.Cell(rowIndex, columnIndex+1).Value = article.Category.Name;
        }
    }
    private static void WriteArticles(IXLWorksheet worksheet,
    ICollection<Article> articles)
    {
        WriteHeader(worksheet);
        int rowIndex = 2;
        foreach (var article in articles)
        {
            WriteArticle(worksheet, article, rowIndex);
            rowIndex += 1;
        }
    }
    public ArticleExportService(Lab1dbContext context)
    {
        this.context = context;
    }
    public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanWrite)
        {
            throw new ArgumentException("Input stream is not writable");
        }
        var articles = await context.Articles
        .Include(article => article.Author)
        .Include(article => article.Category)
        .ToListAsync(cancellationToken);
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(RootWorksheetName);
        WriteArticles(worksheet, articles);
        workbook.SaveAs(stream);
    }
}