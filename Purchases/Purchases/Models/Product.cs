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
        public bool Selected { get; set; }

    }
}
