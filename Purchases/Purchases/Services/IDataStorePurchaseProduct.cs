using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Purchases.Services {
    public interface IDataStorePurchaseProduct<T> {

        Task<bool> CreateDataBase (bool forced = false);
        Task<bool> AddItemAsync (T item);
        Task<bool> AddItemsAsync (IEnumerable<T> items);
        Task<bool> UpdateItemAsync (T item);
        Task<bool> DeleteItemAsync (int id);
        Task<T> GetItemAsync (int id);
        Task<IEnumerable<T>> GetItemsAsync (int idPurchase);

    }
}
