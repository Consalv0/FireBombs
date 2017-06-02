using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour {
	public float lifeTime = 300;
	Material material;
	void Start() {
		material = GetComponent<Renderer>().material;
		material.SetFloat("_Mode", 2);
		material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		material.SetInt("_ZWrite", 0);
		material.DisableKeyword("_ALPHATEST_ON");
		material.EnableKeyword("_ALPHABLEND_ON");
		material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		material.renderQueue = 3000;
	}

	void FixedUpdate () {
		lifeTime--;
		if(lifeTime <= 0)
			Destroy(gameObject);
		var color = material.color;
		if (color.a > 0.05f)
			material.color = new Color(color.r, color.g, color.b, color.a - 0.4f/lifeTime);
	}
}
