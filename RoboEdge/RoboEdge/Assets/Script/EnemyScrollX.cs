using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScrollX : Enemy
{
    private void Awake()
    {
        speed = 5.0f;
        life = 3;
        startTime = 3;
        repeatFire = 0.8f + Random.Range(0.0f, 1.0f);
    }
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

    protected override void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (transform.position.x > 50)
        {
            Destroy(gameObject);
        }
    }
}
