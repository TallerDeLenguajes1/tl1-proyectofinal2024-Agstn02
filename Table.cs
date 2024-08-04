
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
        //una vez iniciado el round:
        //1. Reaprtir las cartas a cada jugador

        //2. Ronda de apuestas.

        //3. Repartir el resto de cartas, pero solo mostrar el flop
        
        //4. Ronda de apuestas.

        //5. Mostrar el turn.

        //6.Rona de apuestas.

        //7.Muestro el river.

        //8.Ronda de apuestas.

        //9.Mostrar ganador.       
        
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
            var values1 = hand1.GetValuesArray();
            var values2 = hand2.GetValuesArray();
            for (int i = 0; i < values1.Length; i++)//Itero el máximo numero de veces posible. Si alguna condicion se cumple la funcion retorna un valor y no habra ningun Null reference
            {
                if(values1[i] != values2[i]){
                    if(values1[i] < values2[i])
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