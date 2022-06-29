using IT_Inventory_Mobileapp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IT_Inventory_Mobileapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateItemPage : ContentPage
    {

        /// <summary>
        /// Az ItemDetailPage-től megkapjuk a kiválasztott item adatait mint paraméter. Aztán az update page mezőinek .Text értékével egyenlővé teszem azokat.
        /// </summary>
        /// <param name="Nnid"></param>
        /// <param name="Nev"></param>
        /// <param name="Hely"></param>
        /// <param name="Felhasznalo"></param>
        /// <param name="Csoport"></param>
        /// <param name="Status"></param>
        /// <param name="Tipusok"></param>
        /// <param name="Gyarto"></param>
        /// <param name="Modell"></param>
        /// <param name="Sorozatszam"></param>
        /// <param name="LeltariSzam"></param>
        public UpdateItemPage(string Nnid, string Nev, string Hely, string Felhasznalo, string Csoport, string Status, string Tipusok, string Gyarto, string Modell, string Sorozatszam, string LeltariSzam)
        {
            InitializeComponent();

            lblNid.Text = Nnid;
            entNev.Text = Nev;
            entHely.Text = Hely;
            entFelhasznalo.Text = Felhasznalo;
            entCsoport.Text = Csoport;
            entStatus.Text = Status;
            entTipus.Text = Tipusok;
            enGyarto.Text = Gyarto;
            entModell.Text = Modell;
            entSorozatszam.Text = Sorozatszam;
            entLeltariszam.Text = LeltariSzam;

        }

        /// <summary>
        /// Miután az update page Mentés gombjára nyomunk, utána aktiválódik a metódus.
        /// A json változóba serializálom az itemet.
        /// Példányosítunk egy HttpClientHandlert, majd elhárítja a validációs hibákat. A HttpClientHandler azért kellett mert ssl hibát kaptam.
        /// A response változóba async meghívom Put kéréssel az apit a kiválasztott item id-jára.
        /// Ha Ok státuszkódot kapunk vissza, akkor egy ablak értesít minket a sikeres módosításról.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            Item item = new Item()
            {
                Nid = Convert.ToInt32(lblNid.Text),
                Nev = entNev.Text,
                Hely = entHely.Text,
                Felhasznalo = entFelhasznalo.Text,
                Csoport = entCsoport.Text,
                Statusz = entStatus.Text,
                Tipusok = entTipus.Text,
                Gyarto = enGyarto.Text,
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
            var result = await client.PutAsync(string.Concat("https://192.168.1.176:44391/api/TE_Leltar/", lblNid.Text), content);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                await DisplayAlert("Figyelem!", "A rekord módosítva!", "Ok");
            }

        }

    }
}