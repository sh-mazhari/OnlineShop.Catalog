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
       
        
        internal static Category CreateNew(string categoryName, bool isActive, string desscription, List<FeatureId> features)
        {
            return new Category(categoryName, isActive, desscription, features);
        }

        private void BuildFeatures(List<FeatureId> featureData)
        {
            featureData.ForEach(featureId =>
            {
                var newFeature = CategoryFeature.CreateNew(Id, featureId);
                _categoryFeatures.Add(newFeature);
            });
        }

        private Category(string categoryName, bool isActive, string desscription, List<FeatureId> features)
        {
            //validation....
            CategoryName = categoryName;
            IsActive = isActive;
            Description = desscription;
            BuildFeatures(features);
        }

        private Category()
        {
        }
    }
}
