using ApiHelper;
using Cards;
using System.Diagnostics;
using System.Text.Json;

namespace Personajes{

//Jugador - A priori no habrán grandes cambios. 
public class Personaje{
    private string name;
    private bool esJugable;
    private int bank;
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
    public bool EsJugable { get => esJugable; set => esJugable = value; }
    public bool IsBigBlind {get; set;} = false;
    public bool IsFolded {get; set;} = false;

        public Personaje(){
        var rand = new Random();
        Aura = rand.Next(100);
        Tells = rand.Next(100);
        Cheat = rand.Next(50);
        Luck = rand.Next(100);

    }
    //Métodos para actuar en la partida.
    public int PayBlind(int amount){
        if(IsBigBlind) return amount;
        else return amount/2; 
    }
    public void Fold(){
        IsFolded = true;
    }
    public int Bet(int min){
        int.TryParse(Console.ReadLine(), out int bet);
        do
        {
            if(bet == 0) Console.WriteLine($"Tu apuesta debe superar el mínimo (${min})");
            else Console.WriteLine("Tu apuesta es mayor a tu dinero disponible.");
            int.TryParse(Console.ReadLine(), out bet);
        } while (bet < min || bet > bank);
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
        return aux;
    }
    public void CashPot(int cash){
        bank += cash;
    }
    
    public void MostrarStats(){
        Console.WriteLine("Nombre:" + Name);
        Console.WriteLine("Es jugable?:" + EsJugable);
        Console.WriteLine("Suerte:" + Luck);
        Console.WriteLine("Aura:" + Aura);
        Console.WriteLine("tells:" + tells);
        Console.WriteLine("Trampa:" + Cheat);
        Console.WriteLine("Mano:");
        hand.Show();
    }
    //Todo: Implementar una habilidad, de una lista de habilidades, como metodos en cada instancia. Implementarlo a través de ExtensionMethods
}

public class Npc : Personaje {

    public int handStrenght;

    public Npc(Personaje p){
        Name = p.Name;
        Aura = p.Aura;
        Caution = p.Caution;
        Cheat = p.Cheat;
        Luck = p.Luck;
        Mind = p.Mind;
        Tells = p.Tells;
    }

    public double CalcRelativeStrenght(Personaje rival){
        double Ahs = Hand.Value;//Abs : Absolute Hand Strenght
        double TendsAgrro = ((Aura * Mind * 0.05 ) + (rival.Tells * (Mind * 0.04)) + (Luck * 0.7)) * 0.001;
        double TendsPasive = (rival.Aura * (Caution * Mind * 0.03) * .01 + Tells * Mind * 0.02  ) * .001;
        double descitionRatio = 0.5 + (TendsAgrro - TendsPasive);// Debe ser un numero entre 0,5 y 1,5
        
        return Ahs * descitionRatio;
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
