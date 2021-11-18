using Purchases.Models;
using Purchases.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Purchases.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProductsPage:ContentPage {

        EditProductsViewModel editProducts;
        public EditProductsPage (Purchase purchase) {
            InitializeComponent();
            BindingContext = editProducts = new EditProductsViewModel(purchase);
        }

        protected override void OnAppearing () {
            editProducts.Appearing();
        }
    }
}