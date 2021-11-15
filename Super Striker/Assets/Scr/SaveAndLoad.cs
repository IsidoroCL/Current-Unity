using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveAndLoad : MonoBehaviour
{
    public string nombre;
    public int regate;
    public int paseBajo;
    public int paseAlto;
    public int tiro;
    public int cabezazo;
    public int defensa;
    public int velocidad;
    public int control;

    private Jugador jugador;

    private void Start()
    {
        jugador = GetComponent<Jugador>();
        nombre = jugador.nombre;
        regate = jugador.regate;
        paseBajo = jugador.paseBajo;
        paseAlto = jugador.paseAlto;
        tiro = jugador.tiro;
        cabezazo = jugador.cabezazo;
        defensa = jugador.defensa;
        velocidad = jugador.velocidad;
        control = jugador.control;

        //Save();
    }
    public void Save()
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(Application.persistentDataPath + "/" + nombre + ".json", json);
        Debug.Log("Jugador guardado");
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveAndLoad data = JsonUtility.FromJson<SaveAndLoad>(json);
            nombre = data.nombre;
            regate = data.regate;
            paseBajo = data.paseBajo;
            paseAlto = data.paseAlto;
            tiro = data.tiro;
            cabezazo = data.cabezazo;
            defensa = data.defensa;
            velocidad = data.velocidad;
            control = data.control;

            jugador.nombre = nombre;
            jugador.regate = regate;
            jugador.paseBajo = paseBajo;
            jugador.paseAlto = paseAlto;
            jugador.tiro = tiro;
            jugador.cabezazo = cabezazo;
            jugador.defensa = defensa;
            jugador.velocidad = velocidad;
            jugador.control = control;
        }
    }
}
