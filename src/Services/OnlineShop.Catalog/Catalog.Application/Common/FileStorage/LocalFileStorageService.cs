using Catalog.Application.Common.Extensions;
using Catalog.Domain.Core.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Catalog.Application.Common.FileStorage
{
    public class LocalFileStorageService : IFileStorageService
    {
        public async Task<FileSaveResultDto?> UploadAsync<T>(IFormFile file, FileType supportedFileType, CancellationToken cancellationToken = default)
        where T : class
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            if (Path.GetExtension(file.FileName) is null || !supportedFileType.GetDescriptionList().Contains(Path.GetExtension(file.FileName).ToLower()))
                throw new InvalidOperationException("File Format Not Supported.");
            if (file.Name is null)
                throw new InvalidOperationException("Name is required.");

            using (var streamData = new MemoryStream())
            {
                file.CopyTo(streamData);
                if (streamData.Length > 0)
                {
                    var fileSaveResult = new FileSaveResultDto();

                    string folder = typeof(T).Name;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        folder = folder.Replace(@"\", "/");
                    }

                    string folderName = supportedFileType switch
                    {
                        FileType.Image => Path.Combine("Files", "Images", folder),
                        _ => Path.Combine("Files", "Others", folder),
                    };
                    string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    Directory.CreateDirectory(pathToSave);

                    string fileName = file.FileName.Trim('"');
                    fileName = RemoveSpecialCharacters(fileName);
                    fileName = fileName.ReplaceWhitespace("-");
                    fileName += Path.GetExtension(file.FileName).Trim();
                    string fullPath = Path.Combine(pathToSave, fileName);
                    string dbPath = Path.Combine(folderName, fileName);
                    if (File.Exists(dbPath))
                    {
                        dbPath = NextAvailableFilename(dbPath);
                        fullPath = NextAvailableFilename(fullPath);
                    }

                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await streamData.CopyToAsync(stream, cancellationToken);

                    fileSaveResult.FilePath = dbPath.Replace("\\", "/");
                    fileSaveResult.Extension = Path.GetExtension(file.FileName).ToLower();
                    fileSaveResult.FileName = fileName;

                    return fileSaveResult;
                }
                else
                {
                    return null;
                }
            }


        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
        }

        public void Remove(string? path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private const string NumberPattern = "-{0}";

        private static string NextAvailableFilename(string path)
        {
            if (!File.Exists(path))
            {
                return path;
            }

            if (Path.HasExtension(path))
            {
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal), NumberPattern));
            }

            return GetNextFilename(path + NumberPattern);
        }

        private static string GetNextFilename(string pattern)
        {
            string tmp = string.Format(pattern, 1);

            if (!File.Exists(tmp))
            {
                return tmp;
            }

            int min = 1, max = 2;

            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (File.Exists(string.Format(pattern, pivot)))
                {
                    min = pivot;
                }
                else
                {
                    max = pivot;
                }
            }

            return string.Format(pattern, max);
        }
    }

}
