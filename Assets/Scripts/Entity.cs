﻿﻿﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Entity : EntityTypes {
  public EntityType type = EntityType.None;
	public bool isAlive;
	public float health = 100;
	public int level = 1;
  public Material material;

	public bool isActive;
	public float smoothVelPosition;

  bool flick = true;
	Material defaultMaterial;
	/* This script only will decide wich type of entity is and how it behaves */
	void Start() {
		switch (type) {
			case EntityType.Shatter:
				if (GetComponent<Renderer>() != null) {
					health += health + Random.Range(-50, 50);
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
				break;
		}
		defaultMaterial = GetComponentInChildren<Renderer>().materials[0];
  }

	void FixedUpdate() {
    switch (type) {
      case EntityType.Shatter:
        health--;
        if (health <= 0)
          Destroy(gameObject);
        var color = material.color;
        if (color.a > 0.05f)
          material.color = new Color(color.r, color.g, color.b, color.a - 0.4f / health);
        break;
      default:
        if (isAlive) {
          if (health < 0) {
            isAlive = !isAlive;
            GetComponent<Destroy>().Go();
          }
        }
        break;
    }
  }

	void OnTriggerStay(Collider other) {
		if (other.GetComponent<Enemy>() != null) {
			TakeDamage(0.01f);
		}
	}

	public float TakeDamage(float percentage) {
		if (isAlive) {
			health = health - percentage / level;
			if (flick)
				StartCoroutine(DamageFlickr());
			return health;
		}
		return health;

	}

	IEnumerator DamageFlickr() {
		flick = false;
		GetComponentInChildren<Renderer>().material = (Material)Resources.Load("Materials/RedMetallic", typeof(Material));
		yield return new WaitForSeconds(0.5f);
		GetComponentInChildren<Renderer>().material = defaultMaterial;
		flick = true;
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(Entity))]
public class EntityEditor : Editor {
  public override void OnInspectorGUI() {
    Entity script = (Entity)target;
    script.type = (EntityType)EditorGUILayout.EnumPopup("Type of Entity", script.type);
    EditorGUILayout.Space();

    switch (script.type) {
      case EntityType.Shatter:
				script.health = EditorGUILayout.Slider("Life Time", script.health, 0f, 100f * script.level);
        script.level = EditorGUILayout.IntSlider("Level", script.level, 1, 100);
        break;

			case EntityType.SpawnBase:
				if (script.gameObject.GetComponent<SpawnEntities>() == null)
					script.gameObject.AddComponent<SpawnEntities>();
				break;

      default:
        EditorGUILayout.BeginHorizontal();
        script.isAlive = EditorGUILayout.Toggle(script.isAlive, GUILayout.MaxWidth(15.0f));
        EditorGUILayout.LabelField("Life", EditorStyles.boldLabel, GUILayout.MaxWidth(98.0f));
        GUILayout.FlexibleSpace();
        GUI.enabled = script.isAlive;
        script.health = EditorGUILayout.Slider(script.health, 0f, 100f);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
        script.level = EditorGUILayout.IntSlider("Level", script.level, 1, 100);
        break;
    }

    switch (script.type) {
      case EntityType.Shatter:
        script.material = EditorGUILayout.ObjectField("Material", script.material, typeof(Material), true) as Material;
        break;
    }
  }
}
#endif
