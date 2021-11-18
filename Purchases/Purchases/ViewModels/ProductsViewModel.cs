using Purchases.Models;
using Purchases.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Purchases.ViewModels {

    public class ProductsViewModel : BaseViewModel {

        ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products { 
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

        public Command AddProductCommand { get; set; }
        public Command CreateListCommand { get; set; }
        public Command EditProductCommand { get; set; }
        public Command DeleteProductCommand { get; set; }

        public ProductsViewModel () {
            Title = "Produtos";
            AddProductCommand = new Command(ClickAddProduct);
            CreateListCommand = new Command(ClickCreateList);
            EditProductCommand = new Command<int>(ClickEditProduto);
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
        public async void ClickCreateList() {
            try {

                var products = Products.Where(p => p.Selected);

                if(products.Count() == 0) {
                    App.Toast("Favor selecionar pelo menos 1 item da lista de produtos.");
                } else {

                    if(string.IsNullOrEmpty(Name)) {

                        App.Toast("Informe um nome válido.");

                    } else {

                        IsBusy = true;

                        var idPurchase = await Task.Run(() => { 
                        
                            Purchase purchase = new Purchase() {
                                Date = DateTime.Now,
                                IsFinished = false, 
                                Name = Name.ToUpper()
                            };

                            var resultado = SetPurchase(purchase);
                            return resultado;

                        });

                        if(idPurchase != 0) {

                            var isOk = await Task.Run(() => { 
                        
                                var resultado = SetListProduct(idPurchase, products);
                                return resultado;

                            });

                            if(isOk) {
                                App.Toast("Lista criada.");
                                await App.Current.MainPage.Navigation.PopAsync();
                            } else {
                                App.Toast("Ouve um erro, tente novamente.");
                            }

                        } else {
                            App.Toast("Ouve um erro, tente novamente.");
                        }
                    }
                } 

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                IsBusy = false;
            }
        }
        public async void ClickEditProduto(int idProduct) {
            try {

                LoadSimpleTask = true;

                var result = await Task.Run(() => {

                    var product = controllerApp.GetProductAsync(idProduct);
                    return product;
                
                });

                await App.Current.MainPage.Navigation.PushModalAsync(new AddProductPage(true, result));

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                LoadSimpleTask = false;
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
        public async Task<int> SetPurchase(Purchase purchase) {
            try {

                var resultado = await controllerApp.AddPurchaseAsync(purchase);
                return resultado;

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
                return 0; 
            }
        }
        public async Task<bool> SetListProduct(int idPurchase, IEnumerable<Product> products) {
            try {

                List<PurchaseProduct> listPurchaseProduct = new List<PurchaseProduct>();

                var idProducts = products.Select(p => p.Id);
                foreach(var idProduct in idProducts) {

                    listPurchaseProduct.Add(new PurchaseProduct() { IdProduct = idProduct, IdPurchase = idPurchase, ProductPurchased = false});

                }

                return await controllerApp.AddProductsPurchaseAsync(listPurchaseProduct);

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
                return false;
            }
        }
        public async Task GetItens () {
            try {

                var products = await controllerApp.GetProductsAsync();

                if(products.Count() == 0) {
                    ExistProducts = false;
                } else {
                    Products = new ObservableCollection<Product>(products);
                    ExistProducts = true;
                } 

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                IsBusy = false;
            }
        }
        public void ClickAddProduct () {
            try {

                App.Current.MainPage.Navigation.PushModalAsync(new AddProductPage(false));

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                IsBusy = false;
            }
        }

    }
}
