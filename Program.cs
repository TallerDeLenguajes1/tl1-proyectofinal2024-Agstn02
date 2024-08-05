using Personajes;
using GameItems;
using Historial;
using System.Text.Json;


//Implementar menú
int val = 0;
string rutaHistorial = Directory.GetCurrentDirectory() + "/historial.json";
string rutaPersonajes = Directory.GetCurrentDirectory() + "/personajes.json";
do{
    Console.WriteLine(" 1 - Jugar   | 2 - Leer Historial ");
    int.TryParse(Console.ReadLine(), out val);
if (val == 2)
{
    HistorialJson.LeerHistorial(rutaHistorial);
}
}while(val != 1);
//Crear los personajes
List<Personaje> personajes =[];

if(PersonajesJson.Existe(rutaPersonajes)){
    personajes = PersonajesJson.LeerPersonajes(rutaPersonajes);
}
else{
    personajes = await FabricaDePersonajes.CrearListaPersonajes(10);
    PersonajesJson.GuardarPersonajes(personajes, rutaPersonajes);
}

//Seleccionar Personaje
int index = 0;
foreach(var item in personajes){
    Console.WriteLine("---------------------------------------------");
    Console.WriteLine((index + 1) + ":");
    item.MostrarStats();
    Console.WriteLine("---------------------------------------------");
    index++;
}
Console.WriteLine("¿Que personaje elijes?");
int input = -1;
do{
    int.TryParse(Console.ReadLine(), out input);
}while(input < 1 && input > 10);
Personaje MiPersonaje = personajes[input-1];
personajes.RemoveAt(input-1);
//Convertir al resto de personajes a Npc
List<Npc> Rivales = [];
foreach (var item in personajes)
{
    Rivales.Add(new Npc(item));
}
//Creo una lista para almacenar todas las Partidas.
List<Table> tableList = [];
//gameloop
do{
    foreach (var item in Rivales)
    {
        Console.WriteLine($"---------- {MiPersonaje.Name} vs- {item.Name} ----------");
        var mesa = new Table(MiPersonaje, item , []);
        do
        {
           await mesa.PlayRound();
            mesa.Result();
            Console.WriteLine($"Ganador: {mesa.WinnersName}");
        } while (!mesa.PlayerWin || !mesa.PlayerDefeat);
        //Chequea si ganaste o no la partida.
        if(mesa.PlayerWin){
            //Almacena la partida en la lista
            tableList.Add(mesa);
            //Se te da un buff en tu aura, o en tus tells.Sumado a que tu bank se duplicó.
            var rand = new Random();
            int randVal = rand.Next(100);
            if(randVal < 50){
                MiPersonaje.Aura += 5;
            }
            else{
                MiPersonaje.Tells -= 5;
            }
        }
        if(mesa.PlayerDefeat){
            HistorialJson.GuardarGanador(tableList, Directory.GetCurrentDirectory());
            Console.WriteLine("Has perdido. El juego se autodestruirá en 3 segundos...");
            Thread.Sleep(3000);
            Environment.Exit(0);
        }
    }
    //Derrotaste a todos los rivales.
    Console.WriteLine("Felicidades!!");
    Console.WriteLine("Derrotaste a todos los rivales.");
    HistorialJson.GuardarGanador(tableList, Directory.GetCurrentDirectory());
    Console.WriteLine("Pulsa una tecla para cerrar el juego.");
    Console.Read();
    return;
}while(true);