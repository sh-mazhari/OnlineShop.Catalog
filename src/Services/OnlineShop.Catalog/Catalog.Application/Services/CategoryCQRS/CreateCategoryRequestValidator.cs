using Catalog.Application.Common.Validation;
using Catalog.Domain.Core;
using Catalog.Domain.Core.AggregatesModel.FeatureAggregate;
using Catalog.Domain.Core.SeedWork;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Catalog.Application.Services.CategoryCQRS;

public class CreateCategoryRequestValidator : CustomValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator(IReadRepository<Category> categoryRepo, IReadRepository<Feature> featureRepo,  IStringLocalizer<CreateCategoryRequestValidator> T)
    {
        //it's deprecate in new version
        //RuleFor(c => c.Name)
        //    .NotEmpty()
        //    .MaximumLength(2)
        //    .MustAsync(async (name, ct) => await categoryRepo.GetBySpecAsync(new CategoryByNameSpec(name), ct) is null)
        //        .WithMessage((_, name) => T["Category {0} already Exists.", name]);
        
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(2) // <-- suspicious: did you mean something larger?
            .MustAsync(async (name, ct) => await categoryRepo.FirstOrDefaultAsync(new CategoryByNameSpec(name), ct) is null)
            .WithMessage((_, name) => T["Category {0} already Exists.", name]);

        RuleForEach(c => c.Features)
            .NotEmpty().MustAsync(async (id, ct) => await featureRepo.GetByIdAsync(id, ct) is not null)
            .WithMessage((_, id) => T["Feature {0} Not Found.", id]);
    }
}