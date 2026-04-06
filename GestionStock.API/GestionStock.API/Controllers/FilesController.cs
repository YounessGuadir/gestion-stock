using Microsoft.AspNetCore.Mvc;

namespace GestionStock.API.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/products");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileUrl = $"uploads/products/{fileName}";

        return Ok(fileUrl); // ✅ يرجع string
    }
}