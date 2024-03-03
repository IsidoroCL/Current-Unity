using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tactica", menuName = "Super Striker/Táctica")]
public class Tactica : ScriptableObject
{
    public string nombre;
    public Vector2Int[] posiciones = new Vector2Int[7];
}
