using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject point;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(point.transform);
    }
}
