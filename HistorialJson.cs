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
        using (StreamWriter sw = File.AppendText(ruta))
        {
            sw.WriteLine(ganadorJson);
        }	
    }
    public static void LeerHistorial(string ruta){
        if(!Existe(ruta)){
            Console.WriteLine("Historial de partidas vacío.");
        }
        else
        {
            var lista = JsonSerializer.Deserialize<List<Table>>(ruta);
            int index = 1;
            Console.WriteLine($"Historial de ganadores:");
            foreach(var item in lista){
                Console.WriteLine($"---------- {index} ----------");
                Console.WriteLine("Ganador:");
                item.Player.MostrarStats();
        }
        
        }
    }
}
}