using Google.Apis.Services;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;

namespace StudentOrg_A4_Website.Services
{
    public class GoogleDriveServices
    {
        private readonly DriveService _driveService;
        private readonly ILogger<GoogleDriveServices> _logger;

        public GoogleDriveServices(IConfiguration configuration, ILogger<GoogleDriveServices> logger) 
        {
            _logger = logger;

            try
            {
                var keyPath = configuration["GoogleDrive:ServiceAccountKeyPath"];

                if (string.IsNullOrEmpty(keyPath))
                {
                    throw new InvalidOperationException("GoogleDrive:ServiceAccountKeyPath is not configured properly in appsettings.json");
                }

                var fullPath = Path.IsPathRooted(keyPath) ? keyPath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, keyPath);

                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"Service account key file not found at: {fullPath}");
                }

                var credential = GoogleCredential.FromFile(fullPath)
                    .CreateScoped(DriveService.Scope.Drive);

                _driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "StudentOrg A4 Website"
                });
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error initializing GoogleDriveService: {ex.Message}");
                throw;
            }
        }

        public async Task<Stream> GetImageStreamAsync(string fileId)
        {
            try
            {
                var request = _driveService.Files.Get(fileId);
                var stream = new MemoryStream();
                await request.DownloadAsync(stream);
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error downloading file {fileId}: {ex.Message}");
                throw;
            }
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName, string mimeType, string folderId = null)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = fileName,
                    MimeType = mimeType
                };

                if (!string.IsNullOrEmpty(folderId))
                {
                    fileMetadata.Parents = new List<string> { folderId };
                }

                var request = _driveService.Files.Create(fileMetadata, fileStream, mimeType);
                request.Fields = "id, name, webViewLink, webContentLink";

                var uploadProgress = await request.UploadAsync();

                if (uploadProgress.Status != Google.Apis.Upload.UploadStatus.Completed)
                {
                    throw new Exception($"Upload failed: {uploadProgress.Exception?.Message}");
                }

                var file = request.ResponseBody;
                _logger.LogInformation($"File uploaded successfully. ID: {file.Id}, Name: {file.Name}");

                return file.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file {fileName}: {ex.Message}");
                throw;
            }
        }
    }
}
