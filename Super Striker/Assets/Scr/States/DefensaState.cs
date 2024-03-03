using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensaState : IState
{
    PartidoManager partidoManager;
    Jugador jugadorSelected;
    int jugadoresMovidos;
    List<Hex> casillas;
    Accion accion;
    public DefensaState(PartidoManager pm, Accion accion)
    {
        partidoManager = pm;
        casillas = new List<Hex>();
        this.accion = accion;
    }
    public void Enter()
    {
        Debug.Log("DEFENSA");
        if (partidoManager.ultimoFutbolistaConBalon.equipo == 0)
        {
            foreach (Jugador jug2 in partidoManager.jugadoresBlanco)
            {
                jug2.IsSelectable = true;
                jug2.transform.position = jug2.Casilla.transform.position;
            }
        }
        else
        {
            foreach (Jugador jug2 in partidoManager.jugadoresNegro)
            {
                jug2.IsSelectable = true;
                jug2.transform.position = jug2.Casilla.transform.position;
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
                if (accion == Accion.FALTA)
                {
                    List<Hex> casillasDosDistanciaBalon = partidoManager.balon.casilla.EncontrarVariosVecinos(2);
                    foreach (Hex casillaCercaBalon in casillasDosDistanciaBalon)
                    {
                        casillas.Remove(casillaCercaBalon);
                    }
                }
                partidoManager.ActivarCasillas(casillas);
            }
            else if (selectedObject.GetComponent<Hex>() &&
                selectedObject.GetComponent<Hex>().activa &&
                jugadorSelected != null)
            {
                jugadorSelected.Casilla = selectedObject.GetComponent<Hex>();
                jugadoresMovidos++;
                jugadorSelected.IsSelectable = false;
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
                partidoManager.SetState(new AtaqueState(partidoManager, Accion.NULL));
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
                partidoManager.SetState(new AtaqueState(partidoManager, Accion.NULL));
            }
        }
    }
}
