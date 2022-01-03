using UnityEngine;

public class EnemySpiral : Enemy
{
    #region Fields
    private float timeCounter;
    [SerializeField]
    private float radius;
    #endregion
    #region Unity methods
    private void Awake()
    {
        speed = 5.0f;
        life = 1;
        repeatFireTime = 3;
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
        SpiralMovement();
    }

    protected void SpiralMovement()
    {
        timeCounter += Time.deltaTime;
        float x = Mathf.Cos(timeCounter);
        float y = Mathf.Sin(timeCounter);
        transform.position = new Vector3(x * radius, y * radius, transform.position.z);
    }
    #endregion
}
