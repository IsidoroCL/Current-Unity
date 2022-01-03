using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Fields
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int life;
    [SerializeField]
    protected float startFireTime;
    [SerializeField]
    protected float repeatFireTime;

    [SerializeField]
    protected GameObject explosion;
    [SerializeField]
    protected GameObject damaged;
    [SerializeField]
    protected ObjectPooler bulletPool;
    protected GameObject player;
    protected GameObject enemyTargeted;
    #endregion
    #region Unity methods
    void Start()
    {
        Initialize();
    }
    void Update()
    {
        Move();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            if (enemyTargeted != null) enemyTargeted.SetActive(true);
        }
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (other.transform.localScale.x > 1.6)
            {
                life -= 3;
            }
            else if (other.transform.localScale.x > 0.8)
            {
                life -= 2;
            }
            else
            {
                other.gameObject.SetActive(false);
                life -= 1;
            }
            CheckLife();
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
            if (enemyTargeted != null) enemyTargeted.SetActive(false);
        }
    }
    #endregion
    #region Methods
    protected virtual void Fire()
    {
        GameObject bullet = bulletPool.GetPooledObject() as GameObject;
        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.SetActive(true);
        }
    }

    protected virtual void Move()
    {
        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        if (transform.position.z < Camera.main.transform.position.z + 5)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyTargeted = transform.Find("Target").gameObject;
        enemyTargeted.SetActive(false);
        bulletPool = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPooler>();
        InvokeRepeating("Fire", startFireTime, repeatFireTime + Random.Range(-0.1f, 0.5f));
    }

    protected IEnumerator Explosion()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    protected void CheckLife()
    {
        Instantiate(damaged, transform.position, Quaternion.identity);
        if (life < 1)
        {
            StartCoroutine(Explosion());
        }
    }
    #endregion
}
