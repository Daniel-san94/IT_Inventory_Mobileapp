using IT_Inventory_Mobileapp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IT_Inventory_Mobileapp.Views
{
    public partial class NewItemPage : ContentPage
    {
        private const string url = "https://192.168.1.176:44391/api/TE_Leltar/";
        private ObservableCollection<Item> _item = new ObservableCollection<Item>();

        public NewItemPage()
        {
            InitializeComponent();
            GetContent();
        }
        HttpClient client;


        private async void GetContent()
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);
            var response = await client.GetStringAsync("https://192.168.1.176:44391/api/TE_Leltar");

            var items = JsonConvert.DeserializeObject<List<Item>>(response);

            var helyek = (from s in items select new { s.Hely }.Hely).Distinct().ToList();
            var felhasznalok = (from s in items select new { s.Felhasznalo }.Felhasznalo).Distinct().ToList();
            var csoportok = (from s in items select new { s.Csoport }.Csoport).Distinct().ToList();
            var statuszok = (from s in items select new { s.Statusz }.Statusz).Distinct().ToList();
            var tipusok = (from s in items select new { s.Tipusok }.Tipusok).Distinct().ToList();
            var gyartok = (from s in items select new { s.Gyarto }.Gyarto).Distinct().ToList();
            var modellek = (from s in items select new { s.Modell }.Modell).Distinct().ToList();

            piHelyek.ItemsSource = helyek;
            piFelhasznalo.ItemsSource = felhasznalok;
            piCsoport.ItemsSource = csoportok;
            piStatus.ItemsSource = statuszok;
            piTipus.ItemsSource = tipusok;
            piGyarto.ItemsSource = gyartok;
            piModell.ItemsSource = modellek;
        }
        private void piHelyek_SelectedIndexChanged(object sender, EventArgs e)
        {
            entHely.Text = piHelyek.SelectedItem.ToString();
        }

        private void piFelhasznalo_SelectedIndexChanged(object sender, EventArgs e)
        {
            entFelhasznalo.Text = piFelhasznalo.SelectedItem.ToString();
        }

        private void piCsoport_SelectedIndexChanged(object sender, EventArgs e)
        {
            entCsoport.Text = piCsoport.SelectedItem.ToString();
        }

        private void piStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            entStatus.Text = piStatus.SelectedItem.ToString();
        }

        private void piTipus_SelectedIndexChanged(object sender, EventArgs e)
        {
            entTipus.Text = piTipus.SelectedItem.ToString();
        }

        private void piGyarto_SelectedIndexChanged(object sender, EventArgs e)
        {
            entGyarto.Text = piGyarto.SelectedItem.ToString();
        }

        private void piModell_SelectedIndexChanged(object sender, EventArgs e)
        {
            entModell.Text = piModell.SelectedItem.ToString();
        }


        /// <summary>
        /// A NewItemPagen ha a mentés gombra kattintunk, akkor aktiválódik a metódus. serializáljuk az item-et a json változóba.
        /// Példányosítunk egy HttpClientHandlert, majd elhárítja a validációs hibákat. A HttpClientHandler azért kellett mert ssl hibát kaptam.
        /// A result változóba async meghívom Post kéréssel az apit. 
        /// Ha OK státuszkódot kapunk vissza, akkor megjelnik egy ablak ami tájékoztat az új itemről.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            Item item = new Item()
            {
                Nev = entNev.Text,
                Hely = entHely.Text,
                Felhasznalo = entFelhasznalo.Text,
                Csoport = entCsoport.Text,
                Statusz = entStatus.Text,
                Tipusok = entTipus.Text,
                Gyarto = entGyarto.Text,
                Modell = entModell.Text,
                Sorozatszam = entSorozatszam.Text,
                LeltariSzam = entLeltariszam.Text
            };

            var json = JsonConvert.SerializeObject(item);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client;

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
   (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);
            var result = await client.PostAsync("https://192.168.1.176:44391/api/TE_Leltar", content);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                await DisplayAlert("Figyelem!", "Az új rekord rögzítve", "Ok");
                await Navigation.PushAsync(new NewItemPage());
            }
            else
            {
                await DisplayAlert("Figyelem!", "Sikertelen mentés, minden mezőt kötelező kitölteni", "Ok");
                return;
            }

        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            entNev.Text = "";
            entHely.Text = "";
            entFelhasznalo.Text = "";
            entCsoport.Text = "";
            entStatus.Text = "";
            entTipus.Text = "";
            entGyarto.Text = "";
            entModell.Text = "";
            entSorozatszam.Text = "";
            entLeltariszam.Text = "";
        }

    }
}