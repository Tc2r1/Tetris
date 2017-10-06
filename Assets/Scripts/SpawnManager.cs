using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	// Groups
	public GameObject[] groups;


	// Use this for initialization
	void Start () {
		// Spawn initial Group
		spawnNext();

	}

	// Update is called once per frame
	void Update () {

	}

	public void spawnNext(){

		// Random index
		int i = Random.Range(0, groups.Length);

		// Spawn Group at current Position

		Instantiate(groups[i], transform.position, Quaternion.identity);

	}
}
