using UnityEngine;

public class EnemyScrollX : Enemy
{
    enum Direction
    {
        Up, Down, Right, Left
    }

    [SerializeField]
    Direction direction;
    #region Unity methods
    private void Awake()
    {
        speed = 5.0f;
        life = 1;
        startFireTime = 5;
        repeatFireTime = 2.8f + Random.Range(0.0f, 1.0f);
    }
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
    protected override void Move()
    {
        switch (direction)
        {
            case Direction.Up:
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                if (transform.position.y > 15)
                {
                    Destroy(gameObject);
                }
                break;
            case Direction.Down:
                transform.Translate(-Vector3.up * speed * Time.deltaTime);
                if (transform.position.y < -15)
                {
                    Destroy(gameObject);
                }
                break;
            case Direction.Right:
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (transform.position.x > 25)
                {
                    Destroy(gameObject);
                }
                break;
            case Direction.Left:
                transform.Translate(-Vector3.right * speed * Time.deltaTime);
                if (transform.position.x < -25)
                {
                    Destroy(gameObject);
                }
                break;
        }

    }
    #endregion
}
