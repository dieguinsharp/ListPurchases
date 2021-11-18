using Purchases.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Purchases.ViewModels {
    class AddProductViewModel : BaseViewModel{

        public Command SaveProductCommand { get; set; }

        string name;
        public string Name {
            get { return name; }
            set { SetProperty(ref name,value); }
        }

        public AddProductViewModel () {
            Title = "Adicionar Produto";
            SaveProductCommand = new Command(ClickAddProduct);
        }

        public async void ClickAddProduct () {
            try {

                if(string.IsNullOrEmpty(Name)) {

                    App.Toast("Informe um nome valido!");

                } else {

                    IsBusy = true;

                    await Task.Run(() => {

                        Product product = new Product() {
                            Name = Name.ToUpper(),
                        };

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
