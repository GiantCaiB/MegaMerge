using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MegaMerge.Dtos;
using FileType = MegaMerge.EnumsAndConstants.FileType;

namespace MegaMerge.Extensions
{
    public static class Extensions
    {
        public static Type GetDtoType(this FileType fileType)
        {
            return fileType switch
            {
                FileType.Catalog => typeof(Catalog),
                FileType.Barcode => typeof(SupplierProductBarcode),
                FileType.Supplier => typeof(Supplier),
                _ => throw new NotSupportedException()
            };
        }

        public static bool IsSameProduct(this MergedProduct source, MergedProduct target)
        {
            var sourceBarcodes = source.Barcodes.Select(x => x.Barcode);
            var targetBarcodes = target.Barcodes.Select(x => x.Barcode);
            return sourceBarcodes.Intersect(targetBarcodes).Any();
        }

        public static IEnumerable<MergedProduct> RemoveDuplicates(this ICollection<MergedProduct> products)
        {
            foreach (var mergedProduct in products)
            {
                // Skip duplicated product
                if (!mergedProduct.IsDuplicated)
                {
                    var duplicateProducts =
                        products.Where(x =>
                            x != mergedProduct && !x.IsDuplicated && mergedProduct.IsSameProduct(x)).ToList();
                    // Mark other duplicates
                    duplicateProducts.ForEach(x => x.IsDuplicated = true);
                }
            }

            return products.Where(x => !x.IsDuplicated);

        }
    }
}
