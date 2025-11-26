using Catalog.Application.Common.FileStorage;
using Catalog.Domain.Core;
using Catalog.Domain.Core.Common;
using Catalog.Domain.Core.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Services.CategoryCQRS.Commands.CreateCategory
{
    public class CreateCategoryRequest : IRequest<Guid>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public List<Guid>? Features { get; set; } = null;
    }

    public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, Guid>
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IFileStorageService _fileService;

        public CreateCategoryRequestHandler(IGenericRepository<Category> categoryRepository, IFileStorageService fileService) =>
            (_categoryRepository, _fileService) = (categoryRepository, fileService);

        public async Task<Guid> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var thumbnailSaveResult = await _fileService.UploadAsync<Category>(request.Thumbnail, FileType.Image, cancellationToken);

            var category = Category.CreateNew(request.Name, request.IsActive, request.Description, request.Features,
                thumbnailSaveResult?.FilePath, thumbnailSaveResult?.FileName, thumbnailSaveResult?.Extension, thumbnailSaveResult?.Size);

            await _categoryRepository.AddAsync(category, cancellationToken);

            return category.Id.Value;
        }
    }
}
