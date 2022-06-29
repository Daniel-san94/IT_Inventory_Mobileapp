using IT_Inventory_Mobileapp.Models;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;

namespace IT_Inventory_Mobileapp.Views
{
    public partial class ItemDetailPage : ContentPage
    {

        /// <summary>
        /// Az item detail pagenek átadott paraméterek segítségével megjelenítem a kiválasztott item részleteit.
        /// </summary>
        /// <param name="Nid"></param>
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
        public ItemDetailPage(int Nid, string Nev, string Hely, string Felhasznalo, string Csoport, string Status,
            string Tipusok, string Gyarto, string Modell, string Sorozatszam, string LeltariSzam)
        {
            InitializeComponent();
            lbNid.Text = Nid.ToString();
            lbNev.Text = Nev;
            lbHely.Text = Hely;
            lbFelhasznalo.Text = Felhasznalo;
            lbCsoport.Text = Csoport;
            lbStatusz.Text = Status;
            lbTipus.Text = Tipusok;
            lbGyarto.Text = Gyarto;
            lbModell.Text = Modell;
            lbSorozatszam.Text = Sorozatszam;
            lbLelatariszam.Text = LeltariSzam;
        }

        /// <summary>
        /// Az item detail pagen a toolbaron ha rányomunk az edit gombra akkor átnavigálunk az UpdateItemPage-re és paraméterként átadom neki az item tulajdonságait.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private async void tbiEdit_Clicked(object sender, System.EventArgs e)
        {
            string Nnid = lbNid.Text;
            string Nev = lbNev.Text;
            string Hely = lbHely.Text;
            string Felhasznalo = lbFelhasznalo.Text;
            string Csoport = lbCsoport.Text;
            string Status = lbStatusz.Text;
            string Tipusok = lbTipus.Text;
            string Gyarto = lbGyarto.Text;
            string Modell = lbModell.Text;
            string Sorozatszam = lbSorozatszam.Text;
            string Leltariszam = lbLelatariszam.Text;


            await Navigation.PushAsync(new UpdateItemPage(Nnid, Nev, Hely, Felhasznalo, Csoport, Status, Gyarto,
                Tipusok, Modell, Sorozatszam, Leltariszam));

        }

        /// <summary>
        /// Az ItemDetailPage-n ha rányomunk a delete gombra, akkor aktiválódik a metódus.
        /// Példányosítunk egy HttpClientHandlert, majd elhárítja a validációs hibákat. A HttpClientHandler azért kellett mert ssl hibát kaptam.
        /// A response változóba async meghívom Delete kéréssel az apit a törölni kívánt item id-jára.
        /// Ha OK státusz kódot kapunk vissza akkor megjelenik egy ablak ami tájékoztat minket a sikeres törlésről.
        /// Majd visszanavigálunk az ItemsPage-re.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private async void tbiDelete_Clicked(object sender, System.EventArgs e)
        {
            HttpClient client;

            var dontes = await DisplayAlert("Figyelem!", "Biztosan törli a rekordot?",
                "Igen", "Nem");
            if (dontes)
            {
                var httpClientHandler = new HttpClientHandler();

                httpClientHandler.ServerCertificateCustomValidationCallback =
       (message, cert, chain, errors) => { return true; };

                client = new HttpClient(httpClientHandler);
                var result = await client.DeleteAsync(string.Concat
                    ("https://192.168.1.176:44391/api/TE_Leltar/", lbNid.Text));

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    await DisplayAlert("Figyelem!", "A rekord törölve lett!", "Ok");
                }

                await Navigation.PushAsync(new ItemsPage());
            }
            else
            {
                return;
            }
        }
    }
}