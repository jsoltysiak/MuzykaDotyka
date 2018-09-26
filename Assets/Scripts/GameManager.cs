using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	
	private int _level = 5;

	private BoardManager boardScript;
	
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		
		DontDestroyOnLoad(gameObject);
		
		boardScript = GetComponent<BoardManager>();
		InitGameBoard();

		InitLevel(_level);
	}

	private void InitLevel(int level)
	{
		var numberOfBlocksToChoose = level + 2;
		var startX = 2;
		var startY = 0;

		var blockSequence = boardScript.GetRandomBlockSequence(startX, startY, numberOfBlocksToChoose);
		
		StartCoroutine(boardScript.CreateTriggers(blockSequence));
	}

	private void InitGameBoard()
	{
		boardScript.CreateBoard();
	}
}

