using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject cross;
    public float speed;
    private Vector3 direction;
    Rigidbody r;

    [SerializeField]
    private string target;
    private void Awake()
    {
        speed = 60.0f;
        r = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (cross == null)
        {
            cross = GameObject.FindGameObjectWithTag(target);
        }
        if (cross != null)
        {
            r.velocity = Vector3.zero;
            Vector3 difference = cross.transform.position - transform.position;
            float distance = difference.magnitude;
            direction = difference / distance;
            direction = difference;
            direction.Normalize();
            r.velocity = direction * speed;
        }
    }
}
