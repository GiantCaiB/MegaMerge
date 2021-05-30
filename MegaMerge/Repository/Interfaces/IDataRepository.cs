using System;
using System.Collections.Generic;
using System.Text;
using MegaMerge.Dtos;

namespace MegaMerge.Repository.Interfaces
{
    public interface IDataRepository
    {
        List<Catalog> GetAllCatalogs();
        List<Catalog> InsertCatalogs(List<Catalog> catalogs);
        List<SupplierProductBarcode> GetAllBarcodes();
        List<SupplierProductBarcode> InsertBarcodes(List<SupplierProductBarcode> barcodes);
        List<Supplier> GetAllSuppliers();
        List<Supplier> InsertSuppliers(List<Supplier> suppliers);
        List<MergedProduct> GetAllMergedProducts();
        List<MergedProduct> InsertMergedProducts(List<MergedProduct> mergedProducts);
    }
}
