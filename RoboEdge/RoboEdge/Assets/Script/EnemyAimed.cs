using System.Collections;
using UnityEngine;

public class EnemyAimed : Enemy
{
    #region Fields
    private bool isCoroutinePlaying;
    #endregion
    #region Unity methods
    private void Awake()
    {
        speed = 20.0f;
        life = 20;
        repeatFireTime = 0;
        isCoroutinePlaying = false;
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
        if (transform.position.z > 10)
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

    protected override void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyTargeted = transform.Find("Target").gameObject;
        enemyTargeted.SetActive(false);
        bulletPool = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPooler>();
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
    #endregion
}
