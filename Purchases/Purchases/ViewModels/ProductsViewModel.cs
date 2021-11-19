using Purchases.Models;
using Purchases.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Linq;
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

        bool _createList;
        public bool CreateList
        {
            get => _createList;
            set => SetProperty(ref _createList, value);
        }

        bool _viewProduct;
        public bool ViewProduct
        {
            get => _viewProduct;
            set => SetProperty(ref _viewProduct, value);
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

        public ProductsViewModel (bool createList) {
            Title = "Produtos";
            AddProductCommand = new Command(ClickAddProduct);
            CreateListCommand = new Command(ClickCreateList);
            EditProductCommand = new Command<Product>(ClickEditProduto);
            DeleteProductCommand = new Command<Product>(ClickDeleteProduto);

            CreateList = createList;
            ViewProduct = !createList;
        }

        bool isLoaded = false;
        public async void Appearing () {
            try {

                if (!isLoaded)
                {

                    IsBusy = true;

                    var products = await Task.Run(() => {

                        return controllerApp.GetProductsAsync();

                    });

                    if (products.Count == 0)
                    {
                        ExistProducts = false;
                    }
                    else
                    {

                        for (int x = 0; x < products.Count; x++)
                        {
                            products[x].SetAmount += OpenAmountProductPage;
                        }

                        Products = new ObservableCollection<Product>(products);
                        ExistProducts = true;
                    }

                    isLoaded = true;
                }


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
        public async void ClickEditProduto(Product product) {
            try {

                LoadSimpleTask = true;

                var indexProductOfList = Products.IndexOf(product);

                await App.Current.MainPage.Navigation.PushModalAsync(new AddProductPage(true, Products[indexProductOfList]));
                isLoaded = false;

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                LoadSimpleTask = false;
            }
        }
        public async void ClickDeleteProduto(Product product) {
            try {

                LoadSimpleTask = true;

                await Task.Run(() => {

                    var sucess = controllerApp.DeleteProductAsync(product);
                    isLoaded = false;
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

                
                foreach(var product in products) {

                    listPurchaseProduct.Add(new PurchaseProduct() { IdProduct = product.Id, IdPurchase = idPurchase, ProductPurchased = false, Amount = product.Amount});

                }

                return await controllerApp.AddProductsPurchaseAsync(listPurchaseProduct);

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
                return false;
            }
        }
        
        public void ClickAddProduct () {
            try {

                App.Current.MainPage.Navigation.PushModalAsync(new AddProductPage(false));
                isLoaded = false;

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                IsBusy = false;
            }
        }


        private AmountProductPage amountProductPage;
        public void OpenAmountProductPage(object s, EventArgs args)
        {
            var product = s as Product;

            amountProductPage = new AmountProductPage(product);

            App.Current.MainPage.Navigation.PushModalAsync(amountProductPage);
        }

    }
}
