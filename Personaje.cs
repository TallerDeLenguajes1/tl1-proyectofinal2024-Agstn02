using ApiHelper;
using Cards;
using System.Text.Json;

namespace Personajes{

//Jugador - A priori no habrán grandes cambios. 
public class Personaje{
    private string name;
    private string edad;
    private bool esJugable;
    private int luck;
    private int aura;
    private int fish;
    private int cheat;
    private Card[] pocket;

    public string Name { get => name; set => name = value; }
    public string Edad { get => edad; set => edad = value; }
    public bool EsJugable { get => esJugable; set => esJugable = value; }
    public int Luck { get => luck; set => luck = value; }
    public int Aura { get => aura; set => aura = value; }
    public int Fish { get => fish; set => fish = value; }
    public int Cheat { get => cheat; set => cheat = value; }
    public Card[] Pocket { get => pocket; set => pocket = value; }

    public void MostrarStats(){
        Console.WriteLine(Name);
        Console.WriteLine(Edad);
        Console.WriteLine(EsJugable);
        Console.WriteLine(Luck);
        Console.WriteLine(Aura);
        Console.WriteLine(Fish);
        Console.WriteLine(Cheat);
        Console.WriteLine("Pocket cards:");
        Console.WriteLine(Pocket[0].Code);
        Console.WriteLine(Pocket[1].Code);
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
        var rand = new Random();
        var listaPersonajes = new List<Personaje>();
        //Iteracion para la creacion de personajes y adicion a la lista
        for (int i = 1; i <= n; i++)
        {
            //Obtención del nombre del personaje
            string url = $"https://rickandmortyapi.com/api/character/{i}";
            var stringResponse = await GET.From(url);
            var personaje = JsonSerializer.Deserialize<Personaje>(stringResponse, jsonOPt);
            // Aleatorización de las caracteristicas de los personajes
            personaje.Edad = rand.Next(17,100).ToString();                                                                                                                                                                                                                                                                            ;
            personaje.Luck = rand.Next(100);
            personaje.Aura = rand.Next(100);
            personaje.Fish = rand.Next(100);
            personaje.Cheat = rand.Next(50);
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
