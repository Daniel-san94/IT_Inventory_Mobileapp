using IT_Inventory_Mobileapp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IT_Inventory_Mobileapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LekerdezesLeltariszamPage : ContentPage
    {

        private ObservableCollection<Item> _item = new ObservableCollection<Item>();
        private HttpClient _Client;
        string leltar_url;
        public LekerdezesLeltariszamPage(string final_url)
        {
            InitializeComponent();

            leltar_url = final_url;
            GetLeltariszam();
        }

        private async void GetLeltariszam()
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };
            _Client = new HttpClient(httpClientHandler);
            try
            {
                var content = await _Client.GetStringAsync(leltar_url);
                var item = JsonConvert.DeserializeObject<Item>(content);
                LeltarLeltariszamListView.ItemsSource = _item;
                _item.Add(item);
            }
            catch (Exception)
            {
                await DisplayAlert("Figyelem!", "Hiba! A keresett érték nem található az adatbázisban.", "Ok");
                await Navigation.PushAsync(new LekerdezesPage());
            }
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.LightPink;
            }
        }
        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var mydetails = e.Item as Item;
            await Navigation.PushAsync(new ItemDetailPage(mydetails.Nid, mydetails.Nev, mydetails.Hely, mydetails.Felhasznalo, mydetails.Csoport, mydetails.Statusz,
                mydetails.Tipusok, mydetails.Gyarto, mydetails.Modell, mydetails.Sorozatszam, mydetails.LeltariSzam));
        }


    }
}