using UnityEngine;

public class Loader : MonoBehaviour
{
	public GameObject gameManagerPrefab;
	public GameObject soundManagerPrefab;
	
	private void Awake () 
	{
		if (GameManager.Instance == null)
		{
			Instantiate(gameManagerPrefab);
		}
		
		if (SoundManager.Instance == null)
		{
			Instantiate(soundManagerPrefab);
		}
	}
}
