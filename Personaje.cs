using ApiHelper;
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

    public string Name { get => name; set => name = value; }
    public string Edad { get => edad; set => edad = value; }
    public bool EsJugable { get => esJugable; set => esJugable = value; }
    public int Luck { get => luck; set => luck = value; }
    public int Aura { get => aura; set => aura = value; }
    public int Fish { get => fish; set => fish = value; }
    public int Cheat { get => cheat; set => cheat = value; }

    public void MostrarStats(){
        Console.WriteLine(Name);
        Console.WriteLine(Edad);
        Console.WriteLine(EsJugable);
        Console.WriteLine(Luck);
        Console.WriteLine(Aura);
        Console.WriteLine(Fish);
        Console.WriteLine(Cheat);
    }
    //Todo: Implementar una habilidad, de una lista de habilidades, como metodos en cada instancia. Implementarlo a través de ExtensionMethods
}
//Clase Fabrica de Personajes.
public class FabricaDePersonajes{
    public static async Task<List<Personaje>> CrearPersonajes(int n){
        //Ver la forma de que no me caguen las opciones del serializer 
        JsonSerializerOptions jsonOPt = new(){
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower//Fundamentalmente esta linea
            };
        var rand = new Random();
        var listaPersonajes = new List<Personaje>();
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
            personaje.Cheat = rand.Next(100);
            listaPersonajes.Add(personaje);
        }
        return listaPersonajes;
    }

}

}