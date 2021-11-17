using UnityEngine;

public class DeactiveOnInvisible : MonoBehaviour
{
    private Vector3 distanceFromCamera;

    private void Update()
    {
        distanceFromCamera = Camera.main.transform.position - transform.position;
        if (distanceFromCamera.magnitude > 300) gameObject.SetActive(false);
    }
}
