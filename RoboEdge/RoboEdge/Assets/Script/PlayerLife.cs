using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField]
    private int life;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.SetActive(false);
            life--;
            if (life < 1)
            {
                anim.SetBool("Die", true);
                GameFlow.sharedInstance.CallGameOver();
            }
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            life--;
            if (life < 1)
            {
                anim.SetBool("Die", true);
                GameFlow.sharedInstance.CallGameOver();
            }
        }
    }

}
