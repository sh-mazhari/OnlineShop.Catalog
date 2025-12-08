using Catalog.Application.Common.Validation;
using Catalog.Domain.Core;
using Catalog.Domain.Core.AggregatesModel.FeatureAggregate;
using Catalog.Domain.Core.SeedWork;
using FluentValidation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Catalog.Application.Services.CategoryCQRS.Commands.CreateCategory;

public class CreateCategoryRequestValidator : CustomValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator(IReadRepository<Category> categoryRepo, IReadRepository<Feature> featureRepo)
    {
        //RuleFor(c => c.Name)
        //    .NotEmpty()
        //    .MaximumLength(20)
        //    .MustAsync(async (name, ct) => await categoryRepo.GetBySpecAsync(new CategoryByNameSpec(name)) is null)
        //        .WithMessage((_, name) => $"Category {name} already Exists.");



        RuleFor(c => c.Name)
           .NotEmpty()
           .MaximumLength(20)
           //.MustAsync(async (name, ct) =>
           //    await categoryRepo.FirstOrDefaultAsync(new CategoryByNameSpec(name), ct) is null)
           .MustAsync(async (name, ct) => !await categoryRepo.AnyAsync(new CategoryByNameSpec(name), ct))
           .WithMessage((_, name) => $"Category {name} already exists.");


        RuleForEach(c => c.Features)
            .NotEmpty().MustAsync(async (id, ct) => await featureRepo.GetByIdAsync(new FeatureId(id), ct) is not null)
            .WithMessage((_, id) => $"Feature {id} Not Found.");
    }
}