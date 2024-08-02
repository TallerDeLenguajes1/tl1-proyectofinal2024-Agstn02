using Personajes;

List<Personaje> personajes = await FabricaDePersonajes.CrearListaPersonajes(3);


Console.Read();

Mesa mesa1 = new(personajes[1], personajes[2]);
// mesa1.Deck.Show();
Console.Read();
mesa1.PocketPairs();
mesa1.Jugador.MostrarStats();
mesa1.Computadora.MostrarStats();
Console.Read();
