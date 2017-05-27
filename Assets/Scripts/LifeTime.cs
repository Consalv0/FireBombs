using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour {
	public float lifeTime = 300;
	void FixedUpdate () {
		lifeTime--;
		if(lifeTime <= 0)
			Destroy(gameObject);
		var color = GetComponent<Renderer>().material.color;
		if (color.a > 0.05f)
			GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, color.a - 0.4f/lifeTime);
	}
}
