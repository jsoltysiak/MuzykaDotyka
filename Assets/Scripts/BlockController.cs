using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
	public Sprite YellowSprite;
	public Sprite GreenSprite;
	public Sprite BlackSprite;
	public Sprite RedSprite;

	private const float InitialEffectAlpha = 0.5f;
	
	private void OnMouseDown()
	{
		GameManager.Instance.ChooseBlock(transform.position);
	}	

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Start"))
		{
			GetComponent<SpriteRenderer>().sprite = GreenSprite;
		}
		else if (other.CompareTag("Play"))
		{
			GetComponent<SpriteRenderer>().sprite = YellowSprite;
		}
		else if (other.CompareTag("Error"))
		{
			GetComponent<SpriteRenderer>().sprite = RedSprite;
		}
		else if (other.CompareTag("Good"))
		{
			GetComponent<SpriteRenderer>().sprite = GreenSprite;
		}

		StartCoroutine(FlashBlock(0.5f));
		
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		GetComponent<SpriteRenderer>().sprite = BlackSprite;
	}

	private IEnumerator FlashBlock(float effectTime)
	{
		var effectObject = new GameObject("effect");
		effectObject.AddComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
		effectObject.transform.position = transform.position;
		var effectRenderer = effectObject.transform.GetComponent<SpriteRenderer>();
		effectRenderer.sortingLayerName = "Effects";

		for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / effectTime)
		{
			var newAlpha = Mathf.Lerp(InitialEffectAlpha, 0, t);
			effectRenderer.color = new Color(1, 1, 1, newAlpha);
			effectObject.transform.localScale = new Vector3(1.0f + t, 1.0f + t);
			yield return null;
		}
		DestroyImmediate(effectObject);
	}
}

public class Block
{
	public List<Block> NextSteps { get; set; }
	public GameObject BlockObject { get; private set; }
	public Vector2 Position { get; private set; }

	public Block(GameObject blockObject, Vector2 position)
	{
		BlockObject = blockObject;
		Position = position;
	}
}