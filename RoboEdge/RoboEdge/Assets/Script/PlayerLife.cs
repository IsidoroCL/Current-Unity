using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private int life;
    [SerializeField]
    private GameObject damaged;
    [SerializeField]
    private GameObject explosion;

    private Animator anim;
    private int lifeTotal;
    #endregion
    #region Unity methods
    private void Awake()
    {
        anim = GetComponent<Animator>();
        lifeTotal = life;
    }

    private void Start()
    {
        LifeBarHandler.SetHealthBarValue(1.0f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Asteroid"))
        {
            ReceiveDamage();
            collision.gameObject.SetActive(false);
        }
    }
    #endregion
    #region Methods
    private void ReceiveDamage()
    {
        life--;
        LifeBarHandler.SetHealthBarValue((float)life / lifeTotal);
        Instantiate(damaged, transform.position, Quaternion.identity);
        if (life < 1)
        {
            anim.SetBool("Die", true);
            Instantiate(explosion, transform.position, Quaternion.identity);
            GameFlow.sharedInstance.CallGameOver();
        }
    }
    #endregion
}
