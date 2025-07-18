using Firebase.Storage;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FUEM.Domain.Enums;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Hosting;

namespace FUEM.Infrastructure.Common
{
    /// <summary>
    /// Firebase is Linux base so it use the "/" for path
    /// </summary>
    public class FirebaseStorageService
    {
        private readonly string _bucketName = "fu-event-management.firebasestorage.app";
        private readonly FirebaseApp _firebaseApp;
        private readonly FirebaseStorage _storage;
        //private readonly StorageClient _storageClient;
        //private readonly UrlSigner _urlSigner;
        private readonly FirebaseAuth _auth;

        public FirebaseStorageService(IHostEnvironment env)
        {
            string serviceAccountPath = Path.Combine(AppContext.BaseDirectory, $"fu-event-management-firebase-admin-service-account.{env.EnvironmentName}.json");

            if (FirebaseApp.DefaultInstance == null)
            {
                _firebaseApp = FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(serviceAccountPath),
                });
            }

            var credential = GoogleCredential.FromFile(serviceAccountPath);
            //_storageClient = StorageClient.Create(credential);
            //_urlSigner = UrlSigner.FromCredentialFile(serviceAccountPath);
            _auth = FirebaseAuth.DefaultInstance;
            _storage = new FirebaseStorage(_bucketName);
        }

        public async Task<string> GetSignedFileUrlAsync(string fileName)
        {
            try
            {
                FirebaseStorageReference? reference = GetStorageReference(fileName);
                string signedUrl = await reference.GetDownloadUrlAsync();
                return signedUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return "";
        }

        public async Task<string> UploadFileAsync(FileType fileType, Stream fileStream, string originFileName)
        {
            try
            {
                var (reference,  uploadFilePath) = GetStorageReference(fileType, originFileName);

                // implement cancellation token after
                await reference.PutAsync(fileStream);
                Console.WriteLine($"path 1123: {uploadFilePath}");
                return uploadFilePath;
            }
            catch (FirebaseStorageException ex)
            {
                Console.WriteLine("From console: " + ex);
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                var reference = GetStorageReference(filePath);
                await reference.DeleteAsync();
                return true;
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex);
                return false;
            }
        }

        // For CREATE
        private (FirebaseStorageReference? reference, string referencePath) GetStorageReference(FileType fileType, string originFileName)
        {
            FirebaseStorageReference? reference = _storage.Child(fileType.Location);
            string? fileExtension = Path.GetExtension(originFileName);
            Guid id = Guid.NewGuid();
            string uploadFileName = $"{id.ToString()}_{originFileName}";
            reference?.Child(uploadFileName);
            return (reference, $"{fileType.Location}/{uploadFileName}");
        }

        // For GET
        private FirebaseStorageReference? GetStorageReference(string filePath)
        {
            string[] paths = filePath.Split("/");
            FirebaseStorageReference? reference = _storage.Child(paths[0]);

            for (int i = 1; i < paths.Length; i++)
            {
                reference = reference.Child(paths[i]);
            }
            return reference;
        }
    }
}
