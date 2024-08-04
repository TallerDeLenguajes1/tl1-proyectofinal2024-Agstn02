
using Cards;
using Personajes;


namespace Table{

public class Round{// La uso para alacenar las partidas
    public string BigBlinds {get; set;}
    public int[] Bets {get; set;}
    public Hand[] Hands {get; set;}
    public int Pot {get; set;}
}

public class Table(Personaje player, Personaje computer)
{
    private int button = 1;
    private Personaje pla = player;
    private Personaje com = computer ;
    private Deck deck = new Deck();
    private List<Round> roundList;
    //Porpiedades
    public Personaje Pla { get => pla;}
    public Personaje Com { get => com;}
    public Deck Deck { get => deck;}
    //Métodos.

    public void StartRound(){
        var round = new Round();
        


    }


}

}