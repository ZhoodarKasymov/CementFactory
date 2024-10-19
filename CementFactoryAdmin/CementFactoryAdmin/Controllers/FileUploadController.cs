using Microsoft.AspNetCore.Mvc;

namespace CementFactoryAdmin.Controllers;

public class FileUploadController : Controller
{
    private readonly string _savePath = @"D:\CamScreenshots"; // Folder to save files

    [HttpPost("api/file-upload/upload")]
    public async Task<IActionResult> UploadFile(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("Файл не получен!");
        }

        try
        {
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }

            // Generate a unique filename using GUID
            var fileName = $"{Guid.NewGuid():N}.jpg";
            var filePath = Path.Combine(_savePath, fileName);

            // Save the file to disk
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка сервера!");
        }
    }
}