using Purchases.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Purchases.ViewModels {
    class AddProductViewModel : BaseViewModel{

        public Command SaveProductCommand { get; set; }

        Product product;
        public Product Product
        {
            get { return product; }
            set { SetProperty(ref product, value); }
        }

        public AddProductViewModel () {
            Title = "Adicionar Produto";
            SaveProductCommand = new Command(ClickAddProduct);

            Product = new Product();
        }

        public async void ClickAddProduct () {
            try {

                if(string.IsNullOrEmpty(Product.Name)) {

                    App.Toast("Informe um nome valido!");

                } else {

                    IsBusy = true;

                    await Task.Run(() => {

                        SetItem(product).Wait();
                        
                    });

                    await App.Current.MainPage.Navigation.PopModalAsync();

                }
                

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                IsBusy = false;
            }
        }

        public async Task SetItem (Product p) {
            try {

                await controllerApp.AddProductAsync(p);

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            }
        }
    }
}
