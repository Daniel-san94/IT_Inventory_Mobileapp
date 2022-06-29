using IT_Inventory_Mobileapp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IT_Inventory_Mobileapp
{
    public partial class MainPage : ContentPage
    {

        /// <summary>
        /// MainPage metódus inicializálja a komponenseket és meghívja a GetAll függvényt.
        /// </summary>

        public MainPage()
        {
            InitializeComponent();

            GetAll();
        }
        HttpClient client;


        /// <summary>
        /// Példányosítunk egy HttpClientHandlert, majd elhárítja a validációs hibákat. A HttpClientHandler azért kellett mert ssl hibát kaptam.
        /// A response változóba async meghívom Get kéréssel az apit. Az items változóba Deserializálom a responset egy Item tipusú listába.
        /// Majd a LeltarListView ItemSourcenak megadom az items változót vagyis a deserializált listát.
        /// </summary>
        private async void GetAll()
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
   (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);
            var response = await client.GetStringAsync("https://192.168.1.176:44391/api/TE_Leltar");

            var items = JsonConvert.DeserializeObject<List<Item>>(response);

            LeltarListView.ItemsSource = items;

        }

    }
}

