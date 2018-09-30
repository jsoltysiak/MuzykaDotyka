using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	
	private int _level = 5;

	private BoardManager boardScript;
	private List<Block> _blockSequence;
	
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

		StartCoroutine(InitLevel(_level));
	}

	public void ChooseBlock(Vector2 position)
	{
		var nextBlock = _blockSequence.FirstOrDefault();
		if (nextBlock == null) return;

		var nextSequencePosition = nextBlock.Position;
		if (position == nextSequencePosition)
		{
			boardScript.MarkCorrectBlock(position);
			_blockSequence.Remove(nextBlock);
		}
		else
		{
			boardScript.MarkErrorBlock(position);
		}
		
	}

	private IEnumerator InitLevel(int level)
	{
		var numberOfBlocksToChoose = level + 2;
		var startX = 2;
		var startY = 0;

		_blockSequence = boardScript.GetRandomBlockSequence(startX, startY, numberOfBlocksToChoose);
		
		boardScript.MarkStartingBlock(_blockSequence.First().Position);
		yield return new WaitForSeconds(1f);
		yield return StartCoroutine(boardScript.MarkPlayBlocks(_blockSequence, 0.5f));
		yield return new WaitForSeconds(1f);
		boardScript.UnmarkPlayBlocks();
		
		PopFirstBlockFromSequence();
	}

	private void PopFirstBlockFromSequence()
	{
		_blockSequence.RemoveAt(0);
	}

	private void InitGameBoard()
	{
		boardScript.CreateBoard();
	}
}

