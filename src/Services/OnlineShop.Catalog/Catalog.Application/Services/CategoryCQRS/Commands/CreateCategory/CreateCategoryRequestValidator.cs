using Catalog.Application.Common.Validation;

namespace Catalog.Application.Services.CategoryCQRS.Commands.CreateCategory;

public class CreateCategoryRequestValidator : CustomValidator<CreateCategoryRequest>
{
    //public CreateCategoryRequestValidator(IReadRepository<Category> categoryRepo, IReadRepository<Feature> featureRepo,  IStringLocalizer<CreateCategoryRequestValidator> T)
    //{
    //    RuleFor(c => c.Name)
    //        .NotEmpty()
    //        .MaximumLength(2)
    //        .MustAsync(async (name, ct) => await categoryRepo.GetBySpecAsync(new CategoryByNameSpec(name), ct) is null)
    //            .WithMessage((_, name) => T["Category {0} already Exists.", name]);


    //    RuleForEach(c => c.Features)
    //        .NotEmpty().MustAsync(async (id, ct) => await featureRepo.GetByIdAsync(id, ct) is not null)
    //        .WithMessage((_, id) => T["Feature {0} Not Found.", id]);
    //}
}