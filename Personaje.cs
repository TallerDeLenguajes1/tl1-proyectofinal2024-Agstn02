using ApiHelper;
using Cards;
using System.Diagnostics;
using System.Text.Json;

namespace Personajes{

//Jugador - A priori no habrán grandes cambios. 
public class Personaje{
    private string name;
    private int bank = 500;
    private float aura;
    private float caution;
    private float cheat;
    private float luck;
    private float mind;
    private float tells;
    private Hand hand;

    public string Name { get => name; set => name = value; }
    public int Bank { get => bank; set => bank = value; } 
    public float Aura { get => aura; set => aura = value; }
    public float Caution { get => caution; set => caution = value; }
    public float Cheat { get => cheat; set => cheat = value; }
    public float Luck { get => luck; set => luck = value; }
    public float Mind { get => mind; set => mind = value; }
    public float Tells { get => tells; set => tells = value; }
    public Hand Hand { get => hand; set => hand = value; }
    public int CurrentBet { get; set;}
    public bool IsBigBlind {get; set;} = false;
    public bool IsFolded {get; set;} = false;

        public Personaje(){
        var rand = new Random();
        Aura = rand.Next(10, 100);
        Caution = rand.Next(10,100);
        Cheat = rand.Next(5,50);
        Luck = rand.Next(10,100);
        Mind = rand.Next(10,100);
        Tells = rand.Next(100);

    }
    //Métodos para actuar en la partida.
    public int PayBlind(int amount){
        bank -= amount;
        CurrentBet = amount;
        return amount;
    }
    public void Fold(){
        IsFolded = true;
    }
    public virtual int Bet(int min){
        Console.Write("ingresa el monto de tu apuesta:");
        int.TryParse(Console.ReadLine(), out int bet);
        do
        {
            if(bet == 0) Console.WriteLine($"No puedes apostar $0. Avíspese!");
            else if (bet > bank){Console.WriteLine("Tu apuesta es mayor a tu dinero disponible.");}
            int.TryParse(Console.ReadLine(), out bet);
        } while (bet < 0 || bet > bank);
        bank -= bet;
        CurrentBet += bet;
        return bet;
    }
    public int Call(int bet){
        if(bet > bank){
            return AllIn();
        }
        bank -= bet;
        CurrentBet += bet;
        return bet;
    }
    public int AllIn(){
        int aux = bank;
        bank = 0;
        CurrentBet += aux;
        return aux;
    }
    public void CashPot(int cash){
        bank += cash;
    }
    
    public void MostrarStats(){
        Console.WriteLine("Nombre:" + Name);
        Console.WriteLine("Aura:" + Aura);
        Console.WriteLine("Precaución:" + Caution);
        Console.WriteLine("Suerte:" + Luck);
        Console.WriteLine("Gestos:" + tells);
        Console.WriteLine("Trampa:" + Cheat);
    }
}

public class Npc : Personaje {

    public double HandStrenght { get ; set; }

    public Npc(Personaje p){
        Name = p.Name;
        Aura = p.Aura;
        Caution = p.Caution;
        Cheat = p.Cheat;
        Luck = p.Luck;
        Mind = p.Mind;
        Tells = p.Tells;
    }

    public void CalcRelativeStrenght(Personaje rival){
        Hand.DefineValue();
        double Ahs = Hand.Value;//Abs : Absolute Hand Strenght
        double TendsAgrro = 0.2 + ((Aura * Mind * 0.05 ) + (rival.Tells * (Mind * 0.02)) + (Luck * 0.15)) * 0.001;//max .91 - min .216
        double TendsPasive = 0.1 +(rival.Aura * (Caution * Mind * 0.03) * .01 + Tells * Mind * 0.01 ) * .001;//max .5 - min .140
        double descitionRatio = 0.5 + (TendsAgrro - TendsPasive);// Debe ser un numero entre 0,5 y 1
        
        HandStrenght = Ahs * descitionRatio;//Max 126.99(...) - Min 10.77(...)
        //Tomaremos como manos debiles desde Min hasta 58.
        //Manos regulares desde 58 hasta 99 (Totalmente arbitrario).
        //Manos buenas > 99.
    }
    public override int Bet(int min){
        var rand = new Random();
        int bet = min;
        do
        {
            bet = min * rand.Next(2,3);
        } while (bet > Bank);
        Bank -= bet;
        CurrentBet += bet;
        return bet;
    }
}
//Clase Fabrica de Personajes.
public class FabricaDePersonajes{

    //TODO crear un metodo para crear 1 personaje y usarlo en el metodo estático.
    //Método estatico para crear una lista de personajes.
    public static async Task<List<Personaje>> CrearListaPersonajes(int n){
        //Ver la forma de que no me caguen las opciones del serializer 
        JsonSerializerOptions jsonOPt = new(){
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower//Fundamentalmente esta linea
            };
        //Creacion de una clase para la creacion de numeros aleatorios y la lista de personajes.
        var listaPersonajes = new List<Personaje>();
        //Iteracion para la creacion de personajes y adicion a la lista
        for (int i = 1; i <= n; i++)
        {
            //Obtención del nombre del personaje
            string url = $"https://rickandmortyapi.com/api/character/{i}";
            var stringResponse = await GET.From(url);
            var personaje = JsonSerializer.Deserialize<Personaje>(stringResponse, jsonOPt);
            // Aleatorización de las caracteristicas de los personajes
            
            listaPersonajes.Add(personaje);
        }
        return listaPersonajes;
    }

}

public class PersonajesJson{
    public bool Existe(string ruta){
        var existe = File.Exists(ruta);
        return existe;
    }
    public void GuardarPersonajes(List<Personaje> lista, string ruta){
        if(!Existe(ruta)){ //Si el archivo no existe lo crea.
            File.Create(ruta);
        }

        foreach (Personaje pj in lista)// para cada personaje en la lista se transforma en json y se escribe en el archivo recibido 
        {
            string personaje = JsonSerializer.Serialize(pj);
            File.WriteAllText(ruta,personaje);// Quiza tenga que usar append.
        }
    }
    public List<Personaje> LeerPersonajes(string ruta){
        var personajes  = File.ReadAllText(ruta);
        var lista = JsonSerializer.Deserialize<List<Personaje>>(personajes);
        return lista;
    }
}

}
