using UnityEngine;

public class EnemyOblicous : Enemy
{
    enum Direction
    {
        Right, Left
    }
    #region Fields
    [SerializeField]
    Direction leftOrRight;
    [SerializeField]
    Vector3 direction;
    [SerializeField]
    float waitingTime;
    float timeSinceReachZ;
    #endregion
    #region Unity Methods
    void Start()
    {
        Initialize();
    }


    void Update()
    {
        Move();
    }
    #endregion
    #region Methods
    protected override void Initialize()
    {
        base.Initialize();
        if (leftOrRight == Direction.Left)
        {
            direction = new Vector3(-direction.x, direction.y, direction.z);
        }
    }
    protected override void Move()
    {
        if (transform.position.z > 10)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
        else
        {
            timeSinceReachZ += Time.deltaTime;
            if (timeSinceReachZ > waitingTime)
            {
                transform.Translate(new Vector3(0, 2, -4)
                    * speed * Time.deltaTime, Space.World);
            }
        }

        if (transform.position.z < Camera.main.transform.position.z) Destroy(gameObject);
    }
    #endregion
}
