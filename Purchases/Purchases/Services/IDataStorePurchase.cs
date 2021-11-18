using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Purchases.Services {
    public interface IDataStorePurchase<T> {

        Task<bool> CreateDataBase (bool forced = false);
        Task<int> AddItemAsync (T item);
        Task<bool> UpdateItemAsync (T item);
        Task<bool> DeleteItemAsync (int id);
        Task<T> GetItemAsync (int id);
        Task<IEnumerable<T>> GetItemsAsync (bool forceRefresh = false);

    }
}
