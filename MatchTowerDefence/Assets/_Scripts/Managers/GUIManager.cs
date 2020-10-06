using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour {
	public static GUIManager instance;

	[SerializeField] public Text phaseTxt;
    [SerializeField] private Text moveCounterTxt;

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject CreditsMenu;

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

}
