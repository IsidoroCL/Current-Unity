using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitState : IState
{
    PartidoManager partidoManager;
    Jugador jugadorSelected;
    List<Hex> casillas;
    Hex casilla_saque;
    Accion accion;

    public InitState(PartidoManager pm, Hex casilla, Accion accion_)
    {
        partidoManager = pm;
        casilla_saque = casilla;
        accion = accion_;
    }
    public void Enter()
    {
        partidoManager.LimpiarCasillas();
        partidoManager.canvasAccion.enabled = false;
        partidoManager.canvasControl.enabled = false;
        partidoManager.canvasControlCabeza.enabled = false;

        if (partidoManager.saqueDeCentro)
        {
            partidoManager.ColocarJugadorEnPosicionInicial();
            if (partidoManager.balon.Jugador != null)
            {
                partidoManager.balon.Jugador.tieneBalon = false;
                partidoManager.balon.Jugador = null;
            }
            partidoManager.balon.transform.position = casilla_saque.transform.position;
        }

        //Se ejecuta cuando el balón sale fuera
        if (accion == Accion.FUERA_CAMPO && partidoManager.equipo_con_balon == 0) partidoManager.equipo_con_balon = 1;
        else if (accion == Accion.FUERA_CAMPO && partidoManager.equipo_con_balon == 1) partidoManager.equipo_con_balon = 0;

        partidoManager.balon.transform.position = casilla_saque.transform.position;
        partidoManager.balon.casilla = casilla_saque;
        Debug.Log("INIT");
        Debug.Log("Coloca a los jugadores: El primer jugador que elijas del equipo con balon, hará el saque");
    }

    public void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject selectedObject = partidoManager.SelectedObjectByMouse();
            if (selectedObject == null) return;
            if (selectedObject.GetComponent<Jugador>() &&
                selectedObject.GetComponent<Jugador>().IsSelectable)
            {
                jugadorSelected = selectedObject.GetComponent<Jugador>();
                casillas = partidoManager.casillaCentral.EncontrarVariosVecinos(25);
                if (jugadorSelected.equipo != partidoManager.equipo_con_balon)
                {
                    List<Hex> casillasDosDistanciaBalon = partidoManager.balon.casilla.EncontrarVariosVecinos(2);
                    foreach (Hex casillaCercaBalon in casillasDosDistanciaBalon)
                    {
                        casillas.Remove(casillaCercaBalon);
                    }
                }
                else
                {
                    if (partidoManager.balon.Jugador == null)
                    {
                        jugadorSelected.casilla.jugador = null;
                        jugadorSelected.transform.position = partidoManager.balon.transform.position;
                        jugadorSelected.casilla = partidoManager.balon.casilla;
                        jugadorSelected.tieneBalon = true;
                        partidoManager.balon.Jugador = jugadorSelected;
                        jugadorSelected = null;
                        if (accion == Accion.FALTA)
                        {
                            partidoManager.Invoke("DeselecteAll", 1);
                            partidoManager.SetState(new AtaqueState(partidoManager, Accion.FALTA));
                        }
                        
                        return;
                    }
                }
                if (partidoManager.saqueDeCentro)
                {
                    if (jugadorSelected.equipo == 0)
                    {
                        casillas.RemoveAll(casilla => casilla.x > 11); 
                    }
                    else
                    {
                        casillas.RemoveAll(casilla => casilla.x < 13);
                    }
                }
                partidoManager.ActivarCasillas(casillas);
            }
            else if (selectedObject.GetComponent<Hex>() &&
                selectedObject.GetComponent<Hex>().activa &&
                jugadorSelected != null)
            {
                jugadorSelected.Casilla = selectedObject.GetComponent<Hex>();
                jugadorSelected = null;
                partidoManager.LimpiarCasillas(casillas);
            }
        }
    }

    public void Exit()
    {
        partidoManager.saqueDeCentro = false;
        partidoManager.Invoke("DeselecteAll", 2);
        foreach (Jugador jugador in partidoManager.jugadoresBlanco)
        {
            jugador.GetComponent<CircleCollider2D>().enabled = true;
            jugador.casilla.jugador = jugador;
        }
        foreach (Jugador jugador in partidoManager.jugadoresNegro)
        {
            jugador.GetComponent<CircleCollider2D>().enabled = true;
            jugador.casilla.jugador = jugador;
        }
    }

    public void ReceiveAction(Accion accion)
    {
        this.accion = accion;
        if (accion == Accion.NADA)
        {
            if (partidoManager.balon.Jugador != null)
            {
                partidoManager.SetState(new AccionState(partidoManager));
            } 
            else
            {
                Debug.Log("Elige un jugador para realizar el saque");
            }
        }
    }

    public virtual void JugadaTerminadaConExito()
    {
        partidoManager.SetState(new AccionState(partidoManager));
    }
}
