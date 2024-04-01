using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Newsreel.Data;
using MVC.Newsreel.Services;
using MVC.Newsreel.Helper;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using A = DocumentFormat.OpenXml.Drawing;

namespace MVC.Newsreel.Controllers_
{
    public class ArticleController : Controller
    {
        private readonly Lab1dbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ArticleDataPortServiceFactory _articleDataPortServiceFactory;

        public ArticleController(Lab1dbContext context,
                                IWebHostEnvironment webHostEnvironment,
                                ArticleDataPortServiceFactory articleDataPortServiceFactory)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _articleDataPortServiceFactory = articleDataPortServiceFactory;
        }

        // GET: Article
        public async Task<IActionResult> Index()
        {
            var lab1dbContext = _context.Articles.OrderByDescending(a => a.PubDate).Include(a => a.Author).Include(a => a.Category);
            return View(await lab1dbContext.ToListAsync());
        }

        // GET: Article/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .Include(a => a.Comments).ThenInclude(a => a.Author)
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            article.Likes=_context.Likes.Where(a => a.ArticleId == article.ArticleId).Where(a => !a.IsDis).Count();
            article.Dislikes=_context.Likes.Where(a => a.ArticleId == article.ArticleId).Where(a => a.IsDis).Count();
            foreach (var item in article.Comments)
            {
                item.Likes=_context.Likes.Where(a => a.CommentId == item.CommentId).Where(a => !a.IsDis).Count();
                item.Dislikes=_context.Likes.Where(a => a.CommentId == item.CommentId).Where(a => a.IsDis).Count();
            }
            if (article == null)
            {
                return NotFound();
            }

            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "Title");
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "Name");

            return View(article);
        }

        // GET: Article/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Article/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleId,Title,CategoryId,AuthorId,Text,Likes,Dislikes,PubDate,ImageFile")] Article article)
        {
            if (ModelState.IsValid)
            {
                if (article.ImageFile != null)
                {
                    string folder = "static/images/Article/";
                    folder += Guid.NewGuid().ToString() + "_" + article.ImageFile.FileName;
                    article.Image = "/"+folder;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                    await article.ImageFile.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                }
                article.PubDate=DateTime.UtcNow;
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "UserId", article.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", article.CategoryId);
            return View(article);
        }

        // GET: Article/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "Name", article.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", article.CategoryId);
            return View(article);
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,Title,CategoryId,AuthorId,Text,Likes,Dislikes,PubDate,ImageFile")] Article article)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (article.ImageFile != null)
                {
                    string folder = "static/images/Article/";
                    folder += Guid.NewGuid().ToString() + "_" + article.ImageFile.FileName ;
                    article.Image = "/"+folder;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                    await article.ImageFile.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                }
                try
                {
                    article.PubDate=DateTime.UtcNow;
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "UserId", article.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", article.CategoryId);
            return View(article);
        }

        // GET: Article/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }

        [HttpGet]
        public async Task<IActionResult> Export([FromQuery] string
        contentType = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet", CancellationToken cancellationToken = default)
        {
            var exportService = _articleDataPortServiceFactory.GetExportService(contentType);
            var memoryStream = new MemoryStream();
            await exportService.WriteToAsync(memoryStream, cancellationToken);
            await memoryStream.FlushAsync(cancellationToken);
            memoryStream.Position = 0;
            return new FileStreamResult(memoryStream, contentType)
            {
                FileDownloadName =$"articles_{DateTime.UtcNow}.xlsx"
            };
        }

        [HttpGet]
        public async Task<IActionResult> ExportDocx(int? id, [FromQuery] string
        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", CancellationToken cancellationToken = default)
        {
            /*var exportService = _articleDataPortServiceFactory.GetExportService(contentType);
            var memoryStream = new MemoryStream();
            await exportService.WriteToAsync(memoryStream, cancellationToken);
            await memoryStream.FlushAsync(cancellationToken);*/
            MemoryStream stream = new MemoryStream();

            var article = await _context.Articles
            .Include(a => a.Author)
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a => a.ArticleId == id);


            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, article.Image.Substring(1));
            byte[] imageData = System.IO.File.ReadAllBytes(filePath);

            // Create a Word document
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
            {
                // Add a main document part
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                // Create the document structure
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                Paragraph paragraph = body.AppendChild(new Paragraph());
                Run run = paragraph.AppendChild(new Run());
                run.AppendChild(new Text(article.Title));

                using (Stream img_stream = imagePart.GetStream())
                {
                    img_stream.Write(imageData, 0, imageData.Length);
                }

                string relationshipId = mainPart.GetIdOfPart(imagePart);

                Paragraph paragraph1 = new Paragraph(new Run(new Drawing(
                new Inline(
                    new Extent() { Cx = 4950000L, Cy = 3960000L },
                    new DocProperties() { Id = (UInt32Value)1U, Name = "Picture 1" },
                    new NonVisualGraphicFrameDrawingProperties(new A.GraphicFrameLocks() { NoChangeAspect = true }),
                    new A.Graphic(
                        new A.GraphicData(
                            new PIC.Picture( // Use PIC namespace for Picture element
                                new PIC.NonVisualPictureProperties(
                                    new PIC.NonVisualDrawingProperties() { Id = (UInt32Value)0U, Name = "New Bitmap Image.jpg" },
                                    new PIC.NonVisualPictureDrawingProperties()),
                                new PIC.BlipFill(
                                    new A.Blip() { Embed = relationshipId },
                                    new A.Stretch(new A.FillRectangle())),
                                new PIC.ShapeProperties(new A.Transform2D(new A.Offset() { X = 0L, Y = 0L }, new A.Extents() { Cx = 4950000L, Cy = 3960000L }),
                                    new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle }))
                            ) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                        )
                    )
                ));
                
                mainPart.Document.Body.Append(paragraph1);

                
                paragraph = body.AppendChild(new Paragraph());
                run = paragraph.AppendChild(new Run());
                run.AppendChild(new Text($"{article.PubDate} | {article.Author.Name} | {article.Category.Name}"));
                
                paragraph = body.AppendChild(new Paragraph());
                run = paragraph.AppendChild(new Run());
                run.AppendChild(new Text(article.Text));

                // Save changes to the document
                mainPart.Document.Save();
            }

            // Reset the stream position to the beginning
            stream.Position = 0;

            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName =$"article{article.ArticleId}_{DateTime.UtcNow}.docx"
            };
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ImportExcel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile articleFile,
        CancellationToken cancellationToken)
        {
            var importService = _articleDataPortServiceFactory.GetImportService(articleFile.ContentType);
            using var stream = articleFile.OpenReadStream();
            await importService.ImportFromStreamAsync(stream, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ImportDocx()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportDocx(IFormFile file,
        CancellationToken cancellationToken)
        {
            string fileName = await FileHelper.Upload(file);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

            var word = WordprocessingDocument.Open(path, true);
            var paragraphs = word.MainDocumentPart.Document.Body.Descendants<Paragraph>().ToList();
            var img = word.MainDocumentPart.ImageParts.FirstOrDefault();
            var title = paragraphs[0].InnerText;
            var text = "";
            for (int i = 1; i<paragraphs.Count(); i++)
            {
                text+=paragraphs[i].InnerText+"\n";
            }

            var article = new Article()
            {
                Title = title,
                Text = text,
                PubDate = DateTime.UtcNow
            };

            var stream = img.GetStream();

            IFormFile imageFile = new FormFile(stream, 0, stream.Length, 
            "name", "imported_"+Guid.NewGuid().ToString());

            article.ImageFile = imageFile;

            string folder = "static/images/Article/";
            folder += Guid.NewGuid().ToString() + "_" + article.ImageFile.FileName;
            article.Image = "/"+folder;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

            await article.ImageFile.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            
            _context.Add(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
