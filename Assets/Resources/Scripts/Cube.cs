using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube : MonoBehaviour {
	static public string[] colours={"Red","Yellow","Blue", "Green", "Orange", "Purple"};
	public string colour = "";
	
	public bool isMatched;
	public bool isMoving;
	
	
//	public Dictionary<Direction, Cube> Neighbour = new Dictionary<Direction, Cube>();
//	public Dictionary<Cube, Direction> RevNeighbour = new Dictionary<Cube, Direction>();
	
	public List<Cube> Neighbours = new List<Cube>();
	
	
	
	public int XCoord {
		get {
			return Mathf.RoundToInt(transform.localPosition.x);
		}
	}
	public int YCoord {
		get {
			return Mathf.RoundToInt(transform.localPosition.y);
		}
	}
	public int ZCoord {
		get {
			return Mathf.RoundToInt(transform.localPosition.z);
		}
	}
	
	void Start() {
		if(CompareTag("Cube")){
			CreateCube();
		}
	}
	
	void CreateCube ()
	{
		colour = colours [Random.Range (0, colours.Length)];
		Material m = Resources.Load ("Materials/" + colour) as Material;
		var mRend = gameObject.GetComponent<MeshRenderer>();
		var mats = mRend.materials;
		mats[0] = m;
		mRend.materials = mats;
	}
	public void CreateWinCube ()
	{
		colour = "";
		Material m = Resources.Load("Materials/WinCube") as Material;
		var mRend = gameObject.GetComponent<MeshRenderer>();
		var mats = mRend.materials;
		mats[0] = m;
		mRend.materials = mats;
	}
	
	
	
	public void AddCube (Cube t) //, Direction d)
	{	
		if( ! Neighbours.Contains(t)){
			Neighbours.Add (t);
		}
	}
	
	public void RemoveCube (Cube t)
	{
		if(Neighbours.Contains(t)) {
			Neighbours.Remove(t);
		}
		Neighbours.RemoveAll(item => item == null);
	}
	
	void OnMouseDown() {
		var b = (BoardManager)GameObject.Find("BoardManager").GetComponent(typeof(BoardManager));
		if(! b.isMoving) {
			b.MovePlayer(this);
		}
	}
	
	override public string ToString() {
		return colour + " - " + XCoord + "," + YCoord + "," + ZCoord;
	}
}
