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
    public enum HandValue {
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
    //Uso la clase PokerStraights para poder deserializar el resultado en json desde el archivo en la carpeta assets.
    public class StraightsValues{
        public List<int[]> PokerStraight {get ; set;}
    } 
    //Clase HAND - Maneja las cartas que se reparten y las cartas de mesa. Identifica la jugada y permite modificar las cartas.
    public class Hand(List<Card> poket)
    {
        private List<Card> _hand = poket;
        private HandValue value;
        public HandValue Value { get => value;}

        //Métodos:
        //GetCards() añade un rango de cartas a mi lista _hand.
        public void GetCards(List<Card> tableCards){
            _hand.AddRange(tableCards);
        }
        //CountSuits() devuelve un diccionario con la cantidad de cartas de cada suit.
        private Dictionary<string, int> CountSuits(List<Card> hand){
            Dictionary<string , int> suits = new();
            suits.Add("SPADES",0);
            suits.Add("HEARTS",0);
            suits.Add("CLUBS",0);
            suits.Add("DIAMONDS",0);
            foreach (Card item in hand)
            {
                switch (item.Suit)
                {
                    case "SPADES":
                    suits["SPADES"] += 1;
                    break;
                    case "HEARTS":
                    suits["HEARTS"] += 1;
                    break;
                    case "CLUBS":
                    suits["CLUBS"] += 1;
                    break;
                    case "DIAMONDS":
                    suits["diamonds"] += 1;
                    break;
                }
            }
            return suits;
        }
        private string NormalizeHand(){
            //Creo una Lista de los valores parseados a su valor entero.
            List<int> ParsedValues = new();
            for (int i = 0; i < _hand.Count; i++)
            {
                ParsedValues.Add( _hand[i].Value switch
                    {
                        "J" => 11,
                        "Q" => 12,
                        "K" => 13,
                        "A" => 14,
                        _ => int.Parse(_hand[i].Value),
                    });
            }
            //Ordeno La lista 
            ParsedValues.Sort();
            //Combierto la Lista a un string para poder compararlo facilmente. cheaqueando la posibilidad de que un As pueda usarse como 2.
            if(ParsedValues.Contains(14)){
                //Si existe un As en la mano inserto el valor 14(el as) al principio de esta y puedo asegurarme que al hacer el contain luego, el as ocupe el lugar de 1 y 14 simultaneamente. Como solo voy a evaluar patrones de 5 caracteres no afecta de ninguna manera.
                ParsedValues.Insert(0,14);
            }
            return ParsedValues.ToString(); 
        }
        private bool IsStraight(){
            var StringValues = NormalizeHand();
            //Verifico si existe una escalera de algun tipo.
            //Para eso uso el archivo straight.json
            string StraightsFile = File.ReadAllText("assets/straight.json");
            //Deserializo en la clase StraightsValues
            var straigths = JsonSerializer.Deserialize<StraightsValues>(StraightsFile);
            //Itero en la lista de arrays de los valores para buscar alguna coincidencia:
            foreach (var item in straigths.PokerStraight)
            {
                if(StringValues.Contains(item.ToString())){//Si para el string(obtenido de un int[]) existe una coincidencia entonces es una escalera.
                    return true;
                }
            }
            //Si para ningun miembro de la lista de int[]'s  entonces no es una escalera.
            return false;
        }
        private bool IsRoyal(){
            //Uso la misma estrategia que en IsStraight()
            string Royal = "1011121314";
            string OrderedHand = NormalizeHand();
            if (OrderedHand.Contains(Royal))
            {
                return true;
            }
            return false;
        }
        public void DefineValue(){
            var suits = CountSuits(_hand);
            bool IsFlushed = suits.ContainsValue(5) | suits.ContainsValue(6) | suits.ContainsValue(7);
            if(IsFlushed){ // Posible escalera real, escalera de color o color.
                if(!IsStraight()){
                    value = HandValue.Color;
                }
                else if(!IsRoyal())
                {
                    value = HandValue.EscaleraColor;
                }
                else
                {
                    value = HandValue.EscaleraReal;
                }
            }
            else 
            {
                //Puede ser cualquier jugada que no involucre los suits, y amerita evaluar los valores de las cartas.

            }

        }
        //Show() muestra los valores de cada carta.
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