using Purchases.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchases.Services {
    public class ProductDataStore:IDataStore<Product> {

        readonly string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private SQLiteConnection connection;

        public async Task<bool> CreateDataBase (bool forced = false) {

            connection = new SQLiteConnection(System.IO.Path.Combine(pasta,"PurchasesList.db"));

            var result = connection.GetTableInfo("Product");
            if ((result == null) || (result.Count == 0) || forced)
            {
                connection.DropTable<Product>();
                connection.CreateTable<Product>();
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> AddItemAsync (Product item) {

            connection.Insert(item);
            return await Task.FromResult(true);

        }

        public async Task<bool> UpdateItemAsync (Product item) {

            var product = await this.GetItemAsync(item.Id);
            product.Name = item.Name;

            connection.Update(product);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync (int id) {
            var oldItem = await this.GetItemAsync(id);
            connection.Delete(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Product> GetItemAsync (int id) {

            return await Task.FromResult(connection.Table<Product>().Where(p => p.Id == id).FirstOrDefault());

        }

        public async Task<IEnumerable<Product>> GetItemsAsync (bool forceRefresh = false) {

            return await Task.FromResult(connection.Table<Product>().ToList());
        }
    }
}
