using Purchases.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Purchases.Services {
    class PurchaseDataStore : IDataStorePurchase<Purchase>{

        readonly string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private SQLiteConnection connection;

        public async Task<bool> CreateDataBase (bool forced = false) {

            connection = new SQLiteConnection(System.IO.Path.Combine(pasta,"PurchasesList.db"));

            var result = connection.GetTableInfo("Purchase");
            if ((result == null) || (result.Count == 0) || forced)
            {
                connection.DropTable<Purchase>();
                connection.CreateTable<Purchase>();
            }

            return await Task.FromResult(true);
        }

        public async Task<int> AddItemAsync (Purchase item) {

            connection.Insert(item);
            return await Task.FromResult(item.Id);

        }

        public async Task<bool> UpdateItemAsync (Purchase item) {

            var purchase = await this.GetItemAsync(item.Id);
            purchase.Id = item.Id;

            connection.Update(purchase);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync (int id) {
            var oldItem = await this.GetItemAsync(id);
            connection.Delete(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Purchase> GetItemAsync (int id) {

            return await Task.FromResult(connection.Table<Purchase>().Where(p => p.Id == id).FirstOrDefault());

        }

        public async Task<IEnumerable<Purchase>> GetItemsAsync (bool forceRefresh = false) {

            return await Task.FromResult(connection.Table<Purchase>().ToList());
        }
    }
}
