
using UnityEngine;

public class SmoothCameraMove : MonoBehaviour
{
    public GameObject player;
    // Update is called once per frame
    void LateUpdate()
    {
        //X: 8 = 2 // Y: 4 = 2
        if (player != null)
        {
            transform.rotation = Quaternion.Euler(0.5f * player.transform.position.y, 0.25f * player.transform.position.x, 0.0f);
        }
        
    }
}
