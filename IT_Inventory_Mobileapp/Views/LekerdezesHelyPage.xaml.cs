using IT_Inventory_Mobileapp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IT_Inventory_Mobileapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LekerdezesHelyPage : ContentPage
    {

        private HttpClient client;
        string helyek_url;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hely_url"></param>
        public LekerdezesHelyPage(string hely_url)
        {
            InitializeComponent();

            helyek_url = hely_url;

            GetHely();
        }


        private async void GetHely()
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
   (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);
            var response = await client.GetStringAsync(helyek_url);

            var items = JsonConvert.DeserializeObject<List<Item>>(response);

            HelyListView.ItemsSource = items;

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