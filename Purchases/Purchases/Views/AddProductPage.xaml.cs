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
    public partial class AddProductPage:ContentPage {

        AddProductViewModel addProduct;
        EditProductViewModel editProduct;
        public AddProductPage (bool Edit, Product product = null) {
            InitializeComponent();

            if(Edit) {
                BindingContext = editProduct = new EditProductViewModel(product);
            } else {
                BindingContext = addProduct = new AddProductViewModel();
            }
            
        }
    }
}