using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WaveScenario : ScriptableObject
{
    #region Internal class
    [System.Serializable]
    class WaveAndTime
    {
        public Wave wave = default;
        public float timeInterval = 1f;
    }
    #endregion

    [SerializeField]
    WaveAndTime[] waves = { new WaveAndTime() };
    float contador;
    int i;

    public void Init()
    {
        contador = 0;
        i = 0;
    }

    public bool Progress()
    {
        if (i >= waves.Length) return true;
        contador += Time.deltaTime;
        if (contador >= waves[i].timeInterval)
        {
            contador -= waves[i].timeInterval;
            waves[i].wave.Activate();
            Debug.Log("Wave: " + i);
            i++;
        }
        return false;
    }
}
