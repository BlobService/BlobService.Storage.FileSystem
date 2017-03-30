using BlobService.Core.Configuration;
using BlobService.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Storage.FileSystem.Configuration
{
    public static class IBlobServiceBuilderExtensions
    {
        public static IBlobServiceBuilder AddFileSystemStorageService(this IBlobServiceBuilder builder, Action<FileSystemStorageOptions> setupAction = null)
        {
            var fileSystemStorageOptions = new FileSystemStorageOptions();
            setupAction?.Invoke(fileSystemStorageOptions);
            fileSystemStorageOptions.TryValidate();

            builder.Services.AddSingleton(fileSystemStorageOptions);

            builder.Services.AddScoped<IStorageService, FileSystemStorageService>();

            return builder;
        }
    }
}
