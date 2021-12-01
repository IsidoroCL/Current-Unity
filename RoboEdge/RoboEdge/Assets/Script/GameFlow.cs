using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    #region Fields
    private GameObject player;
    private AudioSource mainMusic;
    public static GameFlow sharedInstance;
    private bool final;

    [SerializeField]
    WaveScenario scenario;

    [SerializeField]
    private GameObject[] enemies;
    //0: Simple
    //1: Aimed
    //2: Spiral
    //3: ScrollX
    //4: Small
    //5: Asteroids

    [SerializeField]
    private float instantiateDistance;

    [SerializeField]
    private GameObject textWin;
    #endregion
    #region Unity methods
    private void Awake()
    {
        sharedInstance = this;
        mainMusic = GetComponent<AudioSource>();
        final = false;
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainMusic.PlayDelayed(1.0f);
        //StartCoroutine(Waves());
        scenario.Init();
    }
    void Update()
    {
        final = scenario.Progress();
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
        if (final)
        {
            //If there are not any final enemies, then player won
            EnemySmall[] enemies = GameObject.FindObjectsOfType<EnemySmall>();
            if (enemies.Length < 1) Win();
        }
    }
    #endregion
    #region Methods

    public void CallGameOver()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        Destroy(player);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
    private void Win()
    {
        textWin.SetActive(true);
    }
    #endregion
}
