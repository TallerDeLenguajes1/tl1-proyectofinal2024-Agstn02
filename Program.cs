using Cartas;


Baraja prueba = await Baraja.Shuffle();
Console.Write(prueba.Deck_id);
foreach (var item in prueba.Cards)
{
    Console.WriteLine(item.Code);
}

Console.WriteLine("agustin no dejes que temineeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");