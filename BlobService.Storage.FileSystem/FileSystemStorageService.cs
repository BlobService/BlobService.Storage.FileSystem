using BlobService.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Storage.FileSystem
{
    public class FileSystemStorageService : IStorageService
    {
        private FileSystemStorageOptions _options;
        public FileSystemStorageService(FileSystemStorageOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));

            if (!Directory.Exists(_options.RootPath))
            {
                Directory.CreateDirectory(_options.RootPath);
            }
        }

        public async Task<string> AddBlobAsync(string containerId, byte[] blob)
        {
            if (string.IsNullOrEmpty(containerId)) throw new ArgumentNullException(nameof(containerId));
            if (blob == null) throw new ArgumentNullException(nameof(blob));

            var containerFolder = Path.Combine(_options.RootPath, containerId);
            if (!Directory.Exists(containerFolder))
            {
                Directory.CreateDirectory(containerFolder);
            }

            var blobSubject = Guid.NewGuid().ToString();
            var blobPath = Path.Combine(containerFolder, blobSubject);

            if (File.Exists(blobPath))
            {
                throw new Exception("Guid.NewGuid resulted in a duplicate value.");
            }

            await WriteAllBytesAsync(blobPath, blob);

            return blobSubject;
        }

        public async Task DeleteBlobAsync(string containerId, string subject)
        {
            if (string.IsNullOrEmpty(containerId)) throw new ArgumentNullException(nameof(containerId));
            if (string.IsNullOrEmpty(subject)) throw new ArgumentNullException(nameof(subject));

            var containerFolder = Path.Combine(_options.RootPath, containerId);
            if (Directory.Exists(containerFolder))
            {
                var blobPath = Path.Combine(containerFolder, subject);
                if (File.Exists(blobPath))
                {
                    await Task.Run(() => { File.Delete(blobPath); });
                }
            }
        }

        public async Task<byte[]> GetBlobAsync(string containerId, string subject)
        {
            if (string.IsNullOrEmpty(containerId)) throw new ArgumentNullException(nameof(containerId));
            if (string.IsNullOrEmpty(subject)) throw new ArgumentNullException(nameof(subject));

            var containerFolder = Path.Combine(_options.RootPath, containerId);
            if (Directory.Exists(containerFolder))
            {
                var blobPath = Path.Combine(containerFolder, subject);
                if (File.Exists(blobPath))
                {
                    return await ReadAllBytesAsync(blobPath);
                }
            }
            return null;
        }

        public async Task<string> UpdateBlobAsync(string containerId, string subject, byte[] blob)
        {
            if (string.IsNullOrEmpty(containerId)) throw new ArgumentNullException(nameof(containerId));
            if (string.IsNullOrEmpty(subject)) throw new ArgumentNullException(nameof(subject));
            if (blob == null) throw new ArgumentNullException(nameof(blob));

            var containerFolder = Path.Combine(_options.RootPath, containerId);
            if (Directory.Exists(containerFolder))
            {
                var blobPath = Path.Combine(containerFolder, subject);
                if (File.Exists(blobPath))
                {
                    await WriteAllBytesAsync(blobPath, blob);
                }
            }
            return subject;
        }

        protected virtual async Task WriteAllBytesAsync(string path, byte[] blob)
        {
            using (FileStream sourceStream = new FileStream(path,
                FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(blob, 0, blob.Length);
            };
        }

        protected virtual async Task<byte[]> ReadAllBytesAsync(string path)
        {
            using (FileStream sourceStream = new FileStream(path,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                using (var memstream = new MemoryStream())
                {
                    await sourceStream.CopyToAsync(memstream);

                    return memstream.ToArray();
                }
            }
        }
    }
}
