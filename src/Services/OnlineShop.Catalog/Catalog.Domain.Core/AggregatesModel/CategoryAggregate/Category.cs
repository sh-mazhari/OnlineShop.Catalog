using Catalog.Domain.Core.AggregatesModel.CategoryAggregate;
using Catalog.Domain.Core.AggregatesModel.FeatureAggregate;
using Catalog.Domain.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Core
{
    public class Category : AggregateRoot<CategoryId>
    {
        public string CategoryName { get; private set; }
        public bool IsActive { get; private set; }
        public string Description { get; private set; }
        public CategoryThumbnail Thumbnail { get; private set; }

        private readonly List<CategoryFeature> _categoryFeatures = new List<CategoryFeature>();
        public IReadOnlyList<CategoryFeature> CategoryFeatures => _categoryFeatures;


        public static Category CreateNew(string categoryName, bool isActive, string desscription, List<Guid> features,
            string? thumbnailPath, string? thumbnailName, string? thumbnailExtension, int? thumbnailSize)
        {
            return new Category(categoryName, isActive, desscription, features, thumbnailPath, thumbnailName, thumbnailExtension, thumbnailSize);
        }

        private void BuildFeatures(List<Guid> featureData)
        {
            featureData.ForEach(featureId =>
            {
                var newFeature = CategoryFeature.CreateNew(Id, new FeatureId(featureId));
                _categoryFeatures.Add(newFeature);
            });
        }

        private void BuildThumbnail(string? filePath, string? fileName, string? fileExtension, int? fileSize)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            Thumbnail.FilePath = filePath;
            Thumbnail.FileName = fileName;
            Thumbnail.Extension = fileExtension;
            Thumbnail.Size = fileSize.Value;
        }

        private Category(string categoryName, bool isActive, string desscription, List<Guid> features,
            string? thumbnailPath, string? thumbnailName, string? thumbnailExtension, int? thumbnailSize)
        {
            //validation....
            CategoryName = categoryName;
            IsActive = isActive;
            Description = desscription;
            BuildThumbnail(thumbnailPath, thumbnailName, thumbnailExtension, thumbnailSize);
            BuildFeatures(features);
        }

        private Category()
        {
        }
    }
}
