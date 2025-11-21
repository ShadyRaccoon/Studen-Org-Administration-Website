using Microsoft.AspNetCore.Mvc;
using StudentOrg_A4_Website.Services;

namespace StudentOrg_A4_Website.Controllers
{
    public class PictureController : Controller
    {
        private readonly GoogleDriveServices _googleDriveService;
        private readonly ILogger<PictureController> _logger;

        public PictureController(GoogleDriveServices googleDriveService, ILogger<PictureController> logger)
        {
            _googleDriveService = googleDriveService;
            _logger = logger;
        }

        [HttpGet("/image/{fileId}")]
        public async Task<IActionResult> GetImage(string fileId)
        {
            try
            {
                if (string.IsNullOrEmpty(fileId))
                {
                    return BadRequest("File ID is required");
                }

                var stream = await _googleDriveService.GetImageStreamAsync(fileId);
                return File(stream, "image/jpeg");
            }
            catch (FileNotFoundException)
            {
                _logger.LogWarning($"Image not found: {fileId}");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving image {fileId}: {ex.Message}");
                return StatusCode(500, "Error retrieving image");
            }
        }
    }
}
