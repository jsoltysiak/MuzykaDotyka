using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	private BoardManager _boardScript;
	private List<Block> _blockSequence;

	private int _level = 2;
	private int _startX = 2;
	private int _startY = 0;
	
	private int _currentSequenceIndex = 0;
	
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
		
		_boardScript = GetComponent<BoardManager>();
		InitGameBoard();

		StartCoroutine(InitLevel(_level, _startX, _startY));
	}

	public void ChooseBlock(Vector2 position)
	{
		var nextBlock = _blockSequence.FirstOrDefault();
		if (nextBlock == null) return;

		var nextSequencePosition = nextBlock.Position;
		if (position == nextSequencePosition)
		{
			_boardScript.MarkCorrectBlock(position);
			SoundManager.Instance.PlayNote(_currentSequenceIndex++);
			_blockSequence.Remove(nextBlock);
		}
		else
		{
			_boardScript.MarkErrorBlock(position);
			_boardScript.ShakeAllBlocks();
			SoundManager.Instance.PlayErrorSound();
			CameraController.Instance.GetComponent<ShakeEffect>().ShakeMedium();
		}
	}

	private IEnumerator InitLevel(int level, int startX, int startY)
	{
		_currentSequenceIndex = 0;
		var numberOfBlocksToChoose = level + 2;

		_blockSequence = _boardScript.GetRandomBlockSequence(startX, startY, numberOfBlocksToChoose);
		
		_boardScript.MarkStartingBlock(_blockSequence.First().Position);
		yield return new WaitForSeconds(1f);
		yield return StartCoroutine(_boardScript.MarkPlayBlocks(_blockSequence, 0.5f));
		yield return new WaitForSeconds(1f);
		_boardScript.UnmarkPlayBlocks();
		
		PopFirstBlockFromSequence();
	}

	private void PopFirstBlockFromSequence()
	{
		_blockSequence.RemoveAt(0);
	}

	private void InitGameBoard()
	{
		_boardScript.CreateBoard();
	}
}

