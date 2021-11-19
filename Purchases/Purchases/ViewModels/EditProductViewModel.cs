using Purchases.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Purchases.ViewModels {
    class EditProductViewModel : BaseViewModel {

        public Command SaveProductCommand { get; set; }

        Product product;
        public Product Product {
            get { return product; }
            set { SetProperty(ref product, value); }
        }

        public EditProductViewModel (Product produtc) {
            Title = "Editar Produto";

            this.product = produtc;

            SaveProductCommand = new Command(ClickEditProduct);
        }

        public async void ClickEditProduct () {
            try {

                if(string.IsNullOrEmpty(Product.Name)) {

                    App.Toast("Informe um nome valido!");

                } else {

                    IsBusy = true;

                    await Task.Run(() => {

                        EditItem(Product).Wait();
                        
                    });

                    App.Toast("Produto editado.");
                    await App.Current.MainPage.Navigation.PopModalAsync();

                }
                

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                IsBusy = false;
            }
        }

        public async Task EditItem (Product p) {
            try {

                await controllerApp.UpdateProductAsync(p);

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            }
        }

    }
}
