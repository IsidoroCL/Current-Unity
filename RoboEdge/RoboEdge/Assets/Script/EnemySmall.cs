using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmall : Enemy
{
    private Vector3 newPosition;
    public GameObject enemySmallPrefab;
    void Awake()
    {
        speed = 25.0f;
        life = 1;
        startTime = 2;
        repeatFire = 2;
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
        EnemySmall[] sameEnemyTypes = GameObject.FindObjectsOfType<EnemySmall>();
        int posX;
        int posY;
        
        posX = sameEnemyTypes.Length - 8;
        if (sameEnemyTypes.Length > 15 &&
            sameEnemyTypes.Length < 30)
        {
            posY = 2;
        }
        else if (sameEnemyTypes.Length > 30)
        {
            posY = -2;
        }
        else
        {
            posY = 0;
        }        
        newPosition = new Vector3(posX, posY, 10);
        InvokeRepeating("Duplicate", 4.7f, 4.7f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected override void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
    }

    protected override void Fire()
    {
        GameObject b = bulletPool.GetPooledObject() as GameObject;
        GameObject c = bulletPool.GetPooledObject() as GameObject;
        GameObject d = bulletPool.GetPooledObject() as GameObject;
        if (b != null)
        {
            b.transform.position = transform.position;
            b.SetActive(true);
        }
        if (c != null)
        {
            c.transform.position = transform.position - Vector3.forward;
            c.SetActive(true);
        }
        if (d != null)
        {
            d.transform.position = transform.position - Vector3.forward *2;
            d.SetActive(true);
        }

    }

    protected void Duplicate()
    {
        Instantiate(enemySmallPrefab);
    }
}
