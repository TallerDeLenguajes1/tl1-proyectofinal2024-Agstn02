
using System.Security.Cryptography;
using Cards;
using Personajes;


namespace Table{

public class Round{
    // Almacena datos de cada ronda y maneja los eventos
//Propiedades
    public int BigBlind {get; set;}
    public int[] Bets {get; set;}
    public Deck Deck {get;} = new Deck();
    public List<Personaje> Players {get; set;}
    public int Pot {get; set;}
//Metodos:
    public void BettingRound(){
        Pot = 0;
        int minRaise = BigBlind;
        int lastBet = 0;
        bool roundFinished = false;

        while(!roundFinished)
        {
            //Debo iterar entre los dos jugadores mientras no hayan foldeado o no se haya igualado las apuestas.
            for (int i = 0; i < Players.Count; i++)
            {
                Personaje player = Players[i];
                if(player.EsJugable)
                {
                    int action;
                    do
                    {    
                        _ = int.TryParse(Console.ReadLine(), out action);
                        switch (action)
                        {
                            case 1://Call.
                                lastBet = player.Call(minRaise);
                                Pot += lastBet;
                                //Mostrar que el jugador calleo
                            break;
                            case 2://Bet.
                                lastBet = player.Bet(minRaise * 2);
                                Pot += lastBet;
                                //Mostrar que el jugador apostó
                            break;
                            case 3://Check.
                                if(lastBet != 0 ){
                                    action = -1;//saco action de rango, no está permitido pasar si existe una apuesta. 
                                }
                                //mostrar que el jugador pasa
                            break;
                            case 4://Fold
                                player.Fold();
                                //mostrar que el jugador foldea
                            break;
                        }
                    } while (action < 1 || action > 5);
                }
                else
                {
                    player.NpcAlgoritm();
                }
                if (Players.Any(p => p.IsFolded || p.CurrentBet == Pot ))
                {
                    roundFinished = true;
                    break;
                }
            }
        }
    }

    public void PreFlop(){
        //0. Recolectar ciegas
            Players[0].PayBlind(BigBlind);
            Players[1].PayBlind(BigBlind);
        //1. Repartir las cartas a cada jugador
            Players[1].Hand = new Hand(Deck.DealPoket());
            Players[0].Hand = new Hand(Deck.DealPoket());
        //2. Ronda de apuestas.
            BettingRound();
    }

}

public class Table(Personaje player, Personaje computer)
{
    private Personaje pla = player;
    private Personaje com = computer;
    private List<Round> roundList;
    //Porpiedades
    public Personaje Pla { get => pla;}
    public Personaje Com { get => com;}
    //Métodos.

    public void StartRound(){
        var round = new Round();
        //una vez iniciado el round:
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