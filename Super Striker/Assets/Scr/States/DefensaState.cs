using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensaState : IState
{
    PartidoManager partidoManager;
    Jugador jugadorSelected;
    int jugadoresMovidos;
    List<Hex> casillas;
    public DefensaState(PartidoManager pm)
    {
        partidoManager = pm;
        casillas = new List<Hex>();
    }
    public void Enter()
    {
        Debug.Log("DEFENSA");
        if (partidoManager.ultimoFutbolistaConBalon.equipo == 0)
        {
            foreach (Jugador jug2 in partidoManager.jugadoresBlanco)
            {
                jug2.IsSelectable = true;
            }
        }
        else
        {
            foreach (Jugador jug2 in partidoManager.jugadoresNegro)
            {
                jug2.IsSelectable = true;
            }
        }
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
                jugadoresMovidos++;
                jugadorSelected.isSelected = false;
                jugadorSelected = null;
                partidoManager.LimpiarCasillas(casillas);
            }
        }
        if (jugadoresMovidos > 2)
        {
            if (partidoManager.balon.jugador != null)
            {
                partidoManager.SetState(new AccionState(partidoManager));
            }
            else
            {
                partidoManager.SetState(new AtaqueState(partidoManager));
            }
        }
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
        partidoManager.LimpiarCasillas(casillas);
    }

    public void ReceiveAction(Accion accion)
    {
        if (accion == Accion.NADA)
        {
            if (partidoManager.balon.jugador != null)
            {
                partidoManager.SetState(new AccionState(partidoManager));
            }
            else
            {
                partidoManager.SetState(new AtaqueState(partidoManager));
            }
        }
    }
}
