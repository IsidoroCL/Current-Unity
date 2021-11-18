using UnityEngine;

public class LineTwoPoints : MonoBehaviour
{
    #region Fields
    private GameObject player;
    private GameObject cross;
    #endregion
    #region Unity methods
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cross = GameObject.FindGameObjectWithTag("Cross");
    }

    void Update()
    {
        if (player != null) transform.LookAt(cross.transform);
    }
    #endregion
}
