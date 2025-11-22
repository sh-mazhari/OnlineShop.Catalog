using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Common.FileStorage
{
    public interface IFileStorageService : ITransientService
    {
        public Task<FileSaveResultDto?> UploadAsync<T>(IFormFile file, FileType supportedFileType, CancellationToken cancellationToken = default)
        where T : class;

        public void Remove(string? path);
    }
}
