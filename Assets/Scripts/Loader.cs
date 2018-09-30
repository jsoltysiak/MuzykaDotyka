using UnityEngine;

public class Loader : MonoBehaviour
{
	public GameObject GameManagerPrefab;
	
	private void Awake () 
	{
		if (GameManager.Instance == null)
		{
			Instantiate(GameManagerPrefab);
		}
	}
}
