using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    int x;
    int y;
    public Hex casilla;

    //Array de casillas para luego moverse en ellas
    List<Hex> casillas; 

    //El gameManager para obtener datos del estado del juego
    public GameObject gameManager;

    Map terreno;
    PartidoManager partidoManager;

    //Atributos de la carta
    //Luego hay que cambiar para que se asocie a la carta los valores
    public int regate;
    public int paseBajo;
    public int paseAlto;
    public int tiro;
    public int cabezazo;
    public int defensa;
    public int velocidad;
    public int control;
    public int resistencia;

    public int equipo; //NEGRO = 0; BLANCO = 1
    public int numero;

    public bool esPortero;

    //Testea si tiene o no la pelota el jugador, si es seleccionable y si ha sido ya usado en el turno
    public bool tieneBalon = false;
    public bool activo = false;
    public bool usable;

    public Hex Casilla
    {
        get { return casilla; }
        set
        {
            casilla = value;

            StopCoroutine("MoverJugador");
            StartCoroutine("MoverJugador", casilla);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        tieneBalon = false;
        activo = false;
        usable = true;
        partidoManager = gameManager.GetComponent<PartidoManager>();
        terreno = gameManager.GetComponent<Map>();
        casillas = new List<Hex>();

    }

    // Update is called once per frame
    void Update()
    {
        
        //transform.Translate(new Vector3(1 * Time.deltaTime, 0, 0));
    }

    public IEnumerator MoverJugador(Hex casilla_objetivo)
    {
        //Movimiento del jugador cuando está activo el turno
        //Debug.Log("Has pulsado");
        Vector3 destino = casilla_objetivo.transform.position;
        float speed = 5f;
        GetComponent<CircleCollider2D>().enabled = false;
        //Mover jugador a casilla seleccionada
        while (transform.position != destino)
        {
            Vector3 dir = destino - transform.position;
            Vector3 velocity = dir.normalized * speed * Time.deltaTime;

            // Make sure the velocity doesn't actually exceed the distance we want.
            velocity = Vector3.ClampMagnitude(velocity, dir.magnitude);

            transform.Translate(velocity);
            yield return null;
        }
        GetComponent<CircleCollider2D>().enabled = true;
        activo = false;
    }

    public int Tirada(int habilidad)
    {
        //Hace la tirada de la habilidad y devuelve los éxitos conseguidos
        int exitos = 0;
        for (int i = 1; i <= habilidad; i++)
        {
            int dado = Random.Range(1, 7);
            if (dado > 3) exitos++;
            if (dado == 6) i--; //Dado explosivo, se vuelve a tirar
        }
        return exitos;
    }

    public void Activar()
    {
        usable = true;

    }

    public void Desactivar()
    {
        usable = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "casilla")
        {
            casilla = collision.gameObject.GetComponent<Hex>();
            casilla.jugador = gameObject.GetComponent<Jugador>();
            transform.position = casilla.transform.position;
        }
    }


}
