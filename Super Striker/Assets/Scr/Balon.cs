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
    private bool stopMover;
    private CircleCollider2D balonCollider;
    private bool nuevaCasilla;
    public AccionState accionState;

    public Jugador Jugador
    {
        get { return jugador; }
        set {
            jugador = value;
        }
    }

    void Awake()
    {
        pausarMovimientoBalon = false;
        stopMover = false;
        balonCollider = GetComponent<CircleCollider2D>();
        balonCollider.enabled = false;
        nuevaCasilla = false;
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
        balonCollider.enabled = false;
    }

    public void ActivarCollider()
    {
        balonCollider.enabled = true;
    }

    private IEnumerator Mover(Hex casilla_objetivo)
    {
        casilla = casilla_objetivo;
        if (jugador != null) jugador.tieneBalon = false;
        //Movimiento del balon 
        Vector3 destino = casilla_objetivo.transform.position;
        float speed = 12f;

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
        
        balonCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "casilla")
        {
            casilla = collision.gameObject.GetComponent<Hex>();
            accionState.ComprobarJugadorCercano(casilla);
        }
    }

}
