using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    private GameObject player;
    public static GameFlow sharedInstance;
    private void Awake()
    {
        sharedInstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallGameOver()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        //IMPLEMENTAR
        Debug.Log("Muerto");
        Destroy(player);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}
