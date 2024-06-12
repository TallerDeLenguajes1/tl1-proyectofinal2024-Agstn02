

//Jugador - A priori no habrán grandes cambios. 
public class Personaje{
    private string nombre;
    private string edad;
    private bool esJugable;
    private int suerte;
    private int aura;
    private int fish;
    private int cheat;

    public string Nombre { get => nombre; set => nombre = value; }
    public string Edad { get => edad; set => edad = value; }
    public bool EsJugable { get => esJugable; set => esJugable = value; }
    public int Suerte { get => suerte; set => suerte = value; }
    public int Aura { get => aura; set => aura = value; }
    public int Fish { get => fish; set => fish = value; }
    public int Cheat { get => cheat; set => cheat = value; }

    //Todo: Implementar una habilidad, de una lista de habilidades, como metodos en cada instancia. Implementarlo a través de ExtensionMethods
}