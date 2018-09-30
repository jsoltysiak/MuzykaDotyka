using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float MinimumViewSize = 6.0f;

	private void Start () {
		var aspectRatio = (float)Screen.width / (float)Screen.height;
		var cameraHeight = MinimumViewSize / aspectRatio;

		cameraHeight = cameraHeight < MinimumViewSize ? MinimumViewSize : cameraHeight;
		
		GetComponent<Camera>().orthographicSize = cameraHeight / 2.0f;
	}
}
