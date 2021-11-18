using UnityEngine;

public class SmoothCameraMove : MonoBehaviour
{
    #region Fields
    public GameObject player;
    #endregion
    void LateUpdate()
    {
        if (player != null)
        {
            transform.rotation = Quaternion.Euler(0.5f * player.transform.position.y, 
                0.25f * player.transform.position.x, 
                0.0f);
        }
    }
}
