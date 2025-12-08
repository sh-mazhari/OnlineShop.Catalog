using Catalog.Application.Common.FileStorage;
using Catalog.Domain.Core;
using Catalog.Domain.Core.Common;
using Catalog.Domain.Core.SeedWork;
using FluentValidation;
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
        private readonly IRepository<Category> _categoryRepository;
        private readonly IFileStorageService _fileService;
        private IValidator<CreateCategoryRequest> _validator;

        public CreateCategoryRequestHandler(IRepository<Category> categoryRepository,
            IFileStorageService fileService,
            IValidator<CreateCategoryRequest> validator) =>
            (_categoryRepository, _fileService, _validator) = (categoryRepository, fileService, validator);

        public async Task<Guid> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new Exception(String.Join(",", validationResult.Errors.Select(q => q.ErrorMessage).ToArray()));
            }

            var thumbnailSaveResult = await _fileService.UploadAsync<Category>(request.Thumbnail, FileType.Image, cancellationToken);

            var category = Category.CreateNew(request.Name, request.IsActive, request.Description, request.Features,
                thumbnailSaveResult?.FilePath, thumbnailSaveResult?.FileName, thumbnailSaveResult?.Extension, thumbnailSaveResult?.Size);

            await _categoryRepository.AddAsync(category, cancellationToken);

            return category.Id.Value;
        }
    }
}
