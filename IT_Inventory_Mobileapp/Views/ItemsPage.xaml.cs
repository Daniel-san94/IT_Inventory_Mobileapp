using IT_Inventory_Mobileapp.Models;
using IT_Inventory_Mobileapp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IT_Inventory_Mobileapp.Views
{
    public partial class ItemsPage : ContentPage
    {

        /// <summary>
        /// Az ItemsPage függvény inicializálja a komponenseket.
        /// </summary>
        public ItemsPage()
        {
            InitializeComponent();
        }

        HttpClient client;


        /// <summary>
        /// Az OnApperaing metódus elvileg mindig lefut ha megnyissuk az ItemPage-t.
        /// Példányosítunk egy HttpClientHandlert, majd elhárítja a validációs hibákat. A HttpClientHandler azért kellett mert ssl hibát kaptam.
        /// A response változóba async meghívom Get kéréssel az apit. Az items változóba Deserializálom a responset egy Item tipusú listába.
        /// Majd a LeltarListView ItemSourcenak megadom az items változót vagyis a deserializált listát.
        /// </summary>
        protected override async void OnAppearing()
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
   (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);
            var response = await client.GetStringAsync("https://192.168.1.176:44391/api/TE_Leltar");

            var items = JsonConvert.DeserializeObject<List<Item>>(response);

            LeltarListView.ItemsSource = items;
            base.OnAppearing();
        }

        /// <summary>
        /// A mydetail váltoba betöltöm annak az itemnek az adatait, amit kiválasztottunk.
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
        /// Ha rányomunk a toolbaron a hozzáadás gombra akkor átnavigál a NewItemPage-re.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private async void tbiAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewItemPage());
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

    }
}