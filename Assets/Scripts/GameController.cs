using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
	private int _level = 5;

	private BoardManager boardScript;
	
	// Use this for initialization
	private void Awake()
	{
		boardScript = GetComponent<BoardManager>();
		boardScript.CreateBoard();

		var blockSequence = boardScript.GetRandomBlockSequence(2, 0, 4);
		StartCoroutine(boardScript.CreateTriggers(blockSequence));
	}
}

