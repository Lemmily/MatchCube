using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour {
	
	public Transform target;

	BoardManager _boardManager;

	float rotateTime = 0.75f;
	float rotateDegrees = 90.0f;
	private bool rotating = false;

	bool input;	

	// Use this for initialization
	void Start () {
		_boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
	}
	
	void Update() {
		if(! input && !_boardManager.isMoving) {
			if (Input.GetAxis("Horizontal") > 0) {
				StartCoroutine(Rotate(transform, target, Vector3.back, rotateDegrees, rotateTime));
				input = true;
			}
			else if (Input.GetAxis("Horizontal") < 0) {
				StartCoroutine(Rotate(transform, target, Vector3.forward, rotateDegrees, rotateTime));
				input = true;
			}
			else if (Input.GetAxis("Vertical") > 0) {
				StartCoroutine(Rotate(transform, target, Vector3.right, rotateDegrees, rotateTime));
				input = true;
			}
			else if (Input.GetAxis("Vertical") < 0) {
				StartCoroutine(Rotate(transform, target, Vector3.left, rotateDegrees, rotateTime));
				input = true;
			}
			
			
//			if (Input.GetMouseButtonDown(0)){ // if left button pressed...
//				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//				RaycastHit hit;
//				if (Physics.Raycast(ray, out hit)){
//					Debug.Log (hit.transform.gameObject.GetComponent<Cube>());
//				}
//			}
				
			if(!rotating){
				input = false;
			}
		}
	}
	
	IEnumerator Rotate (Transform thisTransform , Transform otherTransform,Vector3 rotateAxis,float degrees,float totalTime) {
		if (rotating) yield return false;
		rotating = true;
		_boardManager.isRotating = true;
		
		var startRotation = thisTransform.rotation;
		var startPosition = thisTransform.position;
		transform.RotateAround(otherTransform.position, rotateAxis, degrees);
		var endRotation = thisTransform.rotation;
		var endPosition = thisTransform.position;
		thisTransform.rotation = startRotation;
		thisTransform.position = startPosition;
		
		var rate = degrees/totalTime;
		for (float i = 0.0f; i < degrees; i += Time.deltaTime * rate) {
			yield return new WaitForSeconds(0.001f);
			thisTransform.RotateAround(otherTransform.position, rotateAxis, Time.deltaTime * rate);
		}
		
		thisTransform.rotation = endRotation;
		thisTransform.position = endPosition;
		rotating = false;
		_boardManager.isRotating = false;
		input = false;
	}
}