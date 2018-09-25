using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
	public Sprite yellowSprite;
	public Sprite greenSprite;
	public Sprite blackSprite;
	public Sprite redSprite;
	
	// Use this for initialization
	private void Start() 
	{
	}
	
	// Update is called once per frame
	private void Update()
	{
	}

	private void OnMouseDown()
	{
		GetComponent<SpriteRenderer>().sprite = redSprite;
		StartCoroutine(FlashBlock(0.5f));
	}	

	private void OnMouseUp()
	{
		//GetComponent<SpriteRenderer>().sprite = blackSprite;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Start"))
		{
			GetComponent<SpriteRenderer>().sprite = greenSprite;
		}
		else if (other.CompareTag("Play"))
		{
			GetComponent<SpriteRenderer>().sprite = yellowSprite;
		}
		else if (other.CompareTag("Error"))
		{
			GetComponent<SpriteRenderer>().sprite = redSprite;
		}

		StartCoroutine(FlashBlock(0.5f));
		
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		GetComponent<SpriteRenderer>().sprite = blackSprite;
	}

	private IEnumerator FlashBlock(float effectTime)
	{
		GameObject effectObject = new GameObject("effect");
		effectObject.AddComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
		effectObject.transform.position = transform.position;
		var effectRenderer = effectObject.transform.GetComponent<SpriteRenderer>();
		effectRenderer.sortingLayerName = "Effects";
		var alpha = 0.5f;

		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / effectTime)
		{
			var newAlpha = Mathf.Lerp(alpha, effectTime, t);
			effectRenderer.color = new Color(1, 1, 1, newAlpha);
			effectObject.transform.localScale = new Vector3(1.0f + t, 1.0f + t);
			yield return null;
		}
		DestroyImmediate(effectObject);
	}
}
