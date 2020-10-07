using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour {
	public static GUIManager instance;

    [Header("HUD Texts")]
    [SerializeField] public TMP_Text phaseTxt;
    [SerializeField] private TMP_Text moveCounterTxt;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Image backGround;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] public GameObject pauseMenu;

    [SerializeField] public Button resumeButton;
    [SerializeField] public Button pauseButton;

    private int score;
    private int moveCounter;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            phaseTxt.text = score.ToString();
        }
    }

    public int MoveCounter
    {
        get
        {
            return moveCounter;
        }
        set
        {
            moveCounter = value;
            moveCounterTxt.text = moveCounter.ToString();
        }
    }

    void Awake() {
		instance = GetComponent<GUIManager>();

        moveCounter = 5;
        moveCounterTxt.text = moveCounter.ToString();
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void InitiateGame()
    {
        mainMenu.SetActive(false);
        backGround.gameObject.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
}
