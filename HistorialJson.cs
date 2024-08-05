using System.Text.Json;
using GameItems;
using Personajes;
namespace Historial{

public class HistorialJson{
    public static bool Existe(string ruta){
        bool existe = File.Exists(ruta);
        return existe;
    }
    public static void GuardarGanador(List<Table> game, string ruta){ // TODO crear clase historial para determinar los resultados de cada partida.
        if(!Existe(ruta)){
            File.Create(ruta);
        }

        string ganadorJson = JsonSerializer.Serialize(game);
        File.WriteAllText(ruta, ganadorJson);
    }
    public static List<Personaje> LeerHistorial(string ruta){
        if(!Existe(ruta)){
            return [];
        }
        var lista = JsonSerializer.Deserialize<List<Personaje>>(ruta);
        return lista;
    }
}
}