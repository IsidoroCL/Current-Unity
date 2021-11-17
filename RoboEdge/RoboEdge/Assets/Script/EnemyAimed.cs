using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimed : Enemy
{
    // Start is called before the first frame update
    float seconds;
    bool isCoroutinePlaying;
    private void Awake()
    {
        speed = 20.0f;
        life = 10;
        startTime = 10;
        repeatFire = 0;
        isCoroutinePlaying = false;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        Move();
    }

    protected override void Move()
    {
        if (transform.position.z < 10)
        {
            base.Move();
        }
        else
        {
            if (!isCoroutinePlaying)
            {
                //This code has to be execute only one time
                isCoroutinePlaying = true;
                StartCoroutine(MovingRandom());
            }
        }
    }

    protected override void Fire()
    {
        base.Fire();
    }

    IEnumerator MovingRandom()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(3);
            Vector3 newPosition = new Vector3(Random.Range(-7, 8), Random.Range(-4, 5), 10);
            while (transform.position != newPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
