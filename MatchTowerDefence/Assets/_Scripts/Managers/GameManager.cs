using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance;

    private void Awake()
    {
        // Only 1 Game Manager can exist at a time
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = GetComponent<GameManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

    }

}
