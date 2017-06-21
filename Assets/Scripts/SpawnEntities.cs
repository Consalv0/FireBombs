using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEntities : EntityTypes {
	public int quantity;
	public float secondsBetween;
	public GameObject target;
	public GameObject enemyPrefab;
	public Vector3 spawnPos;
	public Vector3 spawnCuboid;

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(transform.position + spawnPos, spawnCuboid);
		Gizmos.color = Color.gray;
		Gizmos.DrawWireSphere(transform.position + spawnPos, 0.1f);
	}

	void Start() {
		/* Make a simultaneus function that it will call a function that instantiate the given prefab in a random position range,
		 * if the instantiated gameObject contains the component Gravity then add a target */

		StartCoroutine(createEntity());
	}

	IEnumerator createEntity() {
    for (int i = 0; i < quantity; i++) {
      if (secondsBetween > 0)
        yield return new WaitForSeconds(secondsBetween);
			NewEntity(enemyPrefab, spawnCuboid, spawnPos, target);
    }
	}
}
