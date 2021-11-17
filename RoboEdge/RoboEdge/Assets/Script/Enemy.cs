using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int life;
    [SerializeField]
    protected float startTime;
    [SerializeField]
    protected float repeatFire;

    public ObjectPooler bulletPool;
    protected GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);
            life--;
            if (life < 1)
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual void Fire()
    {
        GameObject b = bulletPool.GetPooledObject() as GameObject;
        if (b != null)
        {
            b.transform.position = transform.position;
            b.SetActive(true);
        }
    }

    protected virtual void Move()
    {
        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        if (transform.position.z < Camera.main.transform.position.z + 5)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bulletPool = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPooler>();
        InvokeRepeating("Fire", startTime, repeatFire);
    }
}
