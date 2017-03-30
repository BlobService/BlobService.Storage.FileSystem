using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Storage.FileSystem
{
    public class FileSystemStorageOptions
    {
        public string RootPath { get; set;  }

        public void TryValidate()
        {
            if (string.IsNullOrEmpty(RootPath)) throw new ArgumentNullException(RootPath);
        }
    }
}
