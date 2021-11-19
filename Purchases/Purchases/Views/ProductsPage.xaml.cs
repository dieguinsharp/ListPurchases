using Purchases.Models;
using Purchases.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Purchases.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {

        ProductsViewModel products;
        public ProductsPage(bool createList = false)
        {
            InitializeComponent();

            BindingContext = products = new ProductsViewModel(createList);
        }

        protected override void OnAppearing()
        {
            products.Appearing();
        }
    }
}