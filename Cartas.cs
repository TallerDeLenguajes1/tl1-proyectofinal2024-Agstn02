using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Cartas
{
    public class Carta {
        private string code;
        private string value;
        private string suit;

        public string Code { get => code; set => code = value; }
        public string Value { get => value; set => this.value = value; }
        public string Suit { get => suit; set => suit = value; }



    }

    public class Baraja{
        private bool success;
        private string deck_id;
        private IList<Carta> cards;

        public bool Success { get => success; set => success = value; }
        public string Deck_id { get => deck_id; set => deck_id = value; }
        public IList<Carta> Cards { get => cards; set => cards = value; }



        public static async Task<Baraja> Shuffle(){
            //modificar para usarlo como constructor y no como metodo estático.            
            //Sin instanciar las Opciones del serializer no podría Deserializar.
            //TODO: abstraer en otra clase y nuevo namespace
            JsonSerializerOptions DoCards = new(){
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower//Fundamentalmente esta linea
            };

            //TODO: abstraer en otra clase y nuevo namespace
            //Creo la instancia de cliente para hacer la petición
            HttpClient client = new();
            //Hago la peticion
            string url = "https://deckofcardsapi.com/api/deck/new/draw/?count=52";
            HttpResponseMessage response = await client.GetAsync(url);
            //Hago el response Ensure
            response.EnsureSuccessStatusCode();
            //Hago un string a partir de el body de mi response
            string body = await response.Content.ReadAsStringAsync();
            
            var mazo = JsonSerializer.Deserialize<Baraja>(body, DoCards);
            
            return mazo;
        }
    }
    
}