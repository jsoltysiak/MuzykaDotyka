using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private int _level = 5;

	private BoardManager boardScript;
	
	private void Awake()
	{
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

