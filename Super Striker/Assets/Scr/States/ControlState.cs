using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlState : AccionState
{
    //Variables necesarias para resolver el balón dividido
    List<Jugador> jugadoresCerca = new List<Jugador>();
    bool jugadoresEquiposDistintos;
    bool jugadorUnoElegido;
    Jugador jugadorUno;
    Jugador jugadorDos;
    public ControlState(PartidoManager pm, Accion accion) : base (pm)
    {
        this.accion = accion;
    }
    public override void Enter()
    {
        Debug.Log("CONTROL");
        Debug.Log("Accion: " + accion);
        balon = partidoManager.balon;
        balon.accionState = this;
        jugadoresEquiposDistintos = false;
        jugadorUnoElegido = false;
        switch (accion)
        {
            case Accion.PASE_BAJO:
                partidoManager.canvasControl.enabled = true;
                break;
            case Accion.PASE_ALTO:
                partidoManager.canvasControlCabeza.enabled = true;
                break;
            case Accion.BALON_SUELTO:
                BalonSuelto();
                break;
        }
        //accion = Accion.NULL;
        if (balon.jugador != null)
        {
            int exitos_control = balon.jugador.Tirada(balon.jugador.control);
            Debug.Log("Éxitos control: " + exitos_control);
            if (exitos_control < 2) partidoManager.SetState(new AtaqueState(partidoManager, Accion.NULL));
        }
    }

    public override void Execute()
    {
        base.Execute();

        if (Input.GetMouseButtonDown(0))
        {
            GameObject selectedObject = partidoManager.SelectedObjectByMouse();
            if (selectedObject == null) return;
            if (accion == Accion.BALON_SUELTO)
            {
                if (selectedObject.GetComponent<Jugador>() &&
                    selectedObject.GetComponent<Jugador>().IsSelectable &&
                    selectedObject.GetComponent<Jugador>().IsActive)
                {
                    if (!jugadoresEquiposDistintos)
                    {
                        selectedObject.GetComponent<Jugador>().Casilla = balon.casilla;
                        balon.jugador = selectedObject.GetComponent<Jugador>();
                        selectedObject.GetComponent<Jugador>().tieneBalon = true;
                        selectedObject.GetComponent<Jugador>().resistencia--;
                        JugadaTerminadaConExito();
                    }
                    else if(!jugadorUnoElegido)
                    {
                        jugadorUno = selectedObject.GetComponent<Jugador>();
                        jugadorUnoElegido = true;
                        foreach (Jugador jgdr in jugadoresCerca)
                        {
                            if (jgdr.equipo == 1) jgdr.IsSelectable = true;
                            if (jgdr.equipo == 0) jgdr.IsSelectable = false;
                        }
                    }
                    else
                    {
                        jugadorDos = selectedObject.GetComponent<Jugador>();
                        ResolverBalonSuelto();
                    }
                }
            }
        }
        
    }


    public override void Exit()
    {
        partidoManager.canvasControl.enabled = false;
        partidoManager.canvasControlCabeza.enabled = false;
        partidoManager.LimpiarCasillas(casillas);
    }

    public override void ReceiveAction(Accion accion)
    {
        this.accion = accion;
        partidoManager.LimpiarCasillas();
        partidoManager.LimpiarCasillas(casillas);
        if (accion == Accion.MOVER) MoverJugador();
        else if (accion == Accion.PASE_BAJO) PaseBajo();
        else if (accion == Accion.TIRO) base.Tiro();
        else if (accion == Accion.CABEZAZO_TIRO) base.Tiro();
        else if (accion == Accion.CABEZAZO_PASE) PaseDeCabeza();
    }

    private void PaseDeCabeza()
    {
        casillas = balon.jugador.casilla.EncontrarVariosVecinos(4);
        partidoManager.ActivarCasillas(casillas);
    }

    private void MoverJugador()
    {
        casillas = balon.jugador.casilla.EncontrarVariosVecinos(1);
        partidoManager.ActivarCasillas(casillas);
    }

    private void BalonSuelto()
    {
        Hex casillaBalon = balon.casilla;
        List<Hex> vecinos_casillaBalon = casillaBalon.EncontrarVariosVecinos(3);
        int blancos = 0;
        int negros = 0;
        //Buscamos a todos los jugadores a tres casillas de distancia del balón
        foreach (Hex cas in vecinos_casillaBalon)
        {
            if (cas.jugador != null)
            {
                if (cas.jugador.equipo == 0 &&
                    cas.jugador.resistencia > 0 &&
                    cas.jugador.PuedeAlcanzarElBalon(casillaBalon))
                {
                    negros++;
                    jugadoresCerca.Add(cas.jugador);
                }
                else if (cas.jugador.equipo == 1 &&
                    cas.jugador.resistencia > 0 &&
                    cas.jugador.PuedeAlcanzarElBalon(casillaBalon))
                {
                    blancos++;
                    jugadoresCerca.Add(cas.jugador);
                } 
            }
        }
        if (jugadoresCerca.Count == 0)
        {
            //Si no hay jugadores cerca, el balón se queda quieto.
            //Llamar Init
            Debug.Log("no hay jugadores cerca");
            Hex casilla_fuera;
            if (balon.casilla.y > 10)
            {
                casilla_fuera = partidoManager.terreno.campo[balon.casilla.x, 11];
            }
            else if (balon.casilla.y < 1)
            {
                casilla_fuera = partidoManager.terreno.campo[balon.casilla.x, 0];
            }
            else
            {
                partidoManager.SetState(new DefensaState(partidoManager, Accion.NULL));
                return;
            }
            partidoManager.SetState(new InitState(partidoManager, casilla_fuera, Accion.FUERA_CAMPO));
        }
        else
        {
            if (negros == 0 || blancos == 0)
            {
                foreach (Jugador jgdr in jugadoresCerca)
                {
                    jgdr.IsSelectable = true;
                }
                Debug.Log("Elige un jugador cercano para coger el balón (un equipo)");
                //Permitir al jugador elegir que quien se queda el balon
                //Modificar Execute
            }
            else
            {
                jugadoresEquiposDistintos = true;
                foreach (Jugador jgdr in jugadoresCerca)
                {
                    if (jgdr.equipo == 0) jgdr.IsSelectable = true;
                    Debug.Log("Elige un jugador cercano para coger el balón (dos equipos)");
                }
            }
        }
    }

    private void ResolverBalonSuelto()
    {
        int exitosUno = jugadorUno.Tirada(jugadorUno.velocidad);
        int exitosDos = jugadorDos.Tirada(jugadorDos.velocidad);
        if (exitosUno > exitosDos)
        {
            jugadorUno.Casilla = balon.casilla;
            balon.jugador = jugadorUno;
            jugadorUno.tieneBalon = true;
            jugadorUno.resistencia--;
            jugadorDos.resistencia--;
            JugadaTerminadaConExito();
        }
        else if (exitosDos > exitosUno)
        {
            jugadorDos.Casilla = balon.casilla;
            balon.jugador = jugadorDos;
            jugadorDos.tieneBalon = true;
            jugadorUno.resistencia--;
            jugadorDos.resistencia--;
            JugadaTerminadaConExito();
        }
        else
        {
            ResolverBalonSuelto();
        }
    }

    public override void JugadaTerminadaConExito()
    {
        partidoManager.SetState(new AtaqueState(partidoManager, Accion.NULL));
    }
}
