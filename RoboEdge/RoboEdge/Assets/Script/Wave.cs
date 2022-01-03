using UnityEngine;

[CreateAssetMenu]
public class Wave : ScriptableObject
{
    #region Internal Class
    [System.Serializable]
    class WaveConfig
    {
        public Enemy prefab = default;
        public Vector3Int position;
    }
    #endregion
    [SerializeField]
    WaveConfig[] enemies = { new WaveConfig() };

    public void Activate()
    {
        for(int i = 0; i<enemies.Length; i++)
        {
            Instantiate(enemies[i].prefab, enemies[i].position, Quaternion.identity);
        }
    }
}
