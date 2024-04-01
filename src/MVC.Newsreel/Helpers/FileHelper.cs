namespace MVC.Newsreel.Helper;

public class FileHelper
{
    public static async Task<string> Upload(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}__{file.FileName}.docx";
        var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        string fullPath = "uploads\\" + fileName;
        using (var stream = File.Create(fullPath))
        {
            await file.CopyToAsync(stream);
        }

        return fileName;
    }
}