
using System.Dynamic;
using Cartas;
using Personajes;

public class Mesa
{
    private int button;
    private Personaje jugador;
    private Personaje computadora;
    private Baraja deck = new();
    //Porpiedades
    public Personaje Jugador { get => jugador; set => jugador = value; }
    public Personaje Computadora { get => computadora; set => computadora = value; }
    public Baraja Deck { get => deck; set => deck = value; }
    public int Button { get => button; set => button = value; }

    //Constructor.
    public Mesa(Personaje pla, Personaje com)
    {
        jugador = pla;
        computadora = com;
        Deck.Suffle();
    }
    
    //Metodo para repartir las dos primeras cartas a cada jugador 
    public void PocketPairs(){
        //Creo los dos Pockets
        PocketCards buttonPoket = new(); 
        PocketCards blindPoket = new();
        buttonPoket.Card1 = Deck.Cards[0]; 
        buttonPoket.Card2 = Deck.Cards[2]; 
        blindPoket.Card1 = Deck.Cards[1]; 
        blindPoket.Card2 = Deck.Cards[3]; 
        //Si el boton tiene un valor impar entonces está en el Jugador.
        if(Button % 2 != 0 ){
            Jugador.Pocket = buttonPoket;
            Computadora.Pocket = blindPoket;
        }
        else{
            Jugador.Pocket = blindPoket;
            Computadora.Pocket = buttonPoket;
        }
    }
}