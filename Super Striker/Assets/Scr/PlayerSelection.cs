using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public static PlayerSelection playerSelection;
    public static Jugador[] jugadoresBlancoSelected;
    public static Jugador[] jugadoresNegroSelected;
    private void Awake()
    {
        //Se destruye si ya existe una instancia de este tipo
        //Pattern: Singleton
        if (playerSelection != null)
        {
            Destroy(gameObject);
            return;
        }
        playerSelection = this;
        DontDestroyOnLoad(gameObject);
    }

}
