
using System.Security.Cryptography;
using Cards;
using Personajes;


namespace GameItems{

public class Round(Personaje player, Npc computer){
    // Almacena datos de cada ronda y maneja los eventos
//Propiedades
    private Personaje pla = player;
    private Npc com = computer;
    private List<Card> table;
    private const int BigBlind = 50;
    public Deck Deck {get;} = new Deck();
    public int Pot {get; set;}
    public int Winner;
//Metodos:
    public void BettingRound(){
        Pot = 0;
        int minRaise = BigBlind;
        int lastBet = 0;
        bool roundFinished = false;
        int button = pla.IsBigBlind ? 0 : 1;

        while(!roundFinished)
        {
            //Debo iterar entre los dos jugadores mientras no hayan foldeado o no se haya igualado las apuestas.
                if(button % 2 == 0)
                {
                    int action;
                    do
                    {    
                        _ = int.TryParse(Console.ReadLine(), out action);
                        switch (action)
                        {
                            case 1://Call.
                                lastBet = pla.Call(minRaise);
                                Pot += lastBet;
                                //Mostrar que el jugador calleo
                            break;
                            case 2://Bet.
                                lastBet = pla.Bet(minRaise * 2);
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
                                pla.Fold();
                                //mostrar que el jugador foldea
                            break;
                        }
                    } while (action < 1 || action > 5);
                }
                else//Logica de NPC basado en su handValue
                {
                    if (com.HandStrenght < 48)// en manos debiles
                    {
                        if(com.CurrentBet < Pot)//Si el jugador hizo un raise
                        {
                            if(lastBet * 2 > com.Bank){
                                com.Fold();
                            }
                            else
                            {
                                int aux = com.Call(lastBet);//Iguala la apuesta
                                lastBet = aux;//LastBet se actualiza
                                Pot += aux;//Actualizo el pot
                            }
                        }
                    }
                    else if(com.HandStrenght < 99)//En manos regulares 
                    {
                        var rand = new Random();
                        double plus = rand.NextDouble() * 10;// Aleatoriza el juego.
                        if(com.HandStrenght + plus < 68)//Si tiene una buena mano regular.
                        {
                            if(com.CurrentBet < Pot)
                            {
                                int aux = com.Call(lastBet);//Iguala la apuesta
                                lastBet = aux;//LastBet se actualiza
                                Pot += aux;//Actualizo el pot
                            }
                            else
                            {
                             //El npc checks   
                            }
                        }
                        else if(com.HandStrenght + plus < 98)//Si tiene una muy buena mano regular
                        {
                            if(com.CurrentBet < Pot)
                            {
                                if(com.Aura < 80){
                                    int aux = com.Call(lastBet);//Iguala la apuesta
                                    lastBet = aux;//LastBet se actualiza
                                    Pot += aux;//Actualizo el pot
                                }
                                else
                                {
                                    int aux = com.Bet(lastBet);//Sube la apuesta
                                    lastBet = aux;//LastBet se actualiza
                                    Pot += aux;//Actualizo el pot
                                }
                            }
                            else
                            {
                                int aux = com.Bet(lastBet);//Sube la apuesta
                                lastBet = aux;//LastBet se actualiza
                                Pot += aux;//Actualizo el pot
                            }

                        }
                        else//Tiene una gran mano regular y muchas posibilidades de ganar.
                        {
                            int aux = com.AllIn();//Va allin - Tiene una buena mano
                            lastBet = aux;//LastBet se actualiza
                            Pot += aux;//Actualizo el pot
                        }
                    }
                    else//Tiene una mano excelente
                    {
                        var rand = new Random();
                        int val = rand.Next(0,30);//Aleatoriza las decisiones.
                        if(com.CurrentBet < Pot){
                            if(val < 10){
                                int aux = com.AllIn();//Va allin - Tiene una buena mano
                                lastBet = aux;//LastBet se actualiza
                                Pot += aux;//Actualizo el pot
                            }
                            else
                            {
                                int aux = com.Bet(lastBet);//Sube la apuesta.
                                lastBet = aux;//LastBet se actualiza
                                Pot += aux;//Actualizo el pot
                            }
                        }
                        else{

                            if(val < 10){
                                int aux = com.AllIn();//Va allin 
                                lastBet = aux;//LastBet se actualiza
                                Pot += aux;//Actualizo el pot
                            }
                            else if(val < 20)
                            {
                                int aux = com.Bet(lastBet);//Sube la apuesta.
                                lastBet = aux;//LastBet se actualiza
                                Pot += aux;//Actualizo el pot
                            }
                            else
                            {
                                //La cosa pasa.
                            }
                        }
                    }
                }
                button++;
                bool betClosed = com.CurrentBet == Pot && pla.CurrentBet == Pot;//Ambos jugadores realizaron sus apuestas.
                if (pla.IsFolded || com.IsFolded || betClosed)
                {
                    roundFinished = true;
                    break;
                }
            }
    }

    public void PreFlop(){
        //0. Recolectar ciegas
        if(pla.IsBigBlind){
            Pot = pla.PayBlind(BigBlind);
            Pot = com.PayBlind(BigBlind/2);
        }
        else
        {
            Pot = pla.PayBlind(BigBlind/2);
            Pot = com.PayBlind(BigBlind);
        }
        //1. Repartir las cartas a cada jugador
            pla.Hand = new Hand(Deck.DealPoket());
            com.Hand = new Hand(Deck.DealPoket());
        //2. Ronda de apuestas.
            BettingRound();
    }
    public void Flop(){
        if(pla.IsFolded || com.IsFolded) return;
        //Repartir las tres cartas a mesa.
        table = Deck.DealFlop();
        pla.Hand.GetCards(table);
        com.Hand.GetCards(table);
        //Mostrar tres cartas de la jugada.
        Console.WriteLine($"Cartas de mesa:");
        foreach (var item in table)
        {
            Console.WriteLine($"|{item.Code}|");
        }
        //Ronda de apuestas.
        if(pla.Bank > 0 && com.Bank > 0)//si ninguno fue allin
        {
            BettingRound();
        }
    }
    public void Turn(){
        if(pla.IsFolded || com.IsFolded) return;
        //Repartir una carta mas
        var turn = Deck.Deal();
        table.Add(turn);
        //Las añado a la mano
        pla.Hand.GetCard(turn);
        com.Hand.GetCard(turn);
        //Ronda de apuestas
        if(pla.Bank > 0 && com.Bank > 0)//si ninguno fue allin
        {
            BettingRound();
        }
    }
    public void River(){
        if(pla.IsFolded || com.IsFolded) return;
        //Repartir una carta mas
        var river = Deck.Deal();
        table.Add(river);
        //Las añado a la mano
        pla.Hand.GetCard(river);
        com.Hand.GetCard(river);
        //Ultima ronda de apuestas
        if(pla.Bank > 0 && com.Bank > 0)//si ninguno fue allin
        {
            BettingRound();
        }
        
    }
    public void DefineWinner()
    {
        pla.Hand.DefineValue();
        var winner = CompareHands(pla.Hand, com.Hand);// 1 player | -1 npc
        switch (winner)
        {
            case 1:
                pla.CashPot(Pot);
                Winner = 1;
                break;
            case -1:
                com.CashPot(Pot);
                Winner = -1;
                break;
            default://las manos son exactamente iguales - el pot se divide entre ambos
                pla.CashPot(Pot/2);
                com.CashPot(Pot/2);
                Winner = 0;
                break;
        }
        if(pla.IsBigBlind) pla.IsBigBlind = false;
        else pla.IsBigBlind = true;
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

public class Table(Personaje player, Npc computer)
{
    private Personaje player = player;
    private Npc comp = computer;
    private List<Round> roundList;
    public bool PlayerWin { get; set;} = false;
    public bool PlayerDefeat  { get ; set ;} = false;
    //Porpiedades
    //Métodos.
    public void PlayRound(){
        var round = new Round(player, comp);
        
        round.PreFlop();
        round.Flop();
        round.Turn();
        round.River();
        round.DefineWinner();
        roundList.Add(round);
    }
    public void Result(){
        if(player.Bank <= 0) PlayerDefeat = true;
        if(comp.Bank <= 0) PlayerWin = true;
    }
}

}