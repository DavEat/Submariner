using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    private void Awake() { inst = this; }

    /// <summary>delegate use to call event whenever the sonar make ping</summary>
    /// <param name="sonarSource">the source of the sonar</param>
    public delegate void SonarPing(Vector3 sonarSource);
    public static SonarPing sonarPing;
    #region Vars
    int _score = 0;

    [SerializeField] TextMeshProUGUI _score_text = null;
    [SerializeField] TextMeshProUGUI _life0_text = null;
    [SerializeField] TextMeshProUGUI _life1_text = null;
    [SerializeField] GameObject _gameOver = null;
    #endregion
    #region MonoFunctions
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    #endregion
    #region Functions
    /// <summary>Update and Display the current score of the player</summary>
    /// <param name="score">the current score to display</param>
    public void UpdateScore(int score)
    {
        _score = score;
        _score_text.text = score.ToString();
    }
    /// <summary>Display the current life of the player</summary>
    /// <param name="point">curretn number of life point</param>
    public void SetLife(int point)
    {
        string l = "";

        for (int i = 0; i < point; i++)
            l += "|";

        _life0_text.text = l;
        _life1_text.text = l;

        if (point <= 0)
            _gameOver.SetActive(true);
    }

    /// <summary>Reload the scene/ restart the game</summary>
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}