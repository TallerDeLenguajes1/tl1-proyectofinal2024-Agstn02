using System.Text.Json;
using ApiHelper;

namespace Cards
{
    public class Card {
        private string code;
        private string value;
        private string suit;

        public string Code { get => code; set => code = value; }
        public string Value { get => value; set => this.value = value; }
        public string Suit { get => suit; set => suit = value; }
    }
    //Enum de Values Para compararlos:
    public enum Value {
        EscaleraReal,
        Poquer,
        EscaleraColor,
        FullHouse,
        Color,
        Escalera,
        Trio,
        DoblePar,
        Par,
        CartaAlta
    }
    //Clase HAND - Maneja las cartas que se reparten y las cartas de mesa. Identifica la jugada y permite modificar las cartas.
    public class Hand{
        private List<Card> _hand;
        private Value value;
        public Value Value { get => value;}
        
        public Hand(List<Card> hand){
            _hand = hand;
        }
        //Métodos:
        public void AddTableCards(List<Card> tableCards){
            _hand.AddRange(tableCards);
        }
        public void DefineValue(){
            
        }

    }

    //Clase Deck
    public class Deck{
        private List<Card> cards;
        public List<Card> Cards { get => cards; set => cards = value; } //Uso una IList para poder conocer la posición de Cards que pueda necesitar y llamar a ese index.

        //Encontrar la forma de que sea el constructor de clase.
        public async void Suffle(){
            //Sin instanciar las Opciones del serializer no podría Deserializar.
            JsonSerializerOptions DoCards = new(){
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower//Fundamentalmente esta linea
            };

            string url = "https://deckofcardsapi.com/api/deck/new/draw/?count=52";
            var body = await GET.From(url);
            var mazo = JsonSerializer.Deserialize<Deck>(body, DoCards);
            
            Cards = mazo.Cards;
        }

        public Card Deal(){
            var card = Cards[0];
            Cards.RemoveAt(0);
            return card;
        }
        
        public void Show(){
            var mazo = Cards;
            foreach (var card in mazo)
            {
                Console.WriteLine(card.Code);
            }
        }

    }   
}