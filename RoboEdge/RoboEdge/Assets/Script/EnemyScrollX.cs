using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScrollX : Enemy
{
    #region Unity methods
    private void Awake()
    {
        speed = 5.0f;
        life = 2;
        startTime = 5;
        repeatFire = 2.8f + Random.Range(0.0f, 1.0f);
    }
    void Start()
    {
        Init();
    }
    void Update()
    {
        Move();
    }
    #endregion
    #region Methods
    protected override void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (transform.position.x > 20)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
