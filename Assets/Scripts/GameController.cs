using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject BlockPrefab;
	public GameObject TriggerPlay;
	public GameObject TriggerStart;
	[Range(1,10)]
	public int Rows = 5;
	[Range(1,10)]
	public int Columns = 5;

	private Block[,] _blockList;
	private int _level = 5;
	
	// Use this for initialization
	private void Awake()
	{
		CreateBoard();

		var blockSequence = GetBlockSequence(2, 0, 4);
		StartCoroutine(CreateTriggers(blockSequence));
	}

	private List<Block> GetBlockSequence(int startX, int startY, int level)
	{
		var blockSequence = new List<Block>();

		var numberOfBlocksToChoose = level + 2;
		var block = _blockList[startX, startY];
		blockSequence.Add(block);

		var previousBlock = block;
		for (var i = 0; i < numberOfBlocksToChoose; i++)
		{
			var possibleSteps = block.NextSteps.Where(b =>
				b != previousBlock &&
				(int) b.BlockObject.transform.position.x != startX && 
				(int) b.BlockObject.transform.position.y != startY).ToList();

			block = possibleSteps[Random.Range(0, possibleSteps.Count)];
			blockSequence.Add(block);
		}

		return blockSequence;
	}
	
	private IEnumerator CreateTriggers(IList<Block> blockSequence)
	{
		Instantiate(TriggerStart, blockSequence[0].BlockObject.transform.position, Quaternion.identity);
		yield return new WaitForSeconds(1.0f);
		
		for (var index = 1; index < blockSequence.Count; index++)
		{
			Instantiate(TriggerPlay, blockSequence[index].BlockObject.transform.position, Quaternion.identity);
			yield return new WaitForSeconds(.5f);
		}
	}
	


	private void CreateBoard()
	{
		_blockList = new Block[Columns, Rows];
		for (var x = 0; x< Columns; x++)
		{
			for (var y = 0; y < Rows; y++)
			{
				var block = new Block
				{
					BlockObject = Instantiate(BlockPrefab, new Vector3(x, y, 0), Quaternion.identity),
				};
				_blockList[x, y] = block;
			}
		}
		
		for (var x = 0; x< Columns; x++)
		{
			for (var y = 0; y < Rows; y++)
			{
				_blockList[x, y].NextSteps = GetNextBlocksFromPosition(x, y);
			}
		}
	}

	private List<Block> GetNextBlocksFromPosition(int x, int y)
	{
		var nextBlocks = new List<Block>();

		if (x > 0)
		{
			nextBlocks.Add(_blockList[x - 1, y]);
			if (y > 0)
			{
				nextBlocks.Add(_blockList[x - 1, y - 1]);
			}

			if (y < Rows - 1)
			{
				nextBlocks.Add(_blockList[x - 1, y + 1]);
			}
		}
		
		if (x < Columns - 1)
		{
			nextBlocks.Add(_blockList[x + 1, y]);
			if (y > 0)
			{
				nextBlocks.Add(_blockList[x + 1, y - 1]);
			}

			if (y < Rows - 1)
			{
				nextBlocks.Add(_blockList[x + 1, y + 1]);
			}
		}

		if (y > 0)
		{
			nextBlocks.Add(_blockList[x, y - 1]);
		}
		
		if (y < Rows - 1)
		{
			nextBlocks.Add(_blockList[x, y + 1]);
		}
		
		return nextBlocks;
	}
	
	
	public class Block
	{
		public List<Block> NextSteps;
		public GameObject BlockObject;
	}
}
