using UnityEngine;
using UnityEngine.Serialization;

public class ShakeEffect : MonoBehaviour {

	public float ShakeAmount = 0.025f;
	private Vector3 _originalPosition;

	public void ShakeMedium()
	{
		_originalPosition = transform.position;

		InvokeRepeating("Shake", 0, .03f);
		Invoke("StopShaking", 0.4f);
	}
	
	private void Shake()
	{
		if (ShakeAmount <= 0) return;
	    
		var quakeAmountX = Random.value * ShakeAmount * 2 - ShakeAmount;
		var quakeAmountY = Random.value * ShakeAmount * 2 - ShakeAmount;
	    
		var newPosition = transform.position;
		newPosition.x += quakeAmountX;
		newPosition.y += quakeAmountY;
		transform.position = newPosition;
	}

	private void StopShaking()
	{
		CancelInvoke("Shake");
		transform.position = _originalPosition;
	}
}
