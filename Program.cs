using Personajes;
using Cards;

List<Personaje> personajes = await FabricaDePersonajes.CrearListaPersonajes(3);

Deck newDeck = new();
await newDeck.Shuffle();

personajes[0].Hand = new Hand(newDeck.DealPoket());
personajes[1].Hand = new Hand(newDeck.DealPoket());

personajes[0].MostrarStats();
personajes[1].MostrarStats();

var table = newDeck.DealFlop();
personajes[0].Hand.GetCards(table);
personajes[1].Hand.GetCards(table);

personajes[0].Hand.DefineValue();
personajes[1].Hand.DefineValue();

personajes[0].MostrarStats();
personajes[1].MostrarStats();

Console.ReadLine();