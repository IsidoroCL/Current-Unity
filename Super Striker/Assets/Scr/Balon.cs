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

    private int equipo; //NEGRO = 0; BLANCO = 1

    //Variables para gestionar el movimiento del balón
    private bool eventoBalon;
    private int exitos_jugada;
    private bool stopMover;

    private Coroutine movBalon;

    // Start is called before the first frame update
    void Start()
    {
        eventoBalon = false;
        stopMover = false;
    }

    // Update is called once per frame
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
            equipo = jugador.equipo;
        }
    }

    public void jugadaBalon(Hex casilla_objetivo)
    {
        movBalon = StartCoroutine(Mover(casilla_objetivo));
        if (partidoManager.accion == PartidoManager.Accion.PASE_BAJO)
        {
            exitos_jugada = jugador.Tirada(jugador.paseBajo);
            jugador.tieneBalon = false;
        }
        else if (partidoManager.accion == PartidoManager.Accion.PASE_ALTO)
        {
            exitos_jugada = jugador.Tirada(jugador.paseAlto);
            ResolverPaseAlto(jugador.casilla);
            ResolverPaseAlto(casilla_objetivo);
            jugador.tieneBalon = false;
        }
        else if (partidoManager.accion == PartidoManager.Accion.TIRO)
        {
            if (partidoManager.tiroCabeza)
            {
                exitos_jugada = jugador.Tirada(jugador.cabezazo + 2 + jugador.casilla.bonus);
            }
            else
            {
                exitos_jugada = jugador.Tirada(jugador.tiro +2 +jugador.casilla.bonus);
            }
            
            jugador.tieneBalon = false;
        }
        else if (partidoManager.accion == PartidoManager.Accion.CABEZAZO)
        {
            exitos_jugada = jugador.Tirada(jugador.cabezazo);
            jugador.tieneBalon = false;
        }
        jugador = null;
        
    }

    public IEnumerator Mover(Hex casilla_objetivo)
    {
        //Movimiento del balon 
        Vector3 destino = casilla_objetivo.transform.position;
        float speed = 12f;

        //Mover balón a casilla seleccionada
        while (transform.position != destino)
        {
            Vector3 dir = destino - transform.position;
            Vector3 velocity = dir.normalized * speed * Time.deltaTime;

            // Make sure the velocity doesn't actually exceed the distance we want.
            velocity = Vector3.ClampMagnitude(velocity, dir.magnitude);

            transform.Translate(velocity);
            if (stopMover)
            {
                stopMover = false; // para permitir que otras corrutinas de mover sean posibles
                break;
            }
            yield return null;
            yield return new WaitUntil(() => eventoBalon == false); 
        }
        //Asignamos el nuevo jugador al balon y le hacemos poseedor de la pelota
        if (casilla_objetivo.jugador != null && transform.position == destino)
        {
            jugador = casilla_objetivo.jugador;
            jugador.tieneBalon = true;
        }
        //Pasar a Fase Control y permitir mover para otra instancia
        if (partidoManager.accion == PartidoManager.Accion.TIRO)
        {
            //GOL
            partidoManager.Gol();
        }
        partidoManager.FaseControl();
        
    }

    private void ResolverPaseAlto(Hex casilla_final)
    {
        //Primero se comprueba si a dos casillas de la casilla de inicio hay 
        //algún jugador del otro equipo
        //Si hay, entonces se hacen tiradas enfrentadas
        //Luego se comprueba lo mismo pero en la casilla final
        List<Hex> vecinos = casilla_final.EncontrarVariosVecinos(2);
        
        foreach (Hex casi in vecinos)
        {
            if (casi.jugador != null && equipo != casi.jugador.equipo && casi.jugador.usable)
            {
                Debug.Log("tiradas de pase y defensa");
                casi.ActivarRojo();
                eventoBalon = true;
                int exitos_defensa = casi.jugador.Tirada(casi.jugador.defensa);
                Debug.Log("Ataque: " + exitos_jugada);
                Debug.Log("Defensa: " + exitos_defensa);
                //Comprobamos si consigue quitarle la pelota
                if (exitos_jugada > exitos_defensa)
                {
                    //La jugada continua
                    eventoBalon = false;
                    casi.jugador.Desactivar();

                }
                else if (exitos_jugada < exitos_defensa)
                {
                    //El defensa consigue el balon y se detiene la corutina
                    StopAllCoroutines();
                    jugador = casi.jugador;
                    jugador.tieneBalon = true;
                    eventoBalon = false;
                    partidoManager.Robo();

                }
                else
                {
                    Rechace(casi.jugador);
                }
            }
        }
    }

    private void ResolverPaseBajo(Jugador jugRival)
    {
        eventoBalon = true;
        int exitos_defensa;
        if (jugRival.esPortero)
        {
            exitos_defensa = jugRival.Tirada(jugRival.defensa +2);
        }
        else
        {
            exitos_defensa = jugRival.Tirada(jugRival.defensa);
        }
        
        Debug.Log("Ataque: " + exitos_jugada);
        Debug.Log("Defensa: " + exitos_defensa);
        //Comprobamos si consigue quitarle la pelota
        if (exitos_jugada > exitos_defensa)
        {
            //La jugada continua
            eventoBalon = false;
            jugRival.Desactivar();

        }
        else if (exitos_jugada < exitos_defensa)
        {
            //El defensa consigue el balon y se detiene la corutina
            StopAllCoroutines();
            jugador = jugRival;
            jugador.tieneBalon = true;
            eventoBalon = false;
            partidoManager.Robo();

        }
        else
        {
            Rechace(jugRival);
        }
    }

    private void Rechace(Jugador jugRival)
    {
        
        Debug.Log("Balon rechazado");
        //Balon rechazado
        StopCoroutine(movBalon);
        eventoBalon = false;
        //Calcular nueva casilla a 2 casillas y mover el balon al azar
        List<Hex> casillas_rechace = jugRival.casilla.EncontrarVariosVecinos(2);
        partidoManager.accion = PartidoManager.Accion.NADA;
        Hex casilla_elegida = casillas_rechace[Random.Range(0, casillas_rechace.Count)];
        casilla_elegida.ActivarRojo();
        movBalon = StartCoroutine(Mover(casilla_elegida));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Jugador" && jugador == null)
        {
            
        }

        if (collision.gameObject.tag == "casilla")
        {
            casilla = collision.gameObject.GetComponent<Hex>();
            List<Hex> vecinos_casilla = casilla.encontrarVecinos();
            foreach (Hex cas in vecinos_casilla)
            {
                if (cas.jugador != null && jugador != null)
                {
                    //Si el balón lo tiene un jugador y pasamos cerca de otro
                    //Comprobar si son del mismo equipo o diferentes
                    if (cas.jugador.usable && cas.jugador.equipo != jugador.equipo)
                    {
                        //Tiradas de Regate y Defensa
                        Debug.Log("Tiradas de regate y defensa");
                        exitos_jugada = jugador.Tirada(jugador.regate);
                        int exitos_def = cas.jugador.Tirada(cas.jugador.defensa);
                        if (exitos_jugada > exitos_def)
                        {
                            cas.jugador.usable = false;
                        }
                        else if (exitos_jugada < exitos_def)
                        {
                            jugador = cas.jugador;
                            jugador.tieneBalon = true;
                            eventoBalon = false;
                            partidoManager.Robo();
                        }
                        else
                        {
                            //Implementar Falta
                        }
                    }
                } 
                else if (cas.jugador != null && jugador == null)
                {
                    //Equipo diferente
                    if (equipo != cas.jugador.equipo && cas.jugador.usable)
                    {
                        if (partidoManager.accion == PartidoManager.Accion.PASE_BAJO || partidoManager.accion == PartidoManager.Accion.TIRO 
                            || partidoManager.accion == PartidoManager.Accion.CABEZAZO)
                        {
                            ResolverPaseBajo(cas.jugador);
                        }

                    }
                    
                }
            }
           
        }
    }


}
