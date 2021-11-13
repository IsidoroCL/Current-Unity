using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartidoManager : MonoBehaviour
{
    //Esta clase gestiona toda la lógica del partido
    public int[] marcador = { 0, 0};

    //GameObjects que tiene que manejar para comunicarse
    public Balon balon;
    public Jugador[] jugadoresBlanco;
    public Jugador[] jugadoresNegro;
    public Map terreno;
    public Jugador jugSeleccionado;

    //Objetos para gestionar el canvas
    public Canvas canvasAccion;
    public Canvas canvasControl;
    public Canvas canvasControlCabeza;

    //Array de casillas para luego moverse en ellas
    List<Hex> casillas;

    //Corrutina de movimiento del balón
    public Coroutine movBalon;

    //Control del partido
    public enum faseTurno { INICIO, ATAQUE, DEFENSA, ACCION, CONTROL}
    public faseTurno estadoTurno;
    public enum Accion { PASE_BAJO, PASE_ALTO, TIRO, CABEZAZO, CONTROL, CORRER, MOVER, NADA}
    public Accion accion;

    public bool tiroCabeza;
    private int jugadores_movidos;
    private int equipo_con_balon; //NEGRO = 0; BLANCO = 1; NINGUNO = 2;
    private bool primerControl;

    private int num_turno;
    public int turnoMax;

    // Start is called before the first frame update
    void Start()
    {
        //Establece el marcador del partido en 0-0 al inicio
        marcador[0] = 0;
        marcador[1] = 0;
        terreno = GetComponent<Map>();
        casillas = new List<Hex>();
        jugadores_movidos = 0;
        estadoTurno = faseTurno.INICIO;
        primerControl = true;
        tiroCabeza = false;
        canvasAccion.enabled = false;
        canvasControl.enabled = false;
        canvasControlCabeza.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        switch(estadoTurno)
        {
            case faseTurno.INICIO:
                //Se inicializa el juego
                limpiarCasillas();
                canvasAccion.enabled = false;
                canvasControl.enabled = false;
                canvasControlCabeza.enabled = false;
                primerControl = true;
                estadoTurno = faseTurno.ATAQUE;
                //Preparo la fase de Ataque
                //Comprueba que equipo tiene el balón y activa a sus jugadores
                Debug.Log("ATAQUE");
                foreach (Jugador jugador in jugadoresBlanco)
                {
                    if (jugador.tieneBalon) equipo_con_balon = 1;
                }
                foreach (Jugador jugador in jugadoresNegro)
                {
                    if (jugador.tieneBalon) equipo_con_balon = 0;
                }
                if (equipo_con_balon == 0)
                {
                    foreach (Jugador jug2 in jugadoresNegro)
                    {
                        jug2.activo = true;
                    }
                }
                else
                {
                    foreach (Jugador jug2 in jugadoresBlanco)
                    {
                        jug2.activo = true;
                    }
                }
                break;
            case faseTurno.ATAQUE:
                //Fase de Ataque
                               
                seleccionarObjetos();
                if (jugadores_movidos >= 3)
                {
                    estadoTurno = faseTurno.DEFENSA;
                    //Preparo fase de Defensa
                    jugadores_movidos = 0;
                    //Activa los jugadores del equipo defensor
                    Debug.Log("DEFENSA");
                    if (equipo_con_balon == 1)
                    {
                        foreach (Jugador jug2 in jugadoresNegro)
                        {
                            jug2.activo = true;
                        }
                    }
                    else
                    {
                        foreach (Jugador jug2 in jugadoresBlanco)
                        {
                            jug2.activo = true;
                        }
                    }
                }
                
                break;
            case faseTurno.DEFENSA:
                
                seleccionarObjetos();
                if (jugadores_movidos >= 3)
                {
                    estadoTurno = faseTurno.ACCION;
                    jugadores_movidos = 0;
                    canvasAccion.enabled = true;
                    Debug.Log("ACCION");
                }   
                break;
            case faseTurno.ACCION:
                //Muestra los botones de acción y permite hacer acciones
                seleccionarObjetos();
                break;
            case faseTurno.CONTROL:
                
                //Tirada de control, si ha habido pase.
                //Se usa primerControl para que haga la comprobación una única vez en el turno.
                if (primerControl)
                {
                    limpiarCasillas();
                    if (accion == Accion.PASE_BAJO && balon.jugador != null)
                    {
                        //Control
                        Control();
                    }
                    else if (accion == Accion.PASE_ALTO && balon.jugador != null)
                    {
                        //Cabezazo o control
                        canvasControlCabeza.enabled = true;
                    }
                    primerControl = false;
                }
                seleccionarObjetos();
                
                //Tiradas de enfrentamientos por jugadores próximos
                
                break;

        }
        
    }

    public void encontrarCasillas(Jugador jug, int dist)
    {
        //***********************************
        //DEPRECIATED
        //Se usa el método EncontrarVariosVecinos(int a) de Hex.cs
        //************************************

        //Cogemos el valor de X,Y de la casilla en la que se encuentra el jugador
        int x = jug.casilla.x;
        int y = jug.casilla.y;

        limpiarCasillas();
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

    public void limpiarCasillas()
    {
        foreach (Hex cas in casillas)
        {
            //Hacer casilla seleccionable
            cas.Desactivar();
        }
        casillas.Clear();
    }

    public void ActivarCasillas(List<Hex> c)
    {
        casillas = c;
        foreach (Hex cas in casillas)
        {
            //Comprobar que no hay otro jugador primero
            //Hacer casilla seleccionable
            cas.Activar();
        }
    }

    public void seleccionarObjetos()
    {
        
        
        if (Input.GetMouseButtonDown(0))
        {
            //Control del ratón mediante Raycast2D
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                //Pasamos a un gameObject el objeto sobre el que está el ratón para manipularlo
                GameObject objetivo = hit.collider.transform.gameObject;

                if (objetivo.GetComponent<Hex>())
                {
                    if (objetivo.GetComponent<Hex>().activa)
                    {
                         if (estadoTurno == faseTurno.ATAQUE || estadoTurno == faseTurno.DEFENSA)
                        {
                            //En Ataque o Defensa se selecciona la casilla para mover un jugador
                            if (jugSeleccionado != null)
                            {
                                //StartCoroutine(jugSeleccionado.MoverJugador(objetivo.GetComponent<Hex>()));
                                jugSeleccionado.Casilla = objetivo.GetComponent<Hex>();
                                jugadores_movidos++;
                                limpiarCasillas();
                                Debug.Log("Jugador movidos: " + jugadores_movidos);
                            
                            }
                        }
                        else if (estadoTurno == faseTurno.ACCION || estadoTurno == faseTurno.CONTROL)
                        {
                            if (accion == Accion.PASE_BAJO || accion == Accion.CABEZAZO)
                            {
                                //Mueve la pelota a la casilla
                                limpiarCasillas();
                                balon.jugadaBalon(objetivo.GetComponent<Hex>());
                                if (estadoTurno == faseTurno.CONTROL) estadoTurno = faseTurno.INICIO;
                            }
                            else if (accion == Accion.PASE_ALTO)
                            {
                                limpiarCasillas();
                                balon.jugadaBalon(objetivo.GetComponent<Hex>());
                            }
                            else if (accion == Accion.CORRER)
                            {
                                limpiarCasillas();
                                if (jugSeleccionado.resistencia > 0)
                                {
                                    //StartCoroutine(jugSeleccionado.MoverJugador(objetivo.GetComponent<Hex>()));
                                    jugSeleccionado.Casilla = objetivo.GetComponent<Hex>();
                                    jugSeleccionado.resistencia--;
                                    estadoTurno = faseTurno.CONTROL;
                                }
                                else Debug.Log("El jugador no puede correr, está cansado");
                            }
                            else if (accion == Accion.MOVER)
                            {
                                //StartCoroutine(jugSeleccionado.MoverJugador(objetivo.GetComponent<Hex>()));
                                jugSeleccionado.Casilla = objetivo.GetComponent<Hex>();
                                estadoTurno = faseTurno.INICIO;
                                accion = Accion.NADA;
                            }
                        }
                    }
                }
                else if (objetivo.GetComponent<Jugador>())
                {
                    if (estadoTurno == faseTurno.ATAQUE || estadoTurno == faseTurno.DEFENSA)
                    {
                        //En Ataque o Defensa se selecciona al jugador para ver las casillas a mover
                        
                        if (objetivo.GetComponent<Jugador>().activo == true)
                        {
                            jugSeleccionado = objetivo.GetComponent<Jugador>();
                            limpiarCasillas();
                            ActivarCasillas(jugSeleccionado.casilla.EncontrarVariosVecinos(3));
                        }
                    }
                    else if (estadoTurno == faseTurno.ACCION || estadoTurno == faseTurno.CONTROL)
                    {
                        if (accion == Accion.PASE_BAJO || accion == Accion.PASE_ALTO || accion == Accion.CABEZAZO)
                        {
                            balon.jugadaBalon(objetivo.GetComponent<Jugador>().casilla);
                        }
                        
                    }

                }
            }
        }      
        
    }

    public void Robo()
    {
        estadoTurno = faseTurno.INICIO;
    }

    public void FaseControl()
    {
        if (estadoTurno != faseTurno.CONTROL)
        {
            estadoTurno = faseTurno.CONTROL;
        }
        else
        {
            estadoTurno = faseTurno.INICIO;
        }
        canvasAccion.enabled = false;
    }

    public void Control()
    {
        int exito_control = balon.jugador.Tirada(balon.jugador.control);
        Debug.Log("Exitos control: " + exito_control);
        if (exito_control > 1)
        {
            //Aparece en pantalla las tres opciones
            Debug.Log("Control exitoso");
            canvasControl.enabled = true;
        }
        else
        {
            Debug.Log("Fallo en Control");
            estadoTurno = faseTurno.INICIO;
            accion = Accion.NADA;
        }
    }
    
    public void paseBajo()
    {
        limpiarCasillas();
        //Selecciono al jugador
        if (balon.jugador != null) jugSeleccionado = balon.jugador;
        //Buscar casilla y activar
        ActivarCasillas(jugSeleccionado.casilla.EncontrarVariosVecinos(8));
        accion = Accion.PASE_BAJO;
    }

    public void paseAlto()
    {
        limpiarCasillas();
        //Selecciono al jugador
        if (balon.jugador != null) jugSeleccionado = balon.jugador;
        //Buscar casilla y activar
        ActivarCasillas(jugSeleccionado.casilla.EncontrarVariosVecinos(25));
        accion = Accion.PASE_ALTO;
    }

    public void Tiro()
    {
        if (balon.jugador != null) jugSeleccionado = balon.jugador;
        accion = Accion.TIRO;
        //Casilla Porteria NEGRA 0,6
        //Casilla Porteria BLANCA 24,6
        Hex porteria = null;
        if (equipo_con_balon == 0) porteria = terreno.campo[24, 6];
        if (equipo_con_balon == 1) porteria = terreno.campo[0, 6];
        if (jugSeleccionado.casilla.puedeDisparar && Mathf.Abs(jugSeleccionado.casilla.x - porteria.x) < 10)
        {
            balon.jugadaBalon(porteria);
            canvasAccion.enabled = false;
            if (estadoTurno == faseTurno.CONTROL) estadoTurno = faseTurno.INICIO;
        }
        else
        {
            Debug.Log("El jugador esta lejos de la porteria");
        }
        
    }

    public void Cabezazo()
    {
        limpiarCasillas();
        //Selecciono al jugador
        if (balon.jugador != null) jugSeleccionado = balon.jugador;
        //Buscar casilla y activar
        ActivarCasillas(jugSeleccionado.casilla.EncontrarVariosVecinos(4));
        accion = Accion.CABEZAZO;
    }

    public void CabezazoTiro()
    {
        tiroCabeza = true;
        Tiro();
    }

    public void Correr()
    {
        accion = Accion.CORRER;
        if (balon.jugador != null) jugSeleccionado = balon.jugador;
        ActivarCasillas(jugSeleccionado.casilla.EncontrarVariosVecinos(jugSeleccionado.velocidad));
    }

    public void Pasar()
    {
        accion = Accion.NADA;
        estadoTurno = faseTurno.CONTROL;
    }

    public void ControlMover()
    {
        accion = Accion.MOVER;
        if (balon.jugador != null) jugSeleccionado = balon.jugador;
        ActivarCasillas(jugSeleccionado.casilla.EncontrarVariosVecinos(1));
    }

    public void Gol()
    {
        //Cambiar marcador
        marcador[equipo_con_balon]++;

        //Reinicia la posicion de los jugador

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

    }

}
