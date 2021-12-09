using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitState : IState
{
    PartidoManager partidoManager;
    Jugador jugadorSelected;
    List<Hex> casillas;
    List<Jugador> jugadoresNegrosSinMover = new List<Jugador>();
    List<Jugador> jugadoresBlancosSinMover = new List<Jugador>();
    int equipo_con_balon; //0 = NEGRO, 1 = BLANCO
    Hex casilla_saque;
    Accion accion;
    bool fueraDelCampo = false;

    public InitState(PartidoManager pm, Hex casilla, bool fueraDelCampo)
    {
        partidoManager = pm;
        casilla_saque = casilla;
        this.fueraDelCampo = fueraDelCampo;
    }
    public void Enter()
    {
        partidoManager.LimpiarCasillas();
        partidoManager.DeselecteAll();
        partidoManager.canvasAccion.enabled = false;
        partidoManager.canvasControl.enabled = false;
        partidoManager.canvasControlCabeza.enabled = false;
        foreach (Jugador jugador in partidoManager.jugadoresBlanco)
        {
            jugador.IsActive = true;
            jugadoresBlancosSinMover.Add(jugador);
        }
        foreach (Jugador jugador in partidoManager.jugadoresNegro)
        {
            jugador.IsActive = true;
            jugadoresNegrosSinMover.Add(jugador);
        }
        equipo_con_balon = partidoManager.equipo_con_balon;
        if (fueraDelCampo && equipo_con_balon == 0) equipo_con_balon = 1;
        else if (fueraDelCampo && equipo_con_balon == 1) equipo_con_balon = 0;
        if (equipo_con_balon == 0)
        {
            foreach (Jugador jug2 in jugadoresBlancosSinMover)
            {
                jug2.IsSelectable = true;
            }
        }
        else
        {
            foreach (Jugador jug2 in jugadoresNegrosSinMover)
            {
                jug2.IsSelectable = true;
            }
        }
        if (partidoManager.balon.Jugador != null)
        {
            partidoManager.balon.Jugador.tieneBalon = false;
            List<Hex> casillasAdyacentes = casilla_saque.EncontrarVariosVecinos(2);
            foreach (Hex c in casillasAdyacentes)
            {
                if (c.jugador == null)
                {
                    partidoManager.balon.Jugador.transform.position = c.transform.position;
                    break;
                }
            }
            partidoManager.balon.Jugador = null;
        }
        partidoManager.balon.transform.position = casilla_saque.transform.position;
        partidoManager.balon.casilla = casilla_saque;
        Debug.Log("INIT");
        Debug.Log("Coloca a los jugadores");
        Debug.Log("El primer jugador que elijas del equipo con balon, hará el saque");
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
                if (jugadorSelected.equipo != equipo_con_balon)
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
                        jugadorSelected.GetComponent<CircleCollider2D>().enabled = false;
                        jugadorSelected.Casilla = partidoManager.balon.casilla;
                        jugadorSelected.tieneBalon = true;
                        
                        if (jugadorSelected.equipo == 0) jugadoresNegrosSinMover.Remove(jugadorSelected);
                        else jugadoresBlancosSinMover.Remove(jugadorSelected);
                        partidoManager.DeselecteAll();
                        ComprobarJugadoresMovidos(jugadorSelected);
                        jugadorSelected = null;
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
                jugadorSelected.GetComponent<CircleCollider2D>().enabled = false;
                jugadorSelected.Casilla = selectedObject.GetComponent<Hex>();
                jugadorSelected.isSelected = false;
                if (jugadorSelected.equipo == 0) jugadoresNegrosSinMover.Remove(jugadorSelected);
                else jugadoresBlancosSinMover.Remove(jugadorSelected);
                partidoManager.DeselecteAll();
                ComprobarJugadoresMovidos(jugadorSelected);
                jugadorSelected = null;
                partidoManager.LimpiarCasillas(casillas);
            }
        }
        
    }

    public void Exit()
    {
        partidoManager.saqueDeCentro = false;
        partidoManager.DeselecteAll();
        foreach (Jugador jugador in partidoManager.jugadoresBlanco)
        {
            jugador.GetComponent<CircleCollider2D>().enabled = true;
        }
        foreach (Jugador jugador in partidoManager.jugadoresNegro)
        {
            jugador.GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    public void ReceiveAction(Accion accion)
    {
        this.accion = accion;
    }

    private void ComprobarJugadoresMovidos(Jugador jugador)
    {
        if (jugadoresBlancosSinMover.Count == 0 &&
            jugadoresNegrosSinMover.Count == 0)
        {
            JugadaTerminadaConExito();
        }
        else
        {
            if (jugador.equipo == 0)
            {
                foreach (Jugador jug2 in jugadoresBlancosSinMover)
                {
                    jug2.IsSelectable = true;
                }
            }
            else
            {
                foreach (Jugador jug2 in jugadoresNegrosSinMover)
                {
                    jug2.IsSelectable = true;
                }
            }
        }
    }

    public virtual void JugadaTerminadaConExito()
    {
            partidoManager.SetState(new AccionState(partidoManager));
    }
}
