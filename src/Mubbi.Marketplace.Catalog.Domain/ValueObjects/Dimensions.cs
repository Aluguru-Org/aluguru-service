using Mubbi.Marketplace.Shared.DomainObjects;

namespace Mubbi.Marketplace.Catalog.Domain.ValueObjects
{
    public class Dimensions : ValueObject
    {
        public Dimensions(decimal height, decimal width, decimal depth)
        {
            Height = height;
            Width = width;
            Depth = depth;

            Validate();
        }

        public decimal Height { get; private set; }
        public decimal Width { get; private set; }
        public decimal Depth { get; private set; }
        public override void Validate()
        {
            EntityConcerns.SmallerOrEqualThan(0, Height, "The field Height cannot be smaller than 0");
            EntityConcerns.SmallerOrEqualThan(0, Width, "The field Width cannot be smaller than 0");
            EntityConcerns.SmallerOrEqualThan(0, Depth, "The field Depth cannot be smaller than 0");
        }
    }
}
