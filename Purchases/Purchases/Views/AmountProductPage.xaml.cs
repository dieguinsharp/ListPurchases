using Purchases.Models;
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
    public partial class AmountProductPage : ContentPage
    {

        public Command SaveCommand { get;set; }

        Product product;
        public Product Product
        {
            get { return product; }
            set
            {
                product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        public AmountProductPage(Product product)
        {
            InitializeComponent();

            SaveCommand = new Command(SaveAmountProduct);

            BindingContext = this;
            this.Product = product;

            Product.Amount = 1;
        }

        public void SaveAmountProduct()
        {
            if(Product.Amount > 0)
            {
                App.Current.MainPage.Navigation.PopModalAsync();
            }
            else
            {
                App.Toast("Informe um valor maior que 0!");
            }
        }
    }
}