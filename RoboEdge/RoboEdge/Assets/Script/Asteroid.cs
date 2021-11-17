using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public GameObject asteroid;

    [SerializeField]
    private float maxRotation = 90;
    private float speed;
    private int life;
    private Vector3 rotationAsteroid;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        speed = 20;
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
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddTorque(rotationAsteroid);
        Move();
    }

    protected virtual void Move()
    {
        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        if (transform.position.z < Camera.main.transform.position.z)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);
            life--;
            if (life <1)
            {
                CreateChild();
                Destroy(gameObject);
            }
            
        }
    }

    private void CreateChild()
    {
        int numChild = Random.Range(2, 7);
        float childMass = rb.mass / numChild;
        if (childMass > 2)
        {
            for (int i = 0; i < numChild; i++)
            {
                Vector3 childPos = new Vector3(transform.position.x, transform.position.y +(i/2), transform.position.z - (i / 2));
                GameObject childAsteroid = Instantiate(asteroid, childPos, Quaternion.identity);
                Rigidbody childAsteroid_rb = childAsteroid.GetComponent<Rigidbody>();
                childAsteroid_rb.mass = childMass;
                childAsteroid_rb.AddExplosionForce(rb.mass, transform.position, rb.mass / 10);
            }
        }
        
    }
}
