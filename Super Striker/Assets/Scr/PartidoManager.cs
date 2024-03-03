using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class PartidoManager : MonoBehaviour
{
    FiniteStateMachine stateMachine;

    //Esta clase gestiona toda la lógica del partido
    public static PartidoManager partidoManager;
    public static int[] marcador = { 0, 0};
    public bool saqueDeCentro;

    [Header("EQUIPOS")]
    public Equipo equipo1;
    public Equipo equipo2;

    //GameObjects que tiene que manejar para comunicarse
    public Balon balon;
    public List<Jugador> jugadoresBlanco;
    public List<Jugador> jugadoresNegro;
    public Map terreno;
    public Hex casillaCentral;

    public Jugador futbolista;
    public Jugador ultimoFutbolistaConBalon;

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
        CrearJugadores(equipo1, jugadoresNegro, true);
        CrearJugadores(equipo2, jugadoresBlanco, false);
        stateMachine.ChangeState(new InitState(this, casillaCentral, Accion.NULL));      
    }

    void Update()
    {
        stateMachine.Progress();
        if (balon.jugador != null) equipo_con_balon = balon.jugador.equipo; //VER SI SE PUEDE CAMBIAR
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
    private void CrearJugadores(Equipo equipo, List<Jugador> futbolistas, bool equipoIzquierda)
    {
        int i = 0;
        foreach(Jugador jugadorIniciar in futbolistas)
        {
            int posicionX = equipoIzquierda ? equipo.tactica.posiciones[i].x : 24 - equipo.tactica.posiciones[i].x;
            int equipoPartido = equipoIzquierda ? 0 : 1;
            jugadorIniciar.Init(equipo.plantilla[i], i+1, new Vector2Int(posicionX, equipo.tactica.posiciones[i].y), equipo.color1, equipo.color2, equipoPartido);
            i++;
        }
    }

    public void ColocarJugadorEnPosicionInicial()
    {
        foreach (Jugador jugador in jugadoresBlanco)
        {
            jugador.IsActive = true;
            jugador.Casilla = terreno.campo[jugador.posicionInicial.x, jugador.posicionInicial.y];
            terreno.campo[jugador.posicionInicial.x, jugador.posicionInicial.y].jugador = jugador;
            jugador.IsSelectable = true;
        }
        foreach (Jugador jugador in jugadoresNegro)
        {
            jugador.IsActive = true;
            jugador.Casilla = terreno.campo[jugador.posicionInicial.x, jugador.posicionInicial.y];
            terreno.campo[jugador.posicionInicial.x, jugador.posicionInicial.y].jugador = jugador;
            jugador.IsSelectable = true;
        }
    }

    public void LimpiarCasillas()
    {
        foreach (Hex cas in casillas)
        {
            cas.Desactivar();
            //cas.LimpiarJugadoresControla();
        }
        casillas.Clear();
    }

    public void LimpiarCasillas(List<Hex> casillas)
    {
        foreach (Hex cas in casillas)
        {
            cas.Desactivar();
            //cas.LimpiarJugadoresControla();
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
        balon.Jugador = null;
        balon.transform.position = casillaCentral.transform.position;
        SetState(new InitState(this, casillaCentral, Accion.NULL));

    }

    public void ComprobarJugadorCercano(Hex casilla)
    {
        List<Hex> vecinos_casilla = casilla.EncontrarVecinos();
        //Debug.Log("Comprueba jugadores");
        foreach (Hex cas in vecinos_casilla)
        {
            if (cas.jugador != null && 
                cas.jugador.IsActive &&
                balon.jugador != null &&
                cas.jugador.equipo != balon.jugador.equipo)
            {
                HacerRegate(balon.jugador, cas.jugador, casilla);
            }
            //else { Debug.Log("Negativo"); }
        }
    }

    public void HacerRegate(Jugador atacante, Jugador defensor, Hex cas)
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
            atacante.StopAllCoroutines();
            balon.casilla = cas;
            Falta(defensor);
        }
    }

    public void Robo(Jugador newJugadorConBalon)
    {
        Debug.Log("Jugador que ha robado: " + newJugadorConBalon.name);
        balon.PararBalon();
        balon.Jugador.StopAllCoroutines();
        balon.Jugador = newJugadorConBalon;
        balon.Jugador.tieneBalon = true;
        balon.pausarMovimientoBalon = false;
        balon.MoverBalon(newJugadorConBalon.Casilla);
        SetState(new AtaqueState(partidoManager, Accion.NULL));
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
                Destroy(defensor.gameObject);
            }
            else
            {
                defensor.tieneTarjeta = true;
            }
        }

        SetState(new FaltaState(this, balon.casilla));
    }
    
    public void DeselecteAll()
    {
        foreach (Jugador jugador in jugadoresBlanco) jugador.IsSelectable = false;
        foreach (Jugador jugador in jugadoresNegro) jugador.IsSelectable = false;
        /*foreach (Hex casilla in terreno.campo)
        {
            casilla.LimpiarJugadoresControla();
        }*/
    }
    public void SetState(IState newState)
    {
        stateMachine.ChangeState(newState);
    }

}
