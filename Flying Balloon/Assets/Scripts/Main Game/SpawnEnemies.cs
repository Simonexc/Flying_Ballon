using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {

	public float spawnRate = 2f; // in seconds
	public Vector2 spawnRange = new Vector2 (-3f, 3f);

	public GameObject EnemyPrefab;

	private float timePassed;

	void Start () {
		timePassed = spawnRate;
	}

	void Update () {
		if (!GameControl.instance.gamePaused) {
			timePassed -= Time.deltaTime;

			if (timePassed <= 0) {
				Instantiate (EnemyPrefab, new Vector3 (10f, Random.Range (spawnRange.x, spawnRange.y)), Quaternion.identity);
				timePassed = spawnRate;
			}
		}
	}
}
