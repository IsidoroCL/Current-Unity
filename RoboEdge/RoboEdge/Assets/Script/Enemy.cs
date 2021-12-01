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
    protected float startTime;
    [SerializeField]
    protected float repeatFire;

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
        Init();
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
            Instantiate(damaged, transform.position, Quaternion.identity);
            if (life < 1)
            {
                StartCoroutine(Explosion());
            }
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
        GameObject b = bulletPool.GetPooledObject() as GameObject;
        if (b != null)
        {
            b.transform.position = transform.position;
            b.SetActive(true);
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

    protected virtual void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyTargeted = transform.Find("Target").gameObject;
        enemyTargeted.SetActive(false);
        bulletPool = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPooler>();
        InvokeRepeating("Fire", startTime, repeatFire + Random.Range(-0.1f, 0.5f));
    }

    protected IEnumerator Explosion()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
    #endregion
}
