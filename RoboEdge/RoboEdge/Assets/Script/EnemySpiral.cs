using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpiral : Enemy
{
    private float timeCounter;
    [SerializeField]
    private float radius;
    // Start is called before the first frame update
    private void Awake()
    {
        speed = 5.0f;
        life = 1;
        repeatFire = 1;
    }
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
        if (transform.position.z > 10)
        {
            base.Move();
        }
        
        timeCounter += Time.deltaTime;
        float x = Mathf.Cos(timeCounter);
        float y = Mathf.Sin(timeCounter);
        
        transform.position = new Vector3(x * radius, y * radius, transform.position.z);

    }
}
