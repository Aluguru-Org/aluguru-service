using Mubbi.Marketplace.Domain;
using PampaDevs.Utils;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Catalog.Domain
{
    public class Dimensions : ValueObject
    {
        public Dimensions(decimal height, decimal width, decimal depth)
        {
            Height = height;
            Width = width;
            Depth = depth;

            ValidateValueObject();
        }

        public decimal Height { get; private set; }
        public decimal Width { get; private set; }
        public decimal Depth { get; private set; }
        protected override void ValidateValueObject()
        {
            Ensure.That(Height > 0, "The field Height cannot be smaller than 0");
            Ensure.That(Width > 0, "The field Width cannot be smaller than 0");
            Ensure.That(Depth > 0, "The field Depth cannot be smaller than 0");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Height;
            yield return Width;
            yield return Depth;
        }
    }
}
