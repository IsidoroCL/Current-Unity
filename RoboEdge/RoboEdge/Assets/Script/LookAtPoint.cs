using UnityEngine;

public class LookAtPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject point;

    void Update()
    {
        transform.LookAt(point.transform);
    }
}
