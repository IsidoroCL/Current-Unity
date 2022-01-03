using UnityEngine;

public class Asteroid : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float maximumRotation = 90;
    private float speed;
    private int life;
    private Vector3 vectorRotationAsteroid;
    private Vector3 direction;
    private Rigidbody rigidbodyAsteroid;

    [SerializeField]
    private GameObject asteroid;
    [SerializeField]
    private GameObject damaged;
    [SerializeField]
    private GameObject explosion;
    #endregion
    #region Unity methods
    private void Awake()
    {
        rigidbodyAsteroid = GetComponent<Rigidbody>();
        speed = 5;
        direction = new Vector3(1, 0, -2);
        SetLife();
        SetRandomScale();
        SetRandomRotation();
    }

    void FixedUpdate()
    {
        rigidbodyAsteroid.AddTorque(vectorRotationAsteroid);
        Move();
    }
    private void OnTriggerEnter(Collider other)
    {
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
    #endregion
    #region Methods
    private void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        if (transform.position.z < Camera.main.transform.position.z) Destroy(gameObject);
    }

    private void CreateChild()
    {
        int numberOfChilds = Random.Range(2, 7);
        float childMass = rigidbodyAsteroid.mass / numberOfChilds;
        if (childMass > 2)
        {
            for (int i = 0; i < numberOfChilds; i++)
            {
                Vector3 childPos = new Vector3(transform.position.x, transform.position.y + (i / 2), transform.position.z - (i / 2));
                GameObject childAsteroid = Instantiate(asteroid, childPos, Quaternion.identity);
                Rigidbody childAsteroid_rb = childAsteroid.GetComponent<Rigidbody>();
                childAsteroid_rb.mass = childMass;
                childAsteroid_rb.AddExplosionForce(rigidbodyAsteroid.mass, transform.position, rigidbodyAsteroid.mass / 10);
            }
        }
    }

    private void SetRandomScale()
    {
        float baseScale = rigidbodyAsteroid.mass / 10;
        float fluctuationScale = baseScale / 5;
        float xScale = baseScale + Random.Range(-fluctuationScale, fluctuationScale);
        float yScale = baseScale + Random.Range(-fluctuationScale, fluctuationScale);
        float zScale = baseScale + Random.Range(-fluctuationScale, fluctuationScale);
        transform.localScale = new Vector3(xScale, yScale, zScale);
    }

    private void SetRandomRotation()
    {
        vectorRotationAsteroid = new Vector3(Random.Range(-maximumRotation, maximumRotation),
            Random.Range(-maximumRotation, maximumRotation),
            Random.Range(-maximumRotation, maximumRotation));
    }

    private void SetLife()
    {
        //Prefab mass = 100
        life = Mathf.FloorToInt(rigidbodyAsteroid.mass / 20);
    }

    private void CheckLife()
    {
        Instantiate(damaged, transform.position, Quaternion.identity);
        if (life < 1)
        {
            CreateChild();
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    #endregion
}
