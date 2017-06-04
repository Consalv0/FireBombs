﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Entity : MonoBehaviour {
	[HideInInspector]
	public bool isAlive;
	[HideInInspector]
	public float life = 100;
	[HideInInspector]
	public int level = 1;

	void LateUpdate() {
		if (isAlive) {
			if (life < 0) {
				isAlive = !isAlive;
				GetComponent<Destroy>().Go();
			}
		}
	}

	public float TakeDamage(float percentage) {
		if (isAlive) {
			life = life - percentage / level;
			return life;
		}
		return life;
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(Entity))]
public class EntityEditor : Editor {
	public override void OnInspectorGUI() {
		Entity script = (Entity)target;

		EditorGUILayout.BeginHorizontal();
		script.isAlive = EditorGUILayout.Toggle(script.isAlive, GUILayout.MaxWidth(15.0f));
		EditorGUILayout.LabelField("Life", EditorStyles.boldLabel, GUILayout.MaxWidth(98.0f));
		GUILayout.FlexibleSpace();
		GUI.enabled = script.isAlive;
		script.life = EditorGUILayout.Slider(script.life, 0, 100);
		GUI.enabled = true;
		EditorGUILayout.EndHorizontal();

		script.level = EditorGUILayout.IntSlider("Level", script.level, 0, 100);
	}
}
#endif
