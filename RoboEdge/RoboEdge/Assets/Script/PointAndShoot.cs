using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndShoot : MonoBehaviour
{
    public GameObject crosshairs;
    public GameObject player;
    public ObjectPooler pool;
    //public GameObject bulletStart;

    public float bulletSpeed = 60.0f;

    private Vector3 target;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        pool = GetComponent<ObjectPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        target = -Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        crosshairs.transform.position = new Vector3(target.x, target.y, crosshairs.transform.position.z);

        if (player != null)
        {
            Vector3 difference = crosshairs.transform.position - player.transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            if (Input.GetMouseButtonDown(0))
            {
                float distance = difference.magnitude;
                Vector3 direction = difference / distance;
                direction.Normalize();
                FireBullet(direction, rotationZ);
            }
        }
        
    }
    void FireBullet(Vector3 direction, float rotationZ)
    {
        GameObject b = pool.GetPooledObject() as GameObject;
        b.transform.position = player.transform.position;
        b.SetActive(true);
    }

}
