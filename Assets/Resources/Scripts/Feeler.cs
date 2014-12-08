using UnityEngine;
using System.Collections;

public class Feeler : MonoBehaviour {

	public Cube owner;
	
	public Direction direction;
	
	void OnTriggerEnter(Collider col) {
		if (col.CompareTag("Cube")) {
			owner.AddCube(col.gameObject.GetComponent<Cube>(), direction);
		}
	}
		
	
	void OnTriggerExit(Collider col) {
		if (col.CompareTag("Cube" )) {
			owner.RemoveCube(col.gameObject.GetComponent<Cube>());
		}
	}
}
