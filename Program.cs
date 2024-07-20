using Cartas;
using Personajes;


Baraja prueba = new();
prueba.Suffle();
var fabrica = new FabricaDePersonajes();


List<Personaje> personajes = await FabricaDePersonajes.CrearListaPersonajes(3);
Console.Write(prueba.Deck_id);
foreach (var item in prueba.Cards)
{
    Console.WriteLine(item.Code);
}

personajes.ForEach(delegate(Personaje pj){
    pj.MostrarStats();
});
