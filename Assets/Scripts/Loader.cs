using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Loader : MonoBehaviour
{

	public GameObject gameManager;
	
	// Use this for initialization
	void Awake () {
		if (GameManager.Instance == null)
		{
			Instantiate(gameManager);
		}
	}
}
