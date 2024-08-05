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
    public static class HandValue{
        public const double CartaAlta = 49.883;
        public const double Par = 57.743;
        public const double DoblePar = 95.241;
        public const double Trio = 97.889;
        public const double Escalera = 99.608;
        public const double Color = 99.805;
        public const double FullHouse = 99.856;
        public const double Poquer = 99.976;
        public const double EscaleraColor = 99.99862;
        public const double EscaleraReal = 99.999864;
    }
    //Uso la clase PokerStraights para poder deserializar el resultado en json desde el archivo en la carpeta assets.
    public class PokerStraight{
        public List<int[]> PokerStraights {get ; set;}
    } 
    //Clase HAND - Maneja las cartas que se reparten y las cartas de mesa. Identifica la jugada y permite modificar las cartas.
    public class Hand(List<Card> poket)
    {
        private List<Card> _hand = poket;
        private double value;
        private KeyValuePair<int, int>[] higherValues;
        //Props
        public double Value { get => value;}
        public KeyValuePair<int, int>[] HigherValues { get => higherValues; }

        //Métodos:
        //GetCard() permite añadir 1 solo miembro a la lista.
        public void GetCard(Card tableCard){
            _hand.Add(tableCard);
        }
        //GetCards() añade un rango de cartas a mi lista _hand.
        public void GetCards(List<Card> tableCards){
            _hand.AddRange(tableCards);
        }
        //CountSuits() devuelve un diccionario con la cantidad de cartas de cada suit.
        private Dictionary<string, int> CountSuits(){
            Dictionary<string , int> suits = new();
            foreach (Card item in _hand)
            {
                if(!suits.TryAdd(item.Suit, 1)){
                    suits[item.Suit] += 1;
                }
            }
            return suits;
        }
        //
        private Dictionary<string, int> CountValues(){
            Dictionary<string, int> values = [];
            foreach (var item in _hand)
            {
                if(!values.TryAdd(item.Value, 1))
                {
                    values[item.Value] += 1;
                }
            }
            return values;
        }
        private int Normalize(string val){
            return val switch
            {
                "JACK" => 11,
                "QUEEN" => 12,
                "KING" => 13,
                "ACE" => 14,
                _ => int.Parse(val),
            };
        }
        private List<int> NormalizeValues(){
           List<int> ParsedValues = [];
           foreach (var item in _hand)
           {
            ParsedValues.Add(Normalize(item.Value));
           }
            return ParsedValues;
        }
        private string NormalizeHand(){
            //Creo una Lista de los valores parseados a su valor entero.
            List<int> ParsedValues = NormalizeValues();
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
            var straigths = JsonSerializer.Deserialize<PokerStraight>(StraightsFile);
            //Itero en la lista de arrays de los valores para buscar alguna coincidencia:
            foreach (var item in straigths.PokerStraights)
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
        private void SetHigherValues(Dictionary<string, int> valuePairs){//Se usa para comparar en caso de que ambos jugadores tengan la misma mano.
            //Esta funcion crea un array de KVP de la siguiente manera
            Dictionary<int, int> casted = new();//Creo un nuevo dictionary que con el tipo <int , int>
            foreach (var item in valuePairs)
            {
                casted[Normalize(item.Key)] = item.Value;//uso la func Normalize() para transformar el Key a su valor correspondiente.
            }
            var arrKeyVal = casted.OrderByDescending(itm => itm.Value)//Ordena de forma descendiente segun los valores.
                            .ThenByDescending(itm => itm.Key)//Como criterio secundario usará el valor de la Key (El valor de la carta)
                            .ToArray();//Transformo en un array.
            higherValues = arrKeyVal;
        }
        public int[] GetValuesArray(){//uso una funcion para devolver los valores ordenados por cantidad de aparicion y valor de carta.
            int[] valuesArray = new int[7];//Siempre obtendré 7 elementos.
            int j = 0; // Uso un index externo para poder armar mi array
            foreach (var pair in higherValues)
            {
                int i = 0;
                while( i < pair.Value)
                {
                    valuesArray[j+i] = pair.Key;
                    i++;
                }
                j += i;
            }
            return valuesArray;
        }

        public void DefineValue(){
            var suits = CountSuits();
            var HigerValues = CountValues();// Cuenta la cantidad de valores repetidos y los inserta en la prop HigherValues.
            SetHigherValues(HigerValues);
            int[] pureba = GetValuesArray();

            bool IsFlushed = suits.ContainsValue(5) | suits.ContainsValue(6) | suits.ContainsValue(7);
            if(IsStraight()){ // Posible escalera real, escalera de color o escalera
                if(IsFlushed)
                {
                    if (IsRoyal())
                    {
                        value = HandValue.EscaleraColor;
                    }
                    else
                    {
                        value = HandValue.Color;
                    }
                }
                else
                {
                    value = HandValue.Escalera;
                }
            }
            else //Resto de jugadas.
            {
                //Puede ser cualquier jugada que no involucre los suits excepto color, y amerita evaluar los valores de las cartas.
                if(HigerValues.ContainsValue(4))//Hay un poker
                {
                    value = HandValue.Poquer;
                }
                else if (HigerValues.ContainsValue(3)) //Hay full o trio.(o color)  
                {
                    if(HigerValues.ContainsValue(2)){ //Si ademas de contener 3, contiene 2 => Es FullHouse.
                        value = HandValue.FullHouse;
                    }
                    else if (IsFlushed) //Es color
                    {
                        value = HandValue.Color;
                    }
                    else // Es trio o pierna.
                    {
                        value = HandValue.Trio;
                    }
                }
                else if(HigerValues.ContainsValue(2))//Hay par o doble par. (o color).
                {
                    int pairs = 0;
                    foreach (var value in HigerValues.Values) //Cuento la cantidad de Pares que hay. Si hay dos o mas entoces hay doble par.
                    {
                        if(value == 2 ){
                            pairs++;
                        }
                    }
                    if(pairs >= 2) //Hay doble par
                    {
                        value = HandValue.DoblePar;
                    }
                    else//Hay par
                    {
                        value = HandValue.Par;
                    }
                    if (IsFlushed)//Estoy odiando el color. Debo chequear igual porque podria existir un color
                    {
                        value = HandValue.Color;
                    }
                }
                else//Hay carta alta o color.
                {
                    if(IsFlushed){//Hay color 
                        value = HandValue.Color;
                    }
                    else // Hay carta alta.
                    {
                        value = HandValue.CartaAlta;
                    }
                }
            }
        }
        //Show() muestra los valores de cada carta.
        public void Show(){
            Console.WriteLine($"|[{_hand[0].Code}]| - |[{_hand[1].Code}]| ");
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
        public List<Card> DealFlop(){
            List<Card> table = new();
            for (int i = 0; i < 3; i++)
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