using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject cross;
    [SerializeField]
    private float speed;
    private Vector3 direction;
    private Rigidbody rigidbodyBullet;

    [SerializeField]
    private string target;
    #endregion
    #region Unity methods
    private void Awake()
    {
        speed = 60.0f;
        rigidbodyBullet = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (cross == null) cross = GameObject.FindGameObjectWithTag(target);
        if (cross != null)
        {
            rigidbodyBullet.velocity = Vector3.zero;
            Vector3 difference = cross.transform.position - transform.position;
            float distance = difference.magnitude;
            direction = difference / distance;
            direction = difference;
            direction.Normalize();
            rigidbodyBullet.velocity = direction * speed;
        }
    }
    #endregion
    #region Methods
    public void SetConfiguration(Vector3 position, Vector3 scale, float shootAccumulated)
    {
        transform.position = position;
        transform.localScale = scale;
        transform.localScale *= shootAccumulated;
        gameObject.SetActive(true);
    }
    #endregion
}
