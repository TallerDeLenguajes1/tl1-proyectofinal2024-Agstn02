
using Cards;
using Personajes;


namespace Table{

public class Round{
    // Almacena datos de cada ronda y maneja los eventos
//Propiedades
    public string BigBlinds {get; set;}
    public int[] Bets {get; set;}
    public Hand[] Hands {get; set;}
    public int Pot {get; set;}
//Metodos:
    public void PreFlop(){

    }

}

public class Table(Personaje player, Personaje computer)
{
    private Personaje pla = player;
    private Personaje com = computer ;
    private List<Round> roundList;
    //Porpiedades
    public Deck Deck {get;} = new Deck();
    public Personaje Pla { get => pla;}
    public Personaje Com { get => com;}
    //Métodos.

    public void StartRound(){
        var round = new Round();

        roundList.Add(round);
        
    }
    private int CompareHands(Hand hand1 , Hand hand2){
        if (hand1.Value > hand2.Value)//compara los valores del Tipo Handvalue
        {
            return 1;//Si hand1 > hand2 devuelve 1
        }
        else if(hand1.Value < hand2.Value)
        {
            return -1;//Si hand1 < hand2 devuelve -1
        }
        else
        {
            for (int i = 0; i < 7; i++)//Itero el máximo numero de veces posible. Si alguna condicion se cumple la funcion retorna un valor y no habra ningun Null reference
            {
                if(hand1.HigherValues[i].Key != hand2.HigherValues[i].Key){
                    if(hand1.HigherValues[i].Key < hand2.HigherValues[i].Key )
                    {
                        return -1;//Si hand1 < hand2 devuelve -1
                    }
                    else
                    {
                        return 1;//Si hand1 > hand2 devuelve 1
                    }
                }
            }
            return 0;//Si las manos son iguales devuelve 0
        }
    }


}

}