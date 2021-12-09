using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartidoManager : MonoBehaviour
{
    FiniteStateMachine stateMachine;

    //Esta clase gestiona toda la lógica del partido
    public static PartidoManager partidoManager;
    public static int[] marcador = { 0, 0};
    public bool saqueDeCentro;

    //GameObjects que tiene que manejar para comunicarse
    public Balon balon;
    public List<Jugador> jugadoresBlanco;
    public List<Jugador> jugadoresNegro;
    public Map terreno;
    public Hex casillaCentral;

    public Jugador futbolista;

    //Objetos para gestionar el canvas
    public Canvas canvasAccion;
    public Canvas canvasControl;
    public Canvas canvasControlCabeza;

    //Array de casillas para luego moverse en ellas
    List<Hex> casillas;

    //Corrutina de movimiento del balón
    public Coroutine movBalon;

    //Control del partido
    public Accion accion;
    public int equipo_con_balon; //NEGRO = 0; BLANCO = 1; NINGUNO = 2;

    private int num_turno;

    [SerializeField]
    private int turnoMax;

    public int Numero_turno
    {
        get { return num_turno; }
        set 
        { 
            num_turno = value;
            Debug.Log("Turno: " + Numero_turno);
            if (Numero_turno > turnoMax)
            {
                SetState(new FinalState(this));
            }
        }
    }

    private void Awake()
    {
        if (partidoManager != null)
        {
            Destroy(gameObject);
            return;
        }
        partidoManager = this;
        DontDestroyOnLoad(gameObject);
        stateMachine = new FiniteStateMachine();
        terreno = GetComponent<Map>();
        casillas = new List<Hex>();
        saqueDeCentro = true;
        marcador[0] = 0;
        marcador[1] = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        casillaCentral = terreno.CasillaCentral();
        //Crear Jugadores
        /*for (int i = 0; i < 7; i++)
        {
            Instantiate(futbolista, new Vector3())
        }*/
        stateMachine.ChangeState(new InitState(this, casillaCentral, false));      
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Progress();
        if (balon.jugador != null) equipo_con_balon = balon.jugador.equipo;
    }

    /*
    public void EncontrarCasillas(Jugador jug, int dist)
    {
        //***********************************
        //DEPRECIATED
        //Se usa el método EncontrarVariosVecinos(int a) de Hex.cs
        //************************************

        //Cogemos el valor de X,Y de la casilla en la que se encuentra el jugador
        int x = jug.casilla.x;
        int y = jug.casilla.y;

        LimpiarCasillas();
        //Encontrar casillas a 3 de distancia
        for (int i = 0; i < (dist +1); i++)
        {
            for (int j = 0; j < (dist +1); j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) < (dist +2))
                {
                    //Debug.Log("i = " + i + ", j = " + j);
                    //Hay problemas debido a que no es una cuadrícula cuadrado,
                    //sino que es una cuadrícula hexagonal.
                    //Se podría hacer un cambio de ejes, pero para este juego
                    //que usa un sistema sencillo de movimientos es más cómodo 
                    //quitar y añadir "manualmente" los valores según estemos
                    //en X par o impar.
                    //Si PAR x: +- 1, +3 eliminar y añadir -+3 -2
                    //Si IMPAR x: eliminar +-1, -3 y añadir -+3, +2 
                    if (i == 1 && j == dist)
                    {
                        if (x % 2 == 0)
                        {
                            //PAR
                            if (x + i < 25 && y - j > -1) casillas.Add(terreno.campo[x + i, y - j]);
                            if (x - i > -1 && y - j > -1) casillas.Add(terreno.campo[x - i, y - j]);

                            if (x - dist > -1 && y - 2 > -1) casillas.Add(terreno.campo[x - dist, y - 2]);
                            if (x + dist < 25 && y - 2 > -1) casillas.Add(terreno.campo[x + dist, y - 2]);
                        }
                        else
                        {
                            //IMPAR
                            if (x + i < 25 && y + j < 12) casillas.Add(terreno.campo[x + i, y + j]);
                            if (x - i > -1 && y + j < 12) casillas.Add(terreno.campo[x - i, y + j]);

                            if (x - dist > -1 && y + 2 < 12) casillas.Add(terreno.campo[x - dist, y + 2]);
                            if (x + dist < 25 && y + 2 < 12) casillas.Add(terreno.campo[x + dist, y + 2]);
                        }
                    }
                    else
                    {
                        if (x + i < 25 && y + j < 12) casillas.Add(terreno.campo[x + i, y + j]);
                        if (x - i > -1 && y - j > -1) casillas.Add(terreno.campo[x - i, y - j]);
                        if (x - i > -1 && y + j < 12) casillas.Add(terreno.campo[x - i, y + j]);
                        if (x + i < 25 && y - j > -1) casillas.Add(terreno.campo[x + i, y - j]);

                    }
                }
            }
        }

        foreach (Hex cas in casillas)
        {
            //Comprobar que no hay otro jugador primero
            //Hacer casilla seleccionable
            cas.Activar();
        }
    }
    */
    public void LimpiarCasillas()
    {
        foreach (Hex cas in casillas)
        {
            cas.Desactivar();
        }
        casillas.Clear();
    }

    public void LimpiarCasillas(List<Hex> casillas)
    {
        foreach (Hex cas in casillas)
        {
            cas.Desactivar();
        }
        casillas.Clear();
    }

    public void ActivarCasillas(List<Hex> casillas)
    {
        foreach (Hex cas in casillas)
        {
            cas.Activar();
        }
    }

    public GameObject SelectedObjectByMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log("Objeto pulsado: " + hit.collider.transform.gameObject.name);
            return hit.collider.transform.gameObject;
        }
        return null;
    }
    
    public void PaseBajo()
    {
        stateMachine.SendAction(Accion.PASE_BAJO);
    }

    public void PaseAlto()
    {
        stateMachine.SendAction(Accion.PASE_ALTO);        
    }

    public void Tiro()
    {
        stateMachine.SendAction(Accion.TIRO);
    }

    public void CabezazoPase()
    {
        stateMachine.SendAction(Accion.CABEZAZO_PASE);
    }

    public void CabezazoTiro()
    {
        stateMachine.SendAction(Accion.CABEZAZO_TIRO);
    }

    public void Correr()
    {
        stateMachine.SendAction(Accion.CORRER); 
    }

    public void PasarTurno()
    {
        stateMachine.SendAction(Accion.NADA);
        LimpiarCasillas();
        accion = Accion.NADA;
    }

    public void ControlMover()
    {
        stateMachine.SendAction(Accion.MOVER);
    }

    public void Gol()
    {
        //Cambiar marcador
        marcador[equipo_con_balon]++;
        //Balon en el centro, cambio de equipo
        if (equipo_con_balon == 0)
        {
            equipo_con_balon = 1;
        }
        else
        {
            equipo_con_balon = 0;
        }
        Debug.Log("Negro: " + marcador[0] + " Blanco: " + marcador[1]);
        saqueDeCentro = true;
        SetState(new InitState(this, casillaCentral, false));

    }

    public void ComprobarJugadorCercano(Hex casilla)
    {
        Jugador jugador = balon.jugador;
        List<Hex> vecinos_casilla = casilla.encontrarVecinos();
        foreach (Hex cas in vecinos_casilla)
        {
            if (cas.jugador != null &&
                cas.jugador.IsActive &&
                cas.jugador.equipo != jugador.equipo)
            {
                HacerRegate(jugador, cas.jugador);
            }
        }
    }

    public void HacerRegate(Jugador atacante, Jugador defensor)
    {
        int exitos_jugada = atacante.Tirada(atacante.regate);
        int exitos_defensa = defensor.Tirada(defensor.defensa);
        Debug.Log("Tiradas de regate y defensa: ATAQUE: " + exitos_jugada + " DEFENSA: " + exitos_defensa);
        if (exitos_jugada > exitos_defensa)
        {
            defensor.IsActive = false;
        }
        else if (exitos_jugada < exitos_defensa)
        {
            atacante.IsActive = false;
            atacante.tieneBalon = false;
            Robo(defensor);
        }
        else
        {
            Falta(defensor);
        }
    }

    private void Robo(Jugador newJugadorConBalon)
    {
        Debug.Log("Jugador que ha robado: " + newJugadorConBalon.name);
        balon.PararBalon();
        balon.Jugador = newJugadorConBalon;
        balon.Jugador.tieneBalon = true;
        balon.pausarMovimientoBalon = false;
        SetState(new AtaqueState(partidoManager));
    }

    public void Falta(Jugador defensor)
    {
        Debug.Log("Falta!!");
        //Comprobar si recibe tarjeta el defensor
        if (Random.Range(1, 7) < 5)
        {
            Debug.Log("Jugador recibe tarjeta amarilla");
            
            if (defensor.tieneTarjeta)
            {
                //Expulsar jugador
                if (defensor.equipo == 0)
                {
                    jugadoresNegro.Remove(defensor);
                }
                else
                {
                    jugadoresBlanco.Remove(defensor);
                }
                Debug.Log("Jugador Expulsado!!!!");
                Destroy(defensor);
            }
            else
            {
                defensor.tieneTarjeta = true;
            }
        }
        SetState(new InitState(this, balon.casilla, false));
    }
    
    public void DeselecteAll()
    {
        foreach (Jugador jugador in jugadoresBlanco) jugador.IsSelectable = false;
        foreach (Jugador jugador in jugadoresNegro) jugador.IsSelectable = false;
    }
    public void SetState(IState newState)
    {
        stateMachine.ChangeState(newState);
    }

}
