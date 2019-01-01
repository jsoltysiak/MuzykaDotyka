using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockController : MonoBehaviour
{
	public Sprite YellowSprite;
	public Sprite GreenSprite;
	public Sprite BlackSprite;
	public Sprite RedSprite;
	public Sprite BlueSprite;

	private const float InitialEffectAlpha = 0.5f;
	
	private int _triggerCount = 0;

	private void OnMouseDown()
	{
		GameManager.Instance.ChooseBlock(transform.position);
	}	

	private void OnTriggerEnter2D(Collider2D other)
	{
		_triggerCount++;
		if (other.CompareTag("Start"))
		{
			GetComponent<SpriteRenderer>().sprite = BlueSprite;
		}
		else if (other.CompareTag("Play"))
		{
			GetComponent<SpriteRenderer>().sprite = GreenSprite;
			return;
		}
		else if (other.CompareTag("PlayCurrent"))
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

		StartCoroutine(FlashBlock(0.8f));
		
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		--_triggerCount;
		if (_triggerCount < 1)
		{
			GetComponent<SpriteRenderer>().sprite = BlackSprite;
		}
	}

	private IEnumerator FlashBlock(float effectTime)
	{
		var effectObject = new GameObject("effect");
		effectObject.AddComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
		effectObject.transform.position = transform.position;
		var effectRenderer = effectObject.transform.GetComponent<SpriteRenderer>();
		effectRenderer.sprite = YellowSprite;
		effectRenderer.sortingLayerName = "Effects";

		for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / effectTime)
		{
			var newAlpha = Mathf.Lerp(InitialEffectAlpha, 0, t * 2);
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