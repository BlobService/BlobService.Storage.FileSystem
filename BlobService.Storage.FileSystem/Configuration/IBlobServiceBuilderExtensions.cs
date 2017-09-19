using BlobService.Core.Configuration;
using BlobService.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlobService.Storage.FileSystem.Configuration
{
    public static class IBlobServiceBuilderExtensions
    {
        public static IBlobServiceBuilder AddFileSystemStorageService<T>(this IBlobServiceBuilder builder, Action<FileSystemStorageOptions> setupAction = null) where T : FileSystemStorageService
        {
            var fileSystemStorageOptions = new FileSystemStorageOptions();
            setupAction?.Invoke(fileSystemStorageOptions);
            fileSystemStorageOptions.TryValidate();

            builder.Services.AddSingleton(fileSystemStorageOptions);

            builder.Services.AddScoped<IStorageService, T>();

            return builder;
        }
    }
}
