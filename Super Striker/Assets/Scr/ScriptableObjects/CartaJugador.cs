using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Carta Jugador", menuName = "Super Striker/Carta Jugador")]
public class CartaJugador : ScriptableObject
{
    public string nombre;
    public Sprite imagenCompleta;
    public Sprite imagenIcono;

    [Header("Atributos")]
    [Range(1, 5)]    public int regate;
    [Range(1, 5)]    public int paseBajo;
    [Range(1, 5)]    public int paseAlto;
    [Range(1, 5)]    public int tiro;
    [Range(1, 5)]    public int cabezazo;
    [Range(1, 5)]    public int defensa;
    [Range(1, 3)]    public int velocidad;
    [Range(1, 5)]    public int control;
    [Range(1, 3)]    public int resistencia;
}
