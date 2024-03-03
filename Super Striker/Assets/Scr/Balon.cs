using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balon : MonoBehaviour
{
    //Clase que modeliza el balon
    public Hex casilla;
    public Jugador jugador;
    public PartidoManager partidoManager;
    
    public float offSet;

    //Variables para gestionar el movimiento del balón
    public bool pausarMovimientoBalon;
    public float speed = 15f;
    private bool stopMover;
    public AccionState accionState;

    public Jugador Jugador
    {
        get { return jugador; }
        set {
            if (value == null)
            {
                partidoManager.ultimoFutbolistaConBalon = jugador;
            }
            else
            {
                partidoManager.ultimoFutbolistaConBalon = value;
            }
            jugador = value;
        }
    }

    void Awake()
    {
        pausarMovimientoBalon = false;
        stopMover = false;
    }

    void Update()
    {
        if (jugador !=null)
        {
            if (jugador.equipo == 0)
            {
                transform.position = new Vector3(jugador.transform.position.x + offSet, jugador.transform.position.y, jugador.transform.position.z);
            }
            if (jugador.equipo == 1)
            {
                transform.position = new Vector3(jugador.transform.position.x - offSet, jugador.transform.position.y, jugador.transform.position.z);
            }
            casilla = jugador.casilla;
            jugador.tieneBalon = true;
        }
    }

    public Coroutine MoverBalon(Hex casilla)
    {
        StopAllCoroutines();
        return StartCoroutine(Mover(casilla));
    }

    public void PararBalon()
    {
        StopAllCoroutines();
    }

    private IEnumerator Mover(Hex casilla_objetivo)
    {
        if (jugador != null) jugador.tieneBalon = false;
        //Movimiento del balon 
        Vector3 destino = casilla_objetivo.transform.position;

        //Mover balón a casilla seleccionada
        while (transform.position != destino)
        {
            Vector3 dir = destino - transform.position;
            Vector3 velocity = dir.normalized * speed * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, dir.magnitude);
            transform.Translate(velocity);
            if (stopMover)
            {
                stopMover = false; // para permitir que otras corrutinas de mover sean posibles
                break;
            }
            yield return null;
            yield return new WaitUntil(() => pausarMovimientoBalon == false); 
        }
        //Asignamos el nuevo jugador al balon y le hacemos poseedor de la pelota
        if (transform.position == destino)
        {
            casilla = casilla_objetivo;
            if (casilla_objetivo.jugador != null)
            {
                jugador = casilla_objetivo.jugador;
                jugador.tieneBalon = true;
            }
            if (accionState != null)
            {
                accionState.JugadaTerminadaConExito();
            }
        }
    }

}
