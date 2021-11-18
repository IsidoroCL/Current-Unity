using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject cross;
    [SerializeField]
    private float speed;
    private Vector3 direction;
    private Rigidbody r;

    [SerializeField]
    private string target;
    #endregion
    #region Unity methods
    private void Awake()
    {
        speed = 60.0f;
        r = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (cross == null) cross = GameObject.FindGameObjectWithTag(target);
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
    #endregion
}
