using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {

	public static bool letGoEnemies = false; // can enemies be spawned

	public Vector2 spawnRate = new Vector2(1.5f, 3); // the rate at which enemies are being spawned - random value between x(min) and y(max)(in seconds)
	public Vector2 spawnRange = new Vector2 (-3f, 3f); // spawn y-position range

	public GameObject EnemyPrefab;

	private float timeLeft; // how much time left to next spawn

	void Start () {
		letGoEnemies = false; // stop spawning enemies
		timeLeft = Random.Range (spawnRate.x, spawnRate.y); // time left to spawn an enemy
	}

	void Update () {
        //Debug.Log(letGoEnemies);
		if (letGoEnemies) { // if enemies can be spawned
			timeLeft -= Time.deltaTime; // decrease "timeLeft" by time it took to render the frame

			if (timeLeft <= 0) { // if enough time has passed
				Instantiate (EnemyPrefab, new Vector3 (14f, Random.Range (spawnRange.x, spawnRange.y)), Quaternion.identity); // spawn an enemy
				timeLeft = Random.Range (spawnRate.x, spawnRate.y);
			}
		}
	}
}
