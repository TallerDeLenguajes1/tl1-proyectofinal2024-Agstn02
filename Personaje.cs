using ApiHelper;
using Cards;
using System.Diagnostics;
using System.Text.Json;

namespace Personajes{

//Jugador - A priori no habrán grandes cambios. 
public class Personaje{
    private string name;
    private string edad;
    private bool esJugable;
    private int bank;
    private int luck;
    private int aura;
    private int fish;
    private int cheat;
    private Hand hand;

    public string Name { get => name; set => name = value; }
    public string Edad { get => edad; set => edad = value; }
    public int Bank { get => bank; set => bank = value; }
    public int Luck { get => luck; set => luck = value; }
    public int Aura { get => aura; set => aura = value; }
    public int Fish { get => fish; set => fish = value; }
    public int Cheat { get => cheat; set => cheat = value; }
    public Hand Hand { get => hand; set => hand = value; }
    public bool EsJugable { get => esJugable; set => esJugable = value; }
    public bool IsBigBlind {get; set;} = false;//Uso la propiedad que como default tiene el valor false.
    public bool IsFolded {get; set;} = false;

        public Personaje(){
        var rand = new Random();
        Edad = rand.Next(17,100).ToString();                                                                                                                                                                                                                                                                            ;
        Luck = rand.Next(100);
        Aura = rand.Next(100);
        Fish = rand.Next(100);
        Cheat = rand.Next(50);

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
        return bet;
    }
    public int Call(int bet){
        if(bet > bank){
            return AllIn();
        }
        bank -= bet;
        return bet;
    }
    public int AllIn(){
        int aux = bank;
        bank = 0;
        return aux;
    }
    public void GainPot(int cash){
        bank += cash;
    }
    
    public void MostrarStats(){
        Console.WriteLine("Nombre:" + Name);
        Console.WriteLine("Edad:" + Edad);
        Console.WriteLine("Es jugable?:" + EsJugable);
        Console.WriteLine("Suerte:" + Luck);
        Console.WriteLine("Aura:" + Aura);
        Console.WriteLine("Fish:" + Fish);
        Console.WriteLine("Trampa:" + Cheat);
        Console.WriteLine("Mano:");
        hand.Show();
    }
    //Todo: Implementar una habilidad, de una lista de habilidades, como metodos en cada instancia. Implementarlo a través de ExtensionMethods
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
