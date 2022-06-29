using IT_Inventory_Mobileapp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IT_Inventory_Mobileapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LekerdezesPage : ContentPage
    {
        private const string url = "https://192.168.1.176:44391/api/TE_Leltar/";
        private ObservableCollection<Item> _item = new ObservableCollection<Item>();

        public LekerdezesPage()
        {
            InitializeComponent();
            GetHelyek();
        }

        HttpClient client;
        /// <summary>
        /// A GetHelyek metódus lekéri az összes itemet az adatbázisból, majd a helyek változóba egy Linq lekérdezés segítségével kiszedem az összes helységet,
        /// ügyelve arra, hogy mindegyik hely csak egyszer szerepeljen. Ebben segít a Distinct. Majd a piHely nevű picker ItemSource-aként átadom a helyek változó tartalmát.
        /// </summary>
        private async void GetHelyek()
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
   (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);
            var response = await client.GetStringAsync("https://192.168.1.176:44391/api/TE_Leltar");

            var items = JsonConvert.DeserializeObject<List<Item>>(response);

            var helyek = (from s in items select new { s.Hely }).Distinct().ToList();
            piHely.ItemsSource = helyek;

        }

        /// <summary>
        /// Az IDLekerdezes_OnClicked metódus az ID szerinti lekérdezés, Lekérdezés gombjának megnyomásakor aktiválódik.
        /// Megvizsgálja, hogy az IDEntry.Text egyenlő e Null-al. Ha igen akkor returnol.
        /// Ha az IDEntry.Text nem egyenlő null, akkor regex segítségével megvizsgálja, hogy szám van-e beírva.
        /// Ha megfelel a kritériumoknak, akkor a final_url nevű változóba összerakja a végleges url-t,  átnavigál a LekerdezesIDPage-re és paraméterként átadom neki az url-t
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void IDLekerdezes_OnClicked(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");

            if (IDEntry.Text == null)
            {
                return;
            }
            if (regex.IsMatch(IDEntry.Text))
            {
                string text = IDEntry.Text;
                string final_url = url + text;

                await Navigation.PushAsync(new LekerdezesIDPage(final_url));
            }

        }

        /// <summary>
        /// Az LeltariSzamLekerdezes_OnClicked metódus a leltári szám szerinti lekérdezés, Lekérdezés gombjának megnyomásakor aktiválódik.
        /// Megvizsgálja, hogy az Leltari_SzamEntry.Text egyenlő e Null-al. Ha igen akkor returnol.
        /// A leltariszam_url nevű változóba összerakja a végleges url-t, átnavigál a LekerdezesIDPage-re és paraméterként átadom neki az url-t.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void LeltariSzamLekerdezes_OnClicked(object sender, EventArgs e)
        {

            if (Leltari_SzamEntry.Text == null)
            {
                return;
            }
            string text = Leltari_SzamEntry.Text;
            string leltariszam = "leltariszam/";
            string leltariszam_url = url + leltariszam + text;

            await Navigation.PushAsync(new LekerdezesLeltariszamPage(leltariszam_url));

            Leltari_SzamEntry.Text = "";


        }

        public async void SorozatSzamLekerdezes_OnClicked(object sender, EventArgs e)
        {

            if (Sorozat_SzamEntry.Text == null)
            {
                return;
            }
            string text = Sorozat_SzamEntry.Text;
            string sorozatszam = "sorozatszam/";
            string sorozatszam_url = url + sorozatszam + text;
            await Navigation.PushAsync(new LekerdezesSorozatSzam(sorozatszam_url));
            Sorozat_SzamEntry.Text = "";

        }

        /// <summary>
        /// Az HelyLekerdezes_OnClicked metódus a hely szerinti lekérdezés, Lekérdezés gombjának megnyomásakor aktiválódik.
        /// Megvizsgálja, hogy az piHely.SelectedItem egyenlő e Null-al. Ha igen akkor returnol.
        /// A hely_url nevű változóba összerakja a végleges url-t, átnavigál a LekerdezesHelyPage-re és paraméterként átadom neki az url-t
        /// Ha van '/' jel a helynévben akkor csak a jel előtti részre keresek rá az adatbázisban, ezért szedtem ki a '/' jel előtti részt
        /// a final_text változóba.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HelyLekerdezes_OnClicked(object sender, EventArgs e)
        {
            if (piHely.SelectedItem == null)
            {
                return;
            }
            string text = piHely.SelectedItem.ToString();

            string[] textsplit = text.Split('=', '}');

            string helynev = textsplit[1].Trim();
            string hely = "hely/";
            string final_text = helynev.Split('/')[0];
            string hely_url = url + hely + final_text;
            await Navigation.PushAsync(new LekerdezesHelyPage(hely_url));
            piHely.SelectedItem = null;
        }
    }

}