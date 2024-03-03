using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaltaState : IState
{
    private PartidoManager partidoManager;
    private Hex casilla_saque;
    private List<Hex> casillas;
    private List<Jugador> jugadoresContrariosCercaBalon;
    private Jugador jugadorSelected;
    Jugador jugadorQueTeniaBalon;

    public FaltaState(PartidoManager partidoManager, Hex casilla_saque)
    {
        this.partidoManager = partidoManager;
        this.casilla_saque = casilla_saque;
        casillas = new List<Hex>();
        jugadoresContrariosCercaBalon = new List<Jugador>();
    }
    public void Enter()
    {
        partidoManager.LimpiarCasillas();
        partidoManager.DeselecteAll();
        partidoManager.canvasAccion.enabled = false;
        partidoManager.canvasControl.enabled = false;
        partidoManager.canvasControlCabeza.enabled = false;

        List<Hex> casillas_dosDistancia = casilla_saque.EncontrarVariosVecinos(2);
        foreach (Hex casillas_ in casillas_dosDistancia)
        {
            if (casillas_.jugador != null &&
                casillas_.jugador.equipo != partidoManager.ultimoFutbolistaConBalon.equipo)
            {
                casillas_.jugador.IsSelectable = true;
                jugadoresContrariosCercaBalon.Add(casillas_.jugador);
            }
        }

        //Quita al jugador del balón y lo aparta dos casillas
        if (partidoManager.balon.Jugador != null)
        {
            jugadorQueTeniaBalon = partidoManager.balon.Jugador;
            partidoManager.balon.Jugador.tieneBalon = false;
            partidoManager.balon.Jugador = null;
            List<Hex> casillasAdyacentes = casilla_saque.EncontrarVecinos();
            foreach(Hex cas in  casillasAdyacentes)
            {
                if (cas.jugador == null)
                {
                    jugadorQueTeniaBalon.transform.position = cas.transform.position;
                    Debug.Log("casilla encontrada");
                    break;
                }
            }
        }
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
                partidoManager.LimpiarCasillas(casillas);
                casillas = jugadorSelected.casilla.EncontrarVariosVecinos(3);
                List<Hex> casillasDosDistanciaBalon = partidoManager.balon.casilla.EncontrarVariosVecinos(2);
                foreach (Hex casillaCercaBalon in casillasDosDistanciaBalon)
                {
                    casillas.Remove(casillaCercaBalon);
                }
                partidoManager.ActivarCasillas(casillas);
            }
            else if (selectedObject.GetComponent<Hex>() &&
                    selectedObject.GetComponent<Hex>().activa &&
                    jugadorSelected != null)
            {
                jugadorSelected.Casilla = selectedObject.GetComponent<Hex>();
                jugadoresContrariosCercaBalon.Remove(jugadorSelected);
                jugadorSelected = null;
                partidoManager.LimpiarCasillas(casillas);
            }
            if (jugadoresContrariosCercaBalon.Count < 1) partidoManager.SetState(new InitState(partidoManager, partidoManager.balon.casilla, Accion.FALTA));
        }
    }

    public void Exit()
    {
        if (partidoManager.ultimoFutbolistaConBalon.equipo == 0)
        {
            foreach (Jugador jug2 in partidoManager.jugadoresNegro)
            {
                jug2.IsSelectable = true;
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
            }
            /*foreach (Jugador jugContrario in partidoManager.jugadoresNegro)
            {
                jugContrario.CasillasControladas();
            }*/
        }
    }

    public void ReceiveAction(Accion accion)
    {
        if (accion == Accion.NADA)
        {
            if (partidoManager.balon.Jugador != null)
            {
                partidoManager.SetState(new AtaqueState(partidoManager, Accion.NULL));
            }
            else
            {
                Debug.Log("Elige un jugador para realizar el saque");
            }
        }
    }
}