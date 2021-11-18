using UnityEngine;

public class EnemySmall : Enemy
{
    #region Fields
    private static EnemySmall[,] arrayEnemy = new EnemySmall[20,5];
    private Vector3 newPosition;
    [SerializeField]
    private GameObject enemySmallPrefab;
    #endregion
    #region Unity methods
    void Awake()
    {
        speed = 25.0f;
        life = 1;
        startTime = 2;
        repeatFire = 4;
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
    protected override void Init()
    {
        base.Init();

        //Put the enemy in the correct position
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

        newPosition = new Vector3(posX, posY, 10);
        InvokeRepeating("Duplicate", 4.7f, 4.7f);
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
        Instantiate(enemySmallPrefab, transform.position, Quaternion.identity);
    }
    #endregion
}
