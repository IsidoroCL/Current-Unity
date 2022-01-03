using UnityEngine;

public class EnemySmall : Enemy
{
    #region Fields
    private static EnemySmall[,] arrayEnemy = new EnemySmall[20, 5];
    private Vector3 targetPosition;
    [SerializeField]
    private GameObject enemySmallPrefab;
    #endregion
    #region Unity methods
    void Awake()
    {
        speed = 25.0f;
        life = 1;
        startFireTime = 2;
        repeatFireTime = 4;
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
    protected override void Initialize()
    {
        base.Initialize();
        targetPosition = CalculatePosition();
        InvokeRepeating("Duplicate", 2.7f, 4.7f);
    }
    protected override void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    protected override void Fire()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject bulletOne = bulletPool.GetPooledObject() as GameObject;
            if (bulletOne != null)
            {
                bulletOne.transform.position = transform.position - new Vector3(0, 0, i);
                bulletOne.SetActive(true);
            }
        }
    }

    protected void Duplicate()
    {
        Instantiate(enemySmallPrefab, transform.position, Quaternion.identity);
    }

    protected Vector3 CalculatePosition()
    {
        int posX = -9;
        int posY = -2;
        bool hasPosition = false;
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (arrayEnemy[i, j] == null)
                {
                    arrayEnemy[i, j] = this;
                    posX = i - 9;
                    posY = j - 2;
                    hasPosition = true;
                    break;
                }
            }
            if (hasPosition) break;
        }
        if (!hasPosition) Destroy(gameObject);

        return new Vector3(posX, posY, 10);
    }
    #endregion
}
