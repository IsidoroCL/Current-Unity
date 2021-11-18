using UnityEngine;

public class Asteroid : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float maxRotation = 90;
    private float speed;
    private int life;
    private Vector3 rotationAsteroid;
    private Vector3 direction;
    private Rigidbody rb;

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
        rb = GetComponent<Rigidbody>();
        speed = 5;
        direction = new Vector3(1, 0, -2);

        //Prefab mass = 100
        life = Mathf.FloorToInt(rb.mass / 10); 
        
        //Random Scale
        float baseScale = rb.mass / 10;
        float fluctuationScale = baseScale / 5;
        float xScale = baseScale + Random.Range(-fluctuationScale, fluctuationScale);
        float yScale = baseScale + Random.Range(-fluctuationScale, fluctuationScale);
        float zScale = baseScale + Random.Range(-fluctuationScale, fluctuationScale);
        transform.localScale = new Vector3(xScale, yScale, zScale);

        //Random rotation
        rotationAsteroid = new Vector3(Random.Range(-maxRotation, maxRotation),
            Random.Range(-maxRotation, maxRotation),
            Random.Range(-maxRotation, maxRotation));
    }

    void FixedUpdate()
    {
        rb.AddTorque(rotationAsteroid);
        Move();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
            life--;
            Instantiate(damaged, transform.position, Quaternion.identity);
            if (life < 1)
            {
                CreateChild();
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
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
        int numChild = Random.Range(2, 7);
        float childMass = rb.mass / numChild;
        if (childMass > 2)
        {
            for (int i = 0; i < numChild; i++)
            {
                Vector3 childPos = new Vector3(transform.position.x, transform.position.y + (i / 2), transform.position.z - (i / 2));
                GameObject childAsteroid = Instantiate(asteroid, childPos, Quaternion.identity);
                Rigidbody childAsteroid_rb = childAsteroid.GetComponent<Rigidbody>();
                childAsteroid_rb.mass = childMass;
                childAsteroid_rb.AddExplosionForce(rb.mass, transform.position, rb.mass / 10);
            }
        }
    }
    #endregion
}
