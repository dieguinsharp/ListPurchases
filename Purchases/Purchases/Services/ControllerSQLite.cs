using Purchases.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Purchases.Services {
    public class ControllerSQLite {

        //IDataStore<Product> productDataStore => DependencyService.Get<IDataStore<Product>>();
        //IDataStorePurchase<Purchase> purchaseDataStore => DependencyService.Get<IDataStorePurchase<Purchase>>();

        readonly string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private SQLiteConnection connection;

        public async Task<bool> CreateDataBase (bool forced = false) {

            connection = new SQLiteConnection(System.IO.Path.Combine(pasta,"PurchasesList.db"));

            var result1 = connection.GetTableInfo("Purchase");
            if ((result1 == null) || (result1.Count == 0) || forced)
            {
                connection.DropTable<Purchase>();
                connection.CreateTable<Purchase>();
            }

            var result2 = connection.GetTableInfo("Product");
            if ((result2 == null) || (result2.Count == 0) || forced)
            {
                connection.DropTable<Product>();
                connection.CreateTable<Product>();
            }

            var result3 = connection.GetTableInfo("PurchaseProduct");
            if ((result3 == null) || (result3.Count == 0) || forced)
            {
                connection.DropTable<PurchaseProduct>();
                connection.CreateTable<PurchaseProduct>();
            }

            return await Task.FromResult(true);
        }

        #region sqlite product
        public async Task<bool> AddProductAsync (Product item) {

            connection.Insert(item);
            return await Task.FromResult(true);

        }


        public async Task<bool> UpdateProductAsync (Product product) {

            connection.Update(product);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteProductAsync (int id) {

            var oldItem = await this.GetProductAsync(id);
            connection.Delete(oldItem);
            return await Task.FromResult(true);

        }

        public async Task<IEnumerable<Product>> GetProductsAsync () {

            return await Task.FromResult(connection.Table<Product>());

        }

        public async Task<Product> GetProductAsync (int id) {

            return await Task.FromResult(connection.Table<Product>().Where(p => p.Id == id).FirstOrDefault());

        }

        #endregion

        #region sqlite purchase products

        public async Task<int> AddPurchaseAsync (Purchase purchase) {

            connection.Insert(purchase);

            return await Task.FromResult(purchase.Id);

        }

        public async Task<bool> AddProductsPurchaseAsync (IEnumerable<PurchaseProduct> productsPurchase) {

            await Task.FromResult(connection.InsertAll(productsPurchase));

            return await Task.FromResult(true);

        }

        public async Task<IEnumerable<Purchase>> GetPurchasesAsync () {

            return await Task.FromResult(connection.Table<Purchase>());

        }

        public async Task<Purchase> GetPurchaseAsync (int idPurchase) {

            return await Task.FromResult(connection.Table<Purchase>().FirstOrDefault(p => p.Id == idPurchase));

        }
        
        public async Task<bool> DeletePurchaseAsync (int idPurchase) {

            var purchase = await GetPurchaseAsync(idPurchase);
            connection.Delete(purchase);

            return await Task.FromResult(true);

        }

        #endregion

        #region sqlite products purchase

        public async Task<IEnumerable<PurchaseProduct>> GetProductsPurchaseAsync (int idPurchase) {

            var purchases = (from p in connection.Table<Product>()
                            join pp in connection.Table<PurchaseProduct>()
                            on p.Id equals pp.IdProduct
                            where pp.IdPurchase == idPurchase
                            select new PurchaseProduct() {
                                Id = pp.Id,
                                IdProduct = pp.IdProduct,
                                IdPurchase = pp.IdPurchase,
                                ProductPurchased = pp.ProductPurchased,
                                Name = p.Name
                            }).ToList();

            for(int x = 0; x < purchases.Count;x++) {
                purchases[x].ProductSelected += ChangeDataBaseProductSelected;
            }

            return await Task.FromResult(purchases);

        }

        public async void ChangeDataBaseProductSelected(object s, EventArgs args) {
            try {

                var purchase = s as PurchaseProduct;
                await this.UpdatePurchaseProduct(purchase);

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            }
        }

        public async Task<bool> UpdatePurchaseProduct (PurchaseProduct purchaseProduct) {

            connection.Update(purchaseProduct);

            return await Task.FromResult(true);
        }

        #endregion
    }
}
