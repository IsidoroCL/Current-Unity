using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    #region Fields
    private GameObject player;
    private AudioSource mainMusic;
    public static GameFlow sharedInstance;
    private bool endOfScenario;

    [SerializeField]
    WaveScenario scenario;

    [SerializeField]
    private GameObject textWin;
    #endregion
    #region Unity methods
    private void Awake()
    {
        sharedInstance = this;
        mainMusic = GetComponent<AudioSource>();
        endOfScenario = false;
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainMusic.PlayDelayed(1.0f);
        scenario.Initialize();
    }
    void Update()
    {
        endOfScenario = scenario.Progress();
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
        if (endOfScenario && !ExistsEnemiesInGame())
        {
            Win();
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

    private bool ExistsEnemiesInGame()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0) return true;
        else return false;
    }
    #endregion
}
