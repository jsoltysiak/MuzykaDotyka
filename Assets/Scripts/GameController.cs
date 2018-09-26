using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
	private int _level = 5;

	private BoardManager boardScript;
	
	private void Awake()
	{
		boardScript = GetComponent<BoardManager>();
		boardScript.CreateBoard();

		var numberOfBlocksToChoose = _level + 2;
		var startX = 2;
		var startY = 0;

		var blockSequence = boardScript.GetRandomBlockSequence(startX, startY, numberOfBlocksToChoose);
		StartCoroutine(boardScript.CreateTriggers(blockSequence));
	}
}

