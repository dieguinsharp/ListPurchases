using Purchases.Services;
using Purchases.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Purchases.Models {

    [Table("Product")]
    public class Product {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }


        bool selected;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                if(value) SetAmount?.Invoke(this, new EventArgs());
            }
        }

        public EventHandler SetAmount;

        [Ignore]
        public int Amount { get; set; }
    }
}
