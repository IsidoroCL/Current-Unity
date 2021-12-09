using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalState : IState
{
    PartidoManager partidoManager;

    public FinalState(PartidoManager pm)
    {
        partidoManager = pm;
    }
    public void Enter()
    {
        Debug.Log("FINAL DEL PARTIDO");
        Debug.Log("------------------");
        Debug.Log("Negros: " + PartidoManager.marcador[0] + " Blancos: " + PartidoManager.marcador[1]);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }

    public void ReceiveAction(Accion accion)
    {
        
    }
}
