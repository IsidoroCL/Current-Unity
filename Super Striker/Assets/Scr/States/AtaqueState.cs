using System.Collections.Generic;
using UnityEngine;

public class AtaqueState : IState
{
    PartidoManager partidoManager;
    Jugador jugadorSelected;
    int jugadoresMovidos;
    List<Hex> casillas;
    Accion accion;
    public AtaqueState(PartidoManager pm, Accion accion)
    {
        partidoManager = pm;
        casillas = new List<Hex>();
        this.accion = accion;
    }
    public void Enter()
    {
        Debug.Log("ATAQUE");
        partidoManager.Numero_turno++;
        partidoManager.ComprobarJugadorCercano(partidoManager.balon.casilla);
        
        if (partidoManager.balon.Jugador == null) { Debug.Log("***BALON SIN JUGADOR***"); }
        if (partidoManager.ultimoFutbolistaConBalon.equipo == 0)
        {
            foreach (Jugador jug2 in partidoManager.jugadoresNegro)
            {
                jug2.IsSelectable = true;
                jug2.transform.position = jug2.Casilla.transform.position;
            }
            /*foreach (Jugador jugContrario in partidoManager.jugadoresBlanco)
            {
                jugContrario.CasillasControladas();
            }*/
        }
        else
        {
            foreach (Jugador jug2 in partidoManager.jugadoresBlanco)
            {
                jug2.IsSelectable = true;
                jug2.transform.position = jug2.Casilla.transform.position;
            }
            /*foreach (Jugador jugContrario in partidoManager.jugadoresNegro)
            {
                jugContrario.CasillasControladas();
            }*/
        }
        if (accion == Accion.FALTA && partidoManager.balon.Jugador != null) partidoManager.balon.Jugador.IsSelectable = false;
        jugadoresMovidos = 0;        
    }

    public void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject selectedObject = partidoManager.SelectedObjectByMouse();
            if (selectedObject == null) return;
            if (selectedObject.GetComponent<Jugador>() &&
                selectedObject.GetComponent<Jugador>().IsSelectable &&
                selectedObject.GetComponent<Jugador>().IsActive)
            {
                jugadorSelected = selectedObject.GetComponent<Jugador>();
                partidoManager.LimpiarCasillas(casillas);
                casillas = jugadorSelected.casilla.EncontrarVariosVecinos(3);
                partidoManager.ActivarCasillas(casillas);
            }
            else if (selectedObject.GetComponent<Hex>() &&
                selectedObject.GetComponent<Hex>().activa &&
                jugadorSelected != null)
            {
                jugadorSelected.Casilla = selectedObject.GetComponent<Hex>();
                jugadorSelected.IsSelectable = false;
                jugadoresMovidos++;
                jugadorSelected = null;
                partidoManager.LimpiarCasillas(casillas);
            }
        }
        if (jugadoresMovidos > 2) partidoManager.SetState(new DefensaState(partidoManager, accion));
    }

    public void Exit()
    {
        foreach (Jugador jugador in partidoManager.jugadoresBlanco)
        {
            jugador.IsSelectable = false;
        }
        foreach (Jugador jugador in partidoManager.jugadoresNegro)
        {
            jugador.IsSelectable = false;
        }
        partidoManager.LimpiarCasillas();
    }

    public void ReceiveAction(Accion accion_)
    {
        if (accion_ == Accion.NADA) partidoManager.SetState(new DefensaState(partidoManager, accion));
    }
}
