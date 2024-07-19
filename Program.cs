using Cartas;
using Personajes;


Baraja prueba = await Baraja.Shuffle();
var fabrica = new FabricaDePersonajes();
List<Personaje> personajes = await FabricaDePersonajes.CrearPersonajes(3);
Console.Write(prueba.Deck_id);
foreach (var item in prueba.Cards)
{
    Console.WriteLine(item.Code);
}
personajes.ForEach(delegate(Personaje pj){
    pj.MostrarStats();
});