using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	private GameObject _levelPanel;
	private GameObject _mainMenu;

	private BoardManager _boardScript;
	private List<Block> _blockSequence = new List<Block>();

	private int _level = 2;
	private int _startX = 2;
	private int _startY = 0;
	
	private int _currentSequenceIndex = 0;

	private bool _userInputEnabled;
	
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
		
		_userInputEnabled = false;
		_levelPanel = GameObject.Find("LevelPanel");
		_levelPanel.SetActive(false);
		
		_mainMenu = GameObject.Find("MainMenuPanel");
		_mainMenu.SetActive(false);
		
		
		_boardScript = GetComponent<BoardManager>();
		InitGameBoard();
		
		PauseGame();
		ShowMainMenu();
	}

	public void ChooseBlock(Vector2 position)
	{
		if (_blockSequence.Count == 0 || !_userInputEnabled)
		{
			return;
		}
		
		var nextBlock = _blockSequence.First();

		var nextSequencePosition = nextBlock.Position;
		if (position == nextSequencePosition)
		{
			_boardScript.MarkCorrectBlock(position);
			SoundManager.Instance.PlayNote(_currentSequenceIndex++);
			_blockSequence.Remove(nextBlock);
			if (_blockSequence.Count == 0)
			{
				StartCoroutine(Victory());
			}
		}
		else
		{
			_boardScript.MarkErrorBlock(position);
			_boardScript.ShakeAllBlocks();
			SoundManager.Instance.PlayErrorSound();
			CameraController.Instance.GetComponent<ShakeEffect>().ShakeMedium();

			StartCoroutine(Fail());
		}
	}

	public IEnumerator InitLevel(int level, int startX, int startY)
	{
		_currentSequenceIndex = 0;
		var numberOfBlocksToChoose = level + 2;

		_boardScript.ResetBoard();
		_blockSequence = _boardScript.GetRandomBlockSequence(startX, startY, numberOfBlocksToChoose);
		
		_boardScript.MarkStartingBlock(_blockSequence.First().Position);
		yield return new WaitForSeconds(1f);
		yield return StartCoroutine(_boardScript.MarkPlayBlocks(_blockSequence, 0.5f));
		yield return new WaitForSeconds(1f);
		_boardScript.UnmarkPlayBlocks();
		
		PopFirstBlockFromSequence();
		_userInputEnabled = true;
	}

	private void ShowLevelMenu(bool show = true)
	{
		_levelPanel.SetActive(show);
	}

	private void ShowMainMenu(bool show = true)
	{
		_mainMenu.SetActive(show);
	}

	private void PauseGame(bool pause = true)
	{
		Time.timeScale = pause ? 0f : 1f;
	}

	private IEnumerator Victory()
	{
		_userInputEnabled = false;
		// TODO play victory sound
		yield return new WaitForSeconds(1f);
		PauseGame();
		ShowLevelMenu();
	}
	
	private IEnumerator Fail()
	{
		_userInputEnabled = false;
		// TODO play victory sound
		yield return new WaitForSeconds(1f);
		PauseGame();
		ShowLevelMenu();
	}

	private void PopFirstBlockFromSequence()
	{
		_blockSequence.RemoveAt(0);
	}

	private void InitGameBoard()
	{
		_boardScript.CreateBoard();
	}

	public void StartButton()
	{
		ShowLevelMenu(false);
		ShowMainMenu(false);
		
		PauseGame(false);	
		StartCoroutine(InitLevel(_level, _startX, _startY));
	}

	public void NextButton()
	{
		ShowLevelMenu(false);
		ShowMainMenu(false);
		
		PauseGame(false);
		StartCoroutine(InitLevel(++_level, _startX, _startY));
	}

	public void BackButton()
	{
		PauseGame();
		ShowLevelMenu(false);
		ShowMainMenu();
	}
}

