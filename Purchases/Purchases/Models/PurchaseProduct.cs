using Purchases.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Purchases.Models {

    [Table("PurchaseProduct")]
    public class PurchaseProduct{

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdPurchase { get; set; }
        public int IdProduct { get; set; }
        public int Amount { get; set; }

        bool productPurchasedelected;
        public bool ProductPurchased { 
            get {

                return productPurchasedelected;
                
            } set {

                productPurchasedelected = value;
                ProductSelected?.Invoke(this, new EventArgs());
                
            } 
        }

        [Ignore]
        public string Name { get; set; }

        public EventHandler ProductSelected;

        public bool Validade()
        {
            return Amount > 0;
        }

    }
}
