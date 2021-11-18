using Purchases.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Purchases.ViewModels {
    class EditProductsViewModel : BaseViewModel{

        ObservableCollection<PurchaseProduct> _products;
        public ObservableCollection<PurchaseProduct> Products { 
            get => _products; 
            set => SetProperty(ref _products, value); 
        }

        bool _existProducts;
        public bool ExistProducts { 
            get => _existProducts; 
            set => SetProperty(ref _existProducts, value); 
        }

        bool _loadSimpleTask;
        public bool LoadSimpleTask { 
            get => _loadSimpleTask; 
            set => SetProperty(ref _loadSimpleTask, value); 
        }

        string _name;
        public string Name {
            get => _name; 
            set => SetProperty(ref _name, value); 
        }

        public Command DeleteProductCommand { get; set; }

        int idPurchase;
        public EditProductsViewModel (Purchase purchase) {
            Title = purchase.Name;

            idPurchase = purchase.Id;
            DeleteProductCommand = new Command<int>(ClickDeleteProduto);
        }

        public async void Appearing () {
            try {

                IsBusy = true;

                await Task.Run(() => {

                    GetItens().Wait();
                
                });

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                IsBusy = false;
            }
        }
        public async void ClickDeleteProduto(int idProduct) {
            try {

                LoadSimpleTask = true;

                await Task.Run(() => {

                    var product = controllerApp.DeleteProductAsync(idProduct);
                    this.Appearing();
                
                });

                App.Toast("Produto removido.");

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                LoadSimpleTask = false;
            }
        }
        public async Task GetItens () {
            try {

                var products = await controllerApp.GetProductsPurchaseAsync(idPurchase);

                if(products == null || products.Count() == 0) {
                    ExistProducts = false;
                } else {
                    Products = new ObservableCollection<PurchaseProduct>(products);
                    ExistProducts = true;
                } 

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                IsBusy = false;
            }
        }
        
    }
}
