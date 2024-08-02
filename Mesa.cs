
using System.Dynamic;
using Cards;
using Personajes;

public class Mesa
{
    private int button;
    private Personaje jugador;
    private Personaje computadora;
    private Deck deck = new();
    //Porpiedades
    public Personaje Jugador { get => jugador; set => jugador = value; }
    public Personaje Computadora { get => computadora; set => computadora = value; }
    public Deck Deck { get => deck; }
    public int Button { get => button; set => button = value; }

    //Constructor.
    public Mesa(Personaje pla, Personaje com)
    {
        jugador = pla;
        computadora = com;
        Deck.Suffle();
    }    

    
    public class Jugadas(){
        
    }

}