using IT_Inventory_Mobileapp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IT_Inventory_Mobileapp.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
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

            string ossz = items.Count.ToString();
            lbCount.Text = ossz;

            base.OnAppearing();
        }

        private async void btnNewItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewItemPage());
        }

        private async void btnExit_Clicked(object sender, EventArgs e)
        {
            var dontes = await DisplayAlert("Figyelem!", "Biztosan kilép?", "Igen", "Nem");
            if (dontes)
            {
                System.Environment.Exit(0);
            }
            else
            {
                return;
            }
        }
    }
}