using UnityEngine;

public class DeactiveOnInvisible : MonoBehaviour
{
    private Vector3 distanceFromCamera;
    private void Update()
    {
        distanceFromCamera = Camera.main.transform.position - transform.position;
        if (distanceFromCamera.magnitude > 300)
        {
            transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            gameObject.SetActive(false);
        }
    }
}
