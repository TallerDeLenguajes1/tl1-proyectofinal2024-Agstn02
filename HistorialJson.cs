using System.Text.Json;
using Personajes;

public class HistorialJson{
    public bool Existe(string ruta){
        bool existe = File.Exists(ruta);
        return existe;
    }
    public void GuardarGanador(Personaje ganador, string ruta){ // TODO crear clase historial para determinar los resultados de cada partida.
        if(!Existe(ruta)){
            File.Create(ruta);
        }

        string ganadorJson = JsonSerializer.Serialize(ganador);
        File.WriteAllText(ruta, ganadorJson);
    }
    public List<Personaje> LeerHistorial(string ruta){
        if(!Existe(ruta)){
            return [];
        }
        var lista = JsonSerializer.Deserialize<List<Personaje>>(ruta);
        return lista;
    }
}