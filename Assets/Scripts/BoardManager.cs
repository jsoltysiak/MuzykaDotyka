using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public GameObject BlockPrefab;

	[Range(1,10)]
	public int Rows = 5;
	[Range(1,10)]
	public int Columns = 5;
	
	public GameObject TriggerPlay;
	public GameObject TriggerStart;
	public GameObject TriggerError;
	public GameObject TriggerGood;

	private Block[,] _blockList;

	private readonly List<GameObject> _playTriggers = new List<GameObject>();
	private GameObject _startTrigger = null;
	
	public void CreateBoard()
	{
		_blockList = new Block[Columns, Rows];
		for (var x = 0; x< Columns; x++)
		{
			for (var y = 0; y < Rows; y++)
			{
				var block = new Block(
					Instantiate(BlockPrefab, new Vector3(x, y, 0), Quaternion.identity),
					new Vector2(x, y));
				
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
	
	public List<Block> GetRandomBlockSequence(int startX, int startY, int sequenceLength)
	{
		var blockSequence = new List<Block>();

		var numberOfBlocksToChoose = sequenceLength;
		var startBlock = _blockList[startX, startY];
		blockSequence.Add(startBlock);

		var currentBlock = startBlock;
		var previousBlock = currentBlock;
		for (var i = 0; i < numberOfBlocksToChoose; i++)
		{
			var possibleNextBlocks = currentBlock.NextSteps.Where(b =>
				b != startBlock &&
				b != previousBlock).ToList();

			previousBlock = currentBlock;
			currentBlock = possibleNextBlocks[Random.Range(0, possibleNextBlocks.Count)];
			blockSequence.Add(currentBlock);
		}

		return blockSequence;
	}
	
	public IEnumerator MarkPlayBlocks(IList<Block> blockSequence, float waitTime)
	{
		for (var index = 1; index < blockSequence.Count; index++)
		{
			_playTriggers.Add(CreateTrigger(TriggerPlay, blockSequence[index].Position));
			yield return new WaitForSeconds(waitTime);
		}
	}

	public void MarkStartingBlock(Vector2 position)
	{
		if(_startTrigger != null)
		{
			RemoveStartTrigger();
		}

		_startTrigger = CreateTrigger(TriggerStart, position);
	}

	private GameObject CreateTrigger(GameObject triggerObject, Vector2 position)
	{
		 return Instantiate(triggerObject, new Vector3(position.x, position.y, 1), Quaternion.identity);
	}

	public void UnmarkPlayBlocks()
	{
		_playTriggers.ForEach(Destroy);
		_playTriggers.Clear();
	}
	
	private void RemoveStartTrigger()
	{
		Destroy(_startTrigger);
		_startTrigger = null;
	}
	
	public void MarkErrorBlock(Vector2 position)
	{
		CreateTrigger(TriggerError, new Vector3(position.x, position.y, -1f));
	}
	
	public void MarkCorrectBlock(Vector2 position)
	{
		CreateTrigger(TriggerGood, position);
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

	public void ShakeAllBlocks()
	{
		foreach (var block in _blockList)
		{
			block.BlockObject.GetComponent<ShakeEffect>().ShakeMedium();
		}
	}
}

