using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlobService.Storage.FileSystem
{
    public class FileSystemStorageOptions
    {
        public string RootPath { get; set;  }

        public void TryValidate()
        {
            if (string.IsNullOrEmpty(RootPath)) throw new ArgumentNullException(RootPath);
            if (IsDirectoryWritable(RootPath) == false) throw new Exception($"Configured RootPath doesn't exist or it isn't writable.");
        }

        private bool IsDirectoryWritable(string dirPath)
        {
            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        dirPath,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                )
                { }
                return true;
            }
            catch
            {
                    return false;
            }
        }
    }
}
