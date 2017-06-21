using UnityEngine;

﻿public enum EntityType {
	None = 0,
  Shatter,
  GeneralEnemy,
  MantaRay,
  Bomb,
	ElectricBomb, 
	Crab,
  MazeWall,
  SpawnMaze,
  SpawnBase,
};

public class EntityTypes : MonoBehaviour {
	public void NewEntity(GameObject gmOb, Vector3 range, Vector3 offset, GameObject target = null) {
		NewEntity(gmOb, range, offset, EntityType.None, target);
	}
	public void NewEntity(GameObject gmOb, EntityType type, GameObject target = null) {
		NewEntity(gmOb, Vector3.zero, Vector3.zero, type, target);
	}
	public void NewEntity(GameObject gmOb, Vector3 range, Vector3 offset, EntityType type, GameObject target) {
		Vector3 rangeCuboid = new Vector3(Random.Range(-range.x, range.x),
																			Random.Range(-range.y, range.y),
																			Random.Range(-range.z, range.z)) * 0.5f + transform.position + offset;

		var enemy = Instantiate(gmOb, rangeCuboid, Quaternion.identity);
		if (enemy.GetComponentInChildren<Gravity>() != null)
			enemy.GetComponentInChildren<Gravity>().target = target == null ? null : target.transform;
		if (enemy.GetComponent<Entity>() != null)
			enemy.GetComponent<Entity>().type = type;
	}
}