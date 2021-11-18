using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Models {

    [Table("Purchase")]
    public class Purchase {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool IsFinished { get; set; }

    }
}
