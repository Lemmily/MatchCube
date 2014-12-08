using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinMarker : MonoBehaviour {
	private BoardManager _board;

	public Cube player;
	public Cube winCube;
	
	private bool notifiedWin = false;
	
	public GameObject winNotification;
		
	void Start() {
		_board = GetComponentInChildren<BoardManager>();
	}	
	
	void Update() {
		if(_board.hasWon && ! notifiedWin) {
			GameObject.Instantiate(winNotification,new Vector3(), Quaternion.identity);
			_board.hasWon = true;
			notifiedWin = true;
		};
	}
}
