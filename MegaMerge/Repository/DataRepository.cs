using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MegaMerge.Dtos;
using MegaMerge.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace MegaMerge.Repository
{
    public class DataRepository : IDataRepository
    {
        private readonly ILogger<DataRepository> _logger;
        private List<Catalog> Catalogs { get; set; }
        private List<SupplierProductBarcode> Barcodes { get; set; }
        private List<Supplier> Suppliers { get; set; }
        private List<MergedProduct> MergedProducts { get; set; }
        public DataRepository(ILogger<DataRepository> logger)
        {
            _logger = logger;
            Catalogs = new List<Catalog>();
            Barcodes = new List<SupplierProductBarcode>();
            Suppliers = new List<Supplier>();
            MergedProducts = new List<MergedProduct>();
        }

        public List<Catalog> GetAllCatalogs()
        {
            return Catalogs;
        }

        public List<SupplierProductBarcode> GetAllBarcodes()
        {
            return Barcodes;
        }

        public List<Supplier> GetAllSuppliers()
        {
            return Suppliers;
        }
        public List<MergedProduct> GetAllMergedProducts()
        {
            return MergedProducts;
        }

        public List<Catalog> InsertCatalogs(List<Catalog> catalogs)
        {
            try
            {
                Catalogs.AddRange(catalogs);
                return catalogs;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<Catalog>();
            }
        }

        public List<SupplierProductBarcode> InsertBarcodes(List<SupplierProductBarcode> barcodes)
        {
            try
            {
                Barcodes.AddRange(barcodes);
                return barcodes;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<SupplierProductBarcode>();
            }
        }

        public List<Supplier> InsertSuppliers(List<Supplier> suppliers)
        {
            try
            {
                Suppliers.AddRange(suppliers);
                return suppliers;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<Supplier>();
            }
        }

        public List<MergedProduct> InsertMergedProducts(List<MergedProduct> mergedProducts)
        {
            try
            {
                MergedProducts.AddRange(mergedProducts);
                return mergedProducts;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<MergedProduct>();
            }
        }
    }
}
