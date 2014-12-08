using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Direction { Up, Down, Left, Right, Back, Forward, NoDirection }

public class Board : MonoBehaviour {

	public GameObject tilePrefab;
	
	public float BoardW;
	public float BoardH;

	public List<GameObject> cubes;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	public void Swap (GameObject cube1, GameObject cube2)
//	{
//		//TODO:
//	}

	public void RemoveCube (GameObject cube)
	{
		if( !cubes.Contains(cube)) {
			cubes.Remove(cube);
		}
	}

	public void AddCube (GameObject cube)
	{
		if( !cubes.Contains(cube)) {
			cubes.Add(cube);
		}
	}
	
	void OnTriggerEnter(Collider col) {
		if (col.tag == "Cube") {
			AddCube(col.gameObject);
		}
	}
	
	
	void OnTriggerExit(Collider col) {
		if (col.tag == "Cube" ) {
			RemoveCube(col.gameObject);
		}
	}
}
