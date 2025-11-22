using Catalog.Application.Common.FileStorage;
using Catalog.Domain.Core;
using Catalog.Domain.Core.Common;
using Catalog.Domain.Core.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Services.CategoryCQRS
{
    public class CreateCategoryRequest : IRequest<Guid>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public IFormFile Thumbnail { get; set; }
        public List<Guid> Features { get; set; }
    }

    public class CreateProductRequestHandler : IRequestHandler<CreateCategoryRequest, Guid>
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IFileStorageService _file;

        public CreateProductRequestHandler(IGenericRepository<Category> categoryRepository, IFileStorageService file) =>
            (_categoryRepository, _file) = (categoryRepository, file);

        //public CreateProductRequestHandler(Domain.Core.SeedWork.Repository.IRepository<Category> categoryRepository, IFileStorageService file)
        //{
        //    _categoryRepository = categoryRepository;
        //    _file = file;
        //}

        public async Task<Guid> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var thumbnailSaveResult = await _file.UploadAsync<Category>(request.Thumbnail, FileType.Image, cancellationToken);

            var category = Category.CreateNew(request.Name, request.IsActive, request.Description, request.Features,
                thumbnailSaveResult?.FilePath, thumbnailSaveResult?.FileName, thumbnailSaveResult?.Extension, thumbnailSaveResult?.Size);

            await _categoryRepository.AddAsync(category, cancellationToken);

            return category.Id.Value;
        }
    }
}
