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
                    .CreateScoped(DriveService.Scope.DriveReadonly);

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
    }
}
