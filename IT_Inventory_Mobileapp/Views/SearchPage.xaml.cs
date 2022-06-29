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
    public partial class SearchPage : ContentPage
    {
        private List<Item> items = new List<Item>();


        /// <summary>
        /// A SearcgPage metódus, inicializálja a komponenseket és meghívja a Kereses függvényt.
        /// </summary>
        public SearchPage()
        {
            InitializeComponent();
            Kereses();

        }

        HttpClient client;

        /// <summary>
        /// A keresés metódus először lekéri az összes itemet a response változóba.
        /// Az items-be pedig deserializálom a responset.
        /// A SearchedListView.ItemsSource érékének megadom a items-t.
        /// </summary>
        private async void Kereses()
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
   (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);
            var response = await client.GetStringAsync("https://192.168.1.176:44391/api/TE_Leltar");


            items = JsonConvert.DeserializeObject<List<Item>>(response);

            SearchedListView.ItemsSource = items;
        }

        /// <summary>
        /// A mydetails váltoba betöltöm annak az itemnek az adatait, amit kiválasztottunk.
        /// Aztán az ItemDetailPage-nek átadom az adatokat paraméterként és átnavigálok rá.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var mydetails = e.Item as Item;
            await Navigation.PushAsync(new ItemDetailPage(mydetails.Nid, mydetails.Nev, mydetails.Hely, mydetails.Felhasznalo, mydetails.Csoport, mydetails.Statusz,
                mydetails.Tipusok, mydetails.Gyarto, mydetails.Modell, mydetails.Sorozatszam, mydetails.LeltariSzam));
        }

        /// <summary>
        /// Ha kiválasztunk egy itemet akkor a cella háttérszíne rózsaszinesre változik.
        /// Erre azért volt szükség mert narancssárga az alap szín.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.LightPink;
            }
        }

        /// <summary>
        /// Akkor aktiválódik ha rányomunk a keresés gombra. A SearchBar text értékét beleteszem a kulcsszo változóba.
        /// SearchedListView.ItemsSource értékének, megadok egy lekérdezést, hogy ha a hely tartalmazza a kulcsszót, akkor azok az elemek listázódnak ki.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBar_Pressed(object sender, EventArgs e)
        {
            var kulcsszo = sbSearch.Text;
            SearchedListView.ItemsSource = items.Where(x => x.Hely.ToLower().Contains(kulcsszo.ToLower()));
        }
    }
}