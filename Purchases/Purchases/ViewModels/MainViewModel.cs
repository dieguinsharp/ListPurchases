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
    public class MainViewModel : BaseViewModel{

        ObservableCollection<Purchase> _purchases;
        public ObservableCollection<Purchase> Purchases { 
            get => _purchases; 
            set => SetProperty(ref _purchases, value); 
        }

        Purchase _purchaseSelected;
        public Purchase PurchaseSelected { 
            get => _purchaseSelected; 
            set => SetProperty(ref _purchaseSelected, value); 
        }  

        bool _existPurchases;
        public bool ExistPurchases { 
            get => _existPurchases; 
            set => SetProperty(ref _existPurchases, value); 
        }
        
        public Command AddPurchaseCommand { get; set; }
        public Command DeletePurchaseCommand { get; set; }
        public Command SelectPurchaseCommand { get; set; }
        public Command ViewProductsCommand { get; set; }

        public MainViewModel () {

            Title = "Pagina Inicial";
            AddPurchaseCommand = new Command(ClickAddPurchase);
            DeletePurchaseCommand = new Command<int>(ClickDeletePurchase);
            SelectPurchaseCommand = new Command(ClickPurchase);
            ViewProductsCommand = new Command(ClickViewProducts);

        }

        public async void ClickPurchase () {
            try {

                SimpleTaskVisible = true;

                var purchaseSelected = await Task.Run(() => {

                    var result = controllerApp.GetPurchaseAsync(PurchaseSelected.Id);
                    return result;
                
                });

                await App.Current.MainPage.Navigation.PushAsync(new EditProductsPage(purchaseSelected));


            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                SimpleTaskVisible = false;
            }
        }
        public async void ClickDeletePurchase (int idPurchase) {
            try {

                SimpleTaskVisible = true;

                await Task.Run(() => {

                    var resultado = controllerApp.DeletePurchaseAsync(idPurchase);
                    this.Appearing();
                
                });

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                SimpleTaskVisible = false;
            }
        }
        public async void Appearing () {
            try {

                IsBusy = true;

                var purchases = await Task.Run(() => {

                    return controllerApp.GetPurchasesAsync();
                
                });

                if(purchases != null && purchases.Any()) {
                    Purchases = new ObservableCollection<Purchase>(purchases);
                    ExistPurchases = true;
                } else {
                    ExistPurchases = false;
                }

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            } finally {
                IsBusy = false;
            }
        }
        
        public void ClickAddPurchase () {
            try {

                App.Current.MainPage.Navigation.PushAsync(new ProductsPage(true));

            } catch(Exception ex) {
                App.Toast("Ouve um erro: " + ex.Message);
            }
        }


        public void ClickViewProducts()
        {
            try
            {

                App.Current.MainPage.Navigation.PushAsync(new ProductsPage());

            }
            catch (Exception ex)
            {
                App.Toast("Ouve um erro: " + ex.Message);
            }
        }

    }
}
