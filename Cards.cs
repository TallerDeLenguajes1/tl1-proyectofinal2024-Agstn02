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
        CartaAlta,
        Par,
        DoblePar,
        Escalera,
        Color,
        FullHouse,
        Trio,
        EscaleraColor,
        Poquer,
        EscaleraReal
    }
    //Clase HAND - Maneja las cartas que se reparten y las cartas de mesa. Identifica la jugada y permite modificar las cartas.
    public class Hand(List<Card> poket)
    {
        private List<Card> _hand = poket;
        private Value value;
        public Value Value { get => value;}

        //Métodos:
        public void GetCards(List<Card> tableCards){
            _hand.AddRange(tableCards);
        }
        public void DefineValue(){
            
            Card[] sorted = [.. _hand];
            Array.Sort(sorted);

            foreach (var item in sorted)
            {
                Console.WriteLine(item.Code);
            }

        }
        public void Show(){
            foreach (var item in _hand)
            {
                Console.WriteLine(item.Code);                
            }
        }
    }

    //Clase Deck
    public class Deck{ 
        private List<Card> cards;
        
        public List<Card> Cards { get => cards; set => cards = value; } //Uso una IList para poder conocer la posición de Cards que pueda necesitar y llamar a ese index.

        //Encontrar la forma de que sea el constructor de clase.
        public async Task Shuffle(){
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
            Card card = Cards[0];
            Cards.RemoveAt(0);
            return card;
        }

        public List<Card> DealPoket(){ //Reparte las poket cards, no se intercalan pero a fines del juego no hay problema.
            List<Card> poket = new();
            poket.Add(Deal());
            poket.Add(Deal());
            return poket;
        }
        public List<Card> DealTable(){
            List<Card> table = new();
            for (int i = 0; i < 5; i++)
            {
                table.Add(Deal());
            }
            return table;
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