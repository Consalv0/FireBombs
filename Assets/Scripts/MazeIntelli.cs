using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MazeIntelli : MonoBehaviour {
	public GameObject wallPrefab;
	public Vector3 wallSize;
	public Vector2 mazeSize;

  List<GameObject> walls = new List<GameObject>();

	Entity entityComponent;

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.green;
		// var centerMazePos = new Vector3(mazeSize.x * wallSize.x - wallSize.x, 0, mazeSize.y * wallSize.z - wallSize.z) * 0.5f;
		Gizmos.DrawWireCube(transform.position + wallSize * 0.5f, wallSize);
		Gizmos.DrawWireCube(transform.position, new Vector3(mazeSize.x * wallSize.x, wallSize.y, mazeSize.y * wallSize.z));
	}

	void Start() {
		mazeSize = new Vector2(Mathf.Abs(Mathf.Round(mazeSize.x)), Mathf.Abs(Mathf.Round(mazeSize.y)));
		var halfSize = new Vector3(mazeSize.x * wallSize.x, 0, mazeSize.y * wallSize.z) * 0.5f;
		for(int i = 0; i < mazeSize.x; i++) {
			for (int j = 0; j < mazeSize.y; j++) {
				if (i == (int)Mathf.Round(mazeSize.x * 0.5f) && j == (int)Mathf.Round(mazeSize.x * 0.5f)) continue;
				var wall = Instantiate(wallPrefab, new Vector3(i * wallSize.x, 0, j * wallSize.z) - halfSize + transform.position, Quaternion.identity);
				wall.GetComponent<Entity>().metaPos = new Vector2(i, j);
				wall.GetComponent<Entity>().wallSize = wallSize;
				if (Random.value > 0.5f)
					wall.GetComponent<Entity>().isActive = !wall.GetComponent<Entity>().isActive;
				walls.Add(wall);
      }
    }
		StartCoroutine(MoveMaze());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator MoveMaze() {
		while (true) {
			foreach (var wall in walls) {
				if (Random.value > 0.5f)
					wall.GetComponent<Entity>().isActive = !wall.GetComponent<Entity>().isActive;
			}
			yield return new WaitForSeconds(20);
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(MazeIntelli))]
public class MazeIntelliEditor : Editor {
	public override void OnInspectorGUI() {
		MazeIntelli script = (MazeIntelli)target;
		script.wallPrefab = EditorGUILayout.ObjectField("Wall", script.wallPrefab, typeof(GameObject), true) as GameObject;

		var entityComponent = script.GetComponent<Entity>();
		if (entityComponent != null) {
			script.mazeSize = entityComponent.mazeSize;
			entityComponent.mazeSize = EditorGUILayout.Vector2Field("Maze Size", entityComponent.mazeSize);
			entityComponent.mazeSize = new Vector2(Mathf.Abs(Mathf.Round(entityComponent.mazeSize.x)), Mathf.Abs(Mathf.Round(entityComponent.mazeSize.y)));
		} else {
			script.mazeSize = EditorGUILayout.Vector2Field("Maze Size", script.mazeSize);
		}
		script.wallSize = EditorGUILayout.Vector3Field("Wall Size", script.wallSize);
		script.mazeSize = new Vector2(Mathf.Abs(Mathf.Round(script.mazeSize.x)), Mathf.Abs(Mathf.Round(script.mazeSize.y)));
	}
}
#endif
