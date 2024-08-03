using Personajes;
using Cards;

List<Personaje> personajes = await FabricaDePersonajes.CrearListaPersonajes(3);

Deck newDeck = new();
await newDeck.Shuffle();

personajes[0].Hand = new Hand(newDeck.DealPoket());
personajes[1].Hand = new Hand(newDeck.DealPoket());

personajes[0].MostrarStats();
personajes[1].MostrarStats();

personajes[0].Hand.GetCards(newDeck.DealTable());
personajes[1].Hand.GetCards(newDeck.DealTable());

personajes[0].Hand.DefineValue();
personajes[1].Hand.DefineValue();

