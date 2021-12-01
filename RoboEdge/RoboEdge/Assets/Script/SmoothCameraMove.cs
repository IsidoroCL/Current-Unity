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
            transform.rotation = Quaternion.Euler(1f * player.transform.position.y, 
                0.5f * player.transform.position.x, 
                0.0f);
        }
    }
}
