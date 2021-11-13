using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Hex : MonoBehaviour {

	// Coordenadas en el mapa
	public int x;
	public int y;

    //Si permite disparar o no la casilla
    public bool puedeDisparar;
    public short bonus;

	//Variables para almacenar jugadores y balon
	public Jugador jugador;
	public Balon balon;
    public Map terreno;

    //Variable para hacer seleccionable la casilla
    public bool activa;

    //Para recuperar el color original
    private Color color_inicial;

    private void Start()
    {

        terreno = FindObjectOfType<Map>();

        //Invisible
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        color_inicial = tmp;
        GetComponent<SpriteRenderer>().color = tmp;

        activa = false;
        puedeDisparar = false;
        bonus = 0;
        //Depende de la posición, puede o no disparar
        if (x == 0 || x == 24)
        {
            if (y == 4 || y == 8)
            {
                puedeDisparar = true;
                bonus = 0;
            }
            else if (y > 4 && y < 8)
            {
                puedeDisparar = true;
                bonus = 1;
            }
        }
        else if (x == 1 || x == 23)
        {
            if (y == 2 || y == 9)
            {
                puedeDisparar = true;
                bonus = -1;
            }
            else if (y == 3 || y == 4 || y == 7 || y == 8)
            {
                puedeDisparar = true;
                bonus = 0;
            }
            else if (y == 5 || y == 6)
            {
                puedeDisparar = true;
                bonus = 1;
            }
        }
        else if (x == 2 || x == 22)
        {
            if (y == 2 || y == 10)
            {
                puedeDisparar = true;
                bonus = -1;
            }
            else if (y == 3 || y == 4 || y == 5 || y == 7 || y == 8 || y == 9)
            {
                puedeDisparar = true;
                bonus = 0;
            }
            else if (y == 6)
            {
                puedeDisparar = true;
                bonus = 1;
            }
        }
        else if (x == 3 || x == 21)
        {
            if (y == 2 || y == 9)
            {
                puedeDisparar = true;
                bonus = -1;
            }
            else if (y > 2 && y < 9)
            {
                puedeDisparar = true;
                bonus = 0;
            }
        }
        else if (x == 4 || x == 20)
        {
            if (y == 2 || y == 3 || y == 4 || y == 8 || y == 9 || y == 10)
            {
                puedeDisparar = true;
                bonus = -1;
            }
            else if (y > 4 && y < 8)
            {
                puedeDisparar = true;
                bonus = 0;
            }
        }
        else if (x == 5 || x == 19)
        {
            if (y == 2 || y == 3 || y == 4 || y == 7 || y == 8 || y == 9)
            {
                puedeDisparar = true;
                bonus = -1;
            }
            else if (y > 4 && y < 7)
            {
                puedeDisparar = true;
                bonus = 0;
            }
        }
        else if (x == 6 || x == 18)
        {
            if (y > 4 && y < 8)
            {
                puedeDisparar = true;
                bonus = -1;
            }
        }
        else if (x == 7 || x == 17)
        {
            if (y > 4 && y < 7)
            {
                puedeDisparar = true;
                bonus = -1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Jugador")
        {
			//Vaciamos Jugador en caso de que el jugador salga de la casilla
			jugador = null;
        }
    }

    public void Activar()
    {
        if (jugador == null)
        {
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = 0.4f;
            GetComponent<SpriteRenderer>().color = tmp;
            activa = true;
        }
        
    }

    public void Desactivar()
    {
        GetComponent<SpriteRenderer>().color = color_inicial;
        activa = false;
    }

    public void ActivarRojo()
    {
            GetComponent<SpriteRenderer>().color = Color.red;
            activa = true;
    }

    public List<Hex> encontrarVecinos()
    {
        List<Hex> vecinos = new List<Hex>();
        
        if (x % 2 == 0)
        {
            if (x + 1 < 25) vecinos.Add(terreno.campo[x + 1, y]);
            if (x - 1 > -1) vecinos.Add(terreno.campo[x - 1, y]);
            if (y + 1 < 12) vecinos.Add(terreno.campo[x, y + 1]);
            if (y - 1 > -1) vecinos.Add(terreno.campo[x, y - 1]);
            if (x + 1 < 25 && y - 1 > -1) vecinos.Add(terreno.campo[x + 1, y - 1]);
            if (x - 1 > -1 && y - 1 > -1) vecinos.Add(terreno.campo[x - 1, y - 1]);
        }
        else
        {
            if (x + 1 < 25) vecinos.Add(terreno.campo[x + 1, y]);
            if (x - 1 > -1) vecinos.Add(terreno.campo[x - 1, y]);
            if (y + 1 < 12) vecinos.Add(terreno.campo[x, y + 1]);
            if (y - 1 > -1) vecinos.Add(terreno.campo[x, y - 1]);
            if (x + 1 < 25 && y + 1 < 12) vecinos.Add(terreno.campo[x + 1, y + 1]);
            if (y + 1 < 12 && x - 1 > -1) vecinos.Add(terreno.campo[x - 1, y + 1]);
        }
        return vecinos;
    }

    public List<Hex> EncontrarVariosVecinos(int dist)
    {
        //dist: casillas de distancia
        List<Hex> vecinos_inicial = this.encontrarVecinos();
        List<Hex> vecinos_temp = new List<Hex>();
        
        for (int i = 1; i < dist; i++)
        {
            foreach (Hex casilla in vecinos_inicial)
            {
                vecinos_temp.AddRange(casilla.encontrarVecinos());
            }
            vecinos_inicial.AddRange(vecinos_temp);
            //Quitamos los repetidos
            vecinos_inicial = vecinos_inicial.Distinct().ToList();
        }
        return vecinos_inicial;
    }

}
