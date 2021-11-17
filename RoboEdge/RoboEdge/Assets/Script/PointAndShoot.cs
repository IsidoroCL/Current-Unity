using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndShoot : MonoBehaviour
{
    public GameObject crosshairs;
    public GameObject player;
    public ObjectPooler pool;

    public float bulletSpeed = 60.0f;

    private Vector3 target;
    private Animator anim;
    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        pool = GetComponent<ObjectPooler>();
        anim = player.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        target = -Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        crosshairs.transform.position = new Vector3(target.x, target.y, crosshairs.transform.position.z);

        if (player != null)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                FireBullet();
            }
        }
        
    }
    void FireBullet()
    {
        anim.SetBool("Shooting", true);
        GameObject b = pool.GetPooledObject() as GameObject;
        b.transform.position = player.transform.position;
        b.SetActive(true);
    }

}
