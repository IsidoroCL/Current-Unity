using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionState : IState
{
    protected PartidoManager partidoManager;
    protected Accion accion;
    protected Jugador jugadorConBalon;
    protected Balon balon;
    protected List<Hex> casillas;
    protected int exitos_jugada;
    public AccionState(PartidoManager pm)
    {
        partidoManager = pm;
        casillas = new List<Hex>();
    }
    public virtual void Enter()
    {
        Debug.Log("ACCION");
        foreach (Jugador jgdr in partidoManager.jugadoresBlanco)
        {
            jgdr.IsActive = true;
        }
        foreach (Jugador jgdr in partidoManager.jugadoresNegro)
        {
            jgdr.IsActive = true;
        }
        partidoManager.canvasAccion.enabled = true;
        balon = partidoManager.balon;
        balon.accionState = this;
        jugadorConBalon = balon.Jugador;
        accion = Accion.NULL;
    }

    public virtual void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (accion == Accion.NULL) return;
            GameObject selectedObject = partidoManager.SelectedObjectByMouse();
            if (selectedObject == null) return;
            if (accion == Accion.PASE_BAJO ||
                accion == Accion.PASE_ALTO ||
                accion == Accion.CABEZAZO_PASE)
            {
                //Mueve la pelota a la casilla
                partidoManager.LimpiarCasillas(casillas);
                if (selectedObject.GetComponent<Jugador>() &&
                    selectedObject.GetComponent<Jugador>().IsSelectable &&
                    selectedObject.GetComponent<Jugador>().IsActive)
                {
                    JugadaBalon(selectedObject.GetComponent<Jugador>().Casilla);
                }
                else if (selectedObject.GetComponent<Hex>())
                {
                    accion = Accion.BALON_SUELTO;
                    JugadaBalon(selectedObject.GetComponent<Hex>());
                }              
            }
            else if (accion == Accion.CORRER)
            {
                partidoManager.LimpiarCasillas(casillas);
                if (jugadorConBalon.resistencia > 0 &&
                    selectedObject.GetComponent<Hex>() &&
                    //selectedObject.GetComponent<Hex>().activa &&
                    selectedObject.GetComponent<Hex>().jugador == null)
                {
                    jugadorConBalon.Casilla = selectedObject.GetComponent<Hex>();
                    jugadorConBalon.isSelected = false;
                    jugadorConBalon.resistencia--;
                    partidoManager.SetState(new AtaqueState(partidoManager));
                }
                else Debug.Log("El jugador no puede correr, está cansado");
            }
            else if (accion == Accion.MOVER)
            {
                partidoManager.LimpiarCasillas(casillas);
                if (selectedObject.GetComponent<Hex>() &&
                    //selectedObject.GetComponent<Hex>().activa &&
                    selectedObject.GetComponent<Hex>().jugador == null)
                {
                    jugadorConBalon.Casilla = selectedObject.GetComponent<Hex>();
                    jugadorConBalon.isSelected = false;
                    partidoManager.SetState(new AtaqueState(partidoManager));
                }
            }
            
        }
    }

    public virtual void Exit()
    {
        partidoManager.canvasAccion.enabled = false;
        foreach (Hex casilla in casillas)
        {
            if (casilla.jugador != null) casilla.jugador.IsSelectable = false;
        }
        partidoManager.LimpiarCasillas(casillas);
        partidoManager.DeselecteAll();
    }

    public virtual void ReceiveAction(Accion accion)
    {
        this.accion = accion;
        partidoManager.LimpiarCasillas(casillas);
        if (accion == Accion.NADA) partidoManager.SetState(new AtaqueState(partidoManager));
        else if (accion == Accion.PASE_BAJO) PaseBajo();
        else if (accion == Accion.PASE_ALTO) PaseAlto();
        else if (accion == Accion.TIRO) Tiro();
        else if (accion == Accion.CORRER) Correr();
    }

    protected void PaseBajo()
    {
        partidoManager.DeselecteAll();
        casillas = jugadorConBalon.casilla.EncontrarVariosVecinos(8);
        foreach(Hex casilla in casillas)
        {
            if (casilla.jugador != null) casilla.jugador.IsSelectable = true;
        }
        partidoManager.ActivarCasillas(casillas);
    }

    private void PaseAlto()
    {
        casillas = jugadorConBalon.casilla.EncontrarVariosVecinos(25);
        foreach (Hex casilla in casillas)
        {
            if (casilla.jugador != null) casilla.jugador.IsSelectable = true;
        }
        partidoManager.ActivarCasillas(casillas);
    }

    protected void Tiro()
    {
        //Casilla Porteria NEGRA 0,6
        //Casilla Porteria BLANCA 24,6
        Hex porteria = null;
        if (jugadorConBalon.equipo == 0) porteria = partidoManager.terreno.campo[24, 6];
        if (jugadorConBalon.equipo == 1) porteria = partidoManager.terreno.campo[0, 6];
        if (jugadorConBalon.casilla.puedeDisparar)
        {
            JugadaBalon(porteria);
        }
        else
        {
            Debug.Log("El jugador esta lejos de la porteria");
        }
    }

    private void Correr()
    {
        partidoManager.DeselecteAll();
        casillas = jugadorConBalon.casilla.EncontrarVariosVecinos(jugadorConBalon.velocidad);
        partidoManager.ActivarCasillas(casillas);
    }

    public void JugadaBalon(Hex casilla_objetivo)
    {
        balon.MoverBalon(casilla_objetivo);
        if (accion == Accion.PASE_BAJO)
        {
            exitos_jugada = jugadorConBalon.Tirada(jugadorConBalon.paseBajo);
            jugadorConBalon.tieneBalon = false;
            balon.ActivarCollider();
        }
        else if (accion == Accion.PASE_ALTO)
        {
            exitos_jugada = jugadorConBalon.Tirada(jugadorConBalon.paseAlto);
            if (ResolverPaseAlto(jugadorConBalon.casilla) &&
                ResolverPaseAlto(casilla_objetivo))
            {
                jugadorConBalon.tieneBalon = false;
            }
        }
        else if (accion == Accion.CABEZAZO_PASE)
        {
            exitos_jugada = jugadorConBalon.Tirada(jugadorConBalon.cabezazo);
            jugadorConBalon.tieneBalon = false;
            balon.ActivarCollider();
        }
        else if (accion == Accion.TIRO)
        {
            exitos_jugada = jugadorConBalon.Tirada(jugadorConBalon.tiro + 2 + jugadorConBalon.casilla.bonus);
            jugadorConBalon.tieneBalon = false;
            balon.ActivarCollider();
        }
        else if (accion == Accion.CABEZAZO_TIRO)
        {
            exitos_jugada = jugadorConBalon.Tirada(jugadorConBalon.cabezazo + jugadorConBalon.casilla.bonus);
            jugadorConBalon.tieneBalon = false;
            balon.ActivarCollider();
        }
        balon.Jugador = null;
    }

    private bool ResolverPaseAlto(Hex casilla_final)
    {
        //Primero se comprueba si a dos casillas de la casilla de inicio hay 
        //algún jugador del otro equipo
        //Si hay, entonces se hacen tiradas enfrentadas
        //Luego se comprueba lo mismo pero en la casilla final
        List<Hex> vecinos = casilla_final.EncontrarVariosVecinos(2);

        foreach (Hex casi in vecinos)
        {
            if (casi.jugador != null && 
                jugadorConBalon.equipo != casi.jugador.equipo && 
                casi.jugador.IsActive)
            {
                ResolverEnfrentamiento(casi.jugador);
            }
        }
        return true;
    }

    public void ComprobarJugadorCercano(Hex casilla)
    {
        List<Hex> vecinos_casilla = casilla.encontrarVecinos();
        foreach (Hex cas in vecinos_casilla)
        {
            if (cas.jugador != null &&
                jugadorConBalon.equipo != cas.jugador.equipo &&
                cas.jugador.IsActive)
            {
                ResolverEnfrentamiento(cas.jugador);
            }
        }
    }

    public bool ResolverEnfrentamiento(Jugador jugadorRival)
    {
        balon.pausarMovimientoBalon = true;
        int exitos_defensa;
        if (jugadorRival.esPortero)
        {
            exitos_defensa = jugadorRival.Tirada(jugadorRival.defensa + 2);
        }
        else
        {
            exitos_defensa = jugadorRival.Tirada(jugadorRival.defensa);
        }

        Debug.Log("Ataque: " + exitos_jugada);
        Debug.Log("Defensa: " + exitos_defensa);
        //Comprobamos si consigue quitarle la pelota
        if (exitos_jugada > exitos_defensa)
        {
            //La jugada continua
            balon.pausarMovimientoBalon = false;
            jugadorRival.Desactivar();
        }
        else if (exitos_jugada < exitos_defensa)
        {
            Robo(jugadorRival);
            return false;
        }
        else
        {
            Rechace(jugadorRival);
        }
        return true;
    }

    private void Robo(Jugador newJugadorConBalon)
    {
        Debug.Log("Jugador que ha robado: " + newJugadorConBalon.name);
        balon.PararBalon();
        balon.Jugador = newJugadorConBalon;
        balon.Jugador.tieneBalon = true;
        balon.pausarMovimientoBalon = false;
        partidoManager.SetState(new AtaqueState(partidoManager));
    }

    private void Rechace(Jugador jugRival)
    {
        Debug.Log("Balon rechazado");
        //Balon rechazado
        balon.PararBalon();
        balon.pausarMovimientoBalon = false;
        //Calcular nueva casilla a 2 casillas y mover el balon al azar
        List<Hex> casillas_rechace = jugRival.casilla.EncontrarVariosVecinos(2);
        Hex casilla_elegida = casillas_rechace[Random.Range(0, casillas_rechace.Count)];
        balon.MoverBalon(casilla_elegida);
        accion = Accion.BALON_SUELTO;
        JugadaTerminadaConExito();
    }

    public virtual void JugadaTerminadaConExito()
    {
        if (accion == Accion.TIRO)
        {
            partidoManager.Gol();
        }
        else
        {
            partidoManager.SetState(new ControlState(partidoManager, accion));
        }
        
    }
}
