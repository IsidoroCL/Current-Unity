using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Jugador : MonoBehaviour
{
    public Hex casilla;

    //Array de casillas para luego moverse en ellas
    List<Hex> casillas; 

    //El gameManager para obtener datos del estado del juego
    Map terreno;
    PartidoManager partidoManager;

    //Atributos de la carta
    //Luego hay que cambiar para que se asocie a la carta los valores
    //Cada carta puede ser un Scriptable Object
    public CartaJugador jugadorBase;

    public string nombre;

    [Range(1,5)]
    public int regate;
    [Range(1, 5)]
    public int paseBajo;
    [Range(1, 5)]
    public int paseAlto;
    [Range(1, 5)]
    public int tiro;
    [Range(1, 5)]
    public int cabezazo;
    [Range(1, 5)]
    public int defensa;
    [Range(1, 3)]
    public int velocidad;
    [Range(1, 5)]
    public int control;
    [Range(1, 3)]
    public int resistencia;

    public int equipo; //NEGRO = 0; BLANCO = 1
    [Range(1, 7)]
    public int numero;
    public bool tieneTarjeta = false; 

    public bool esPortero;

    //Posici�n inicial
    public Vector2Int posicionInicial;

    //Testea si tiene o no la pelota el jugador, si es seleccionable y si ha sido ya usado en el turno
    public bool tieneBalon = false;
    public bool isSelectable = false;
    public bool isActive;
    public float speed = 10f;

    //Gestiona el movimiento casilla a casilla
    private bool nuevaCasilla;

    //Los gr�ficos del jugador
    [SerializeField] private SpriteRenderer colorPrincipal;
    [SerializeField] private SpriteRenderer colorSecundario;
    private TextMeshPro camiseta;
    [SerializeField] private GameObject aura;

    //PROPIEDADES
    public CartaJugador JugadorBase
    {
        get { return jugadorBase; }
        set 
        { 
            jugadorBase = value;
            nombre = jugadorBase.nombre;
            regate = jugadorBase.regate;
            paseBajo = jugadorBase.paseBajo;
            paseAlto = jugadorBase.paseAlto;
            tiro = jugadorBase.tiro;
            cabezazo = jugadorBase.cabezazo;
            defensa = jugadorBase.defensa;
            velocidad = jugadorBase.velocidad;
            control = jugadorBase.control;
            resistencia = jugadorBase.resistencia;
        }
    }
    
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

    public bool IsSelectable
    {
        get { return isSelectable; }
        set
        {
            isSelectable = value;
            if (isSelectable)
            {
                if (!isActive)
                {
                    isSelectable = false;
                    return;
                }
                aura.SetActive(true);
            }
            else
            {
                aura.SetActive(false);
            }
        }
    }

    public bool IsActive
    {
        get { return isActive; }
        set
        {
            isActive = value;
            if (isActive)
            {
                //Normal
                Color tmp = colorPrincipal.color;
                tmp.a = 1.0f;
                colorPrincipal.color = tmp;
            }
            else
            {
                //transparente
                Color tmp = colorPrincipal.color;
                tmp.a = 0.4f;
                colorPrincipal.color = tmp;
            }
        }
    }

    private void Awake()
    {
        camiseta = GetComponentInChildren<TextMeshPro>();
        partidoManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<PartidoManager>();
        terreno = GameObject.FindGameObjectWithTag("GameController").GetComponent<Map>();
        casillas = new List<Hex>();
    }
    
    public void Init(CartaJugador carta, int dorsal, Vector2Int posicion, Color color1, Color color2, int equipoPartido)
    {
        nuevaCasilla = false;
        tieneBalon = false;
        IsActive = true;
        IsSelectable = true;

        colorPrincipal.color = color1;
        colorSecundario.color = color2;
        camiseta.color = color2;
        numero = dorsal;
        camiseta.text = numero.ToString();
        posicionInicial = posicion;
        JugadorBase = carta;
        equipo = equipoPartido;

    }

    public IEnumerator MoverJugador(Hex casilla_objetivo)
    {
        //Movimiento del jugador cuando est� activo el turno
        Vector3 destino = new Vector3(casilla_objetivo.transform.position.x,
                                       casilla_objetivo.transform.position.y,
                                        -2);
        //Mover jugador a casilla seleccionada
        while (transform.position != destino)
        {
            Vector3 dir = destino - transform.position;
            Vector3 velocity = dir.normalized * speed * Time.deltaTime;

            // Make sure the velocity doesn't actually exceed the distance we want.
            velocity = Vector3.ClampMagnitude(velocity, dir.magnitude);

            transform.Translate(velocity);
            if (nuevaCasilla)
            {
                nuevaCasilla = false;
                yield return MoverJugador(casilla);  
            }
            yield return null;
        }
        if (casilla == partidoManager.balon.casilla &&
            partidoManager.balon.Jugador == null)
        {
            tieneBalon = true;
            partidoManager.balon.Jugador = this;
        }
    }

    public int Tirada(int habilidad)
    {
        //Hace la tirada de la habilidad y devuelve los �xitos conseguidos
        int exitos = 0;
        for (int i = 1; i <= habilidad; i++)
        {
            int dado = Random.Range(1, 7);
            if (dado > 3) exitos++;
            if (dado == 6) i--; //Dado explosivo, se vuelve a tirar
        }
        return exitos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "casilla")
        {
            casilla = collision.gameObject.GetComponent<Hex>();
            if (casilla.jugador == null)
            {
                casilla.jugador = gameObject.GetComponent<Jugador>();
            }
            nuevaCasilla = true;
            if (tieneBalon)
            {
                partidoManager.ComprobarJugadorCercano(casilla);
            }
        }
    }

    public bool PuedeAlcanzarElBalon(Hex casillaBalon)
    {
        List<Hex> casillasVelocidadLlegaJugador = casilla.EncontrarVariosVecinos(velocidad);
        foreach (Hex casilla_balon_si_no in casillasVelocidadLlegaJugador)
        {
            if (casilla_balon_si_no.x == casillaBalon.x &&
                casilla_balon_si_no.y == casillaBalon.y)
            {
                return true;
            }
        }
        return false;
    }
}
