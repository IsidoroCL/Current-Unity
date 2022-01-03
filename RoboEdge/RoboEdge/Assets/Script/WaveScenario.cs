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
    float timePassed;
    int i;

    public void Initialize()
    {
        timePassed = 0;
        i = 0;
    }

    public bool Progress()
    {
        if (i >= waves.Length) return true;
        timePassed += Time.deltaTime;
        if (timePassed >= waves[i].timeInterval)
        {
            timePassed -= waves[i].timeInterval;
            waves[i].wave.Activate();
            Debug.Log("Wave: " + i);
            i++;
        }
        return false;
    }
}
