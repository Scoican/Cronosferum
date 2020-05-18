using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
	public bool isFullyGrown;

	private void Start()
	{
		isFullyGrown = false;
	}
	// Update is called once per frame
	void Update()
	{
		if (transform.localScale.x != 1)
		{
			var newScale = Mathf.Lerp(0, 1, Time.time / 20);
			transform.localScale = new Vector3(newScale, newScale, newScale);
		}
		else
		{
			isFullyGrown = true;
		}
	}


}
