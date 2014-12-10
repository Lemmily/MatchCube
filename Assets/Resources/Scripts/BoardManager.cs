using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public class BoardManager : MonoBehaviour {

	public int CubeSize = 5;
	
	public GameObject cubePrefab;
//	public GameObject boardPrefab;
	public GameObject playerPrefab;
	
	public Bounds _bounds;
	
//	public List<Board> boards;
	public List<Cube> cubesList;
	public Cube[,,] cubes;
	
	Cube cube;

	float startTime;

	Vector3 cubePos;
	Vector3 playerPos;
	
	public int MatchNum = 3;

	public float SwapRate = 0.75f;

	public bool isMatched;
	public bool isMoving;
	public bool isRotating;
	
	public bool hasWon = false;
	
	public Cube player;

	Cube winCube;

	private float _matchTimeOut;

	void Start () {
		_bounds = transform.parent.renderer.bounds;
		print (_bounds);
		CreateCubes(CubeSize);
		PutPlayer((CubeSize-1)/2, CubeSize-1, (CubeSize-1)/2);
		print (_bounds);	
		transform.localPosition = new Vector3(-CubeSize/2 +0.5f, -CubeSize/2 + 0.5f, -CubeSize/2 + 0.5f);
		AssignWinCube();
	}
	
	// Update is called once per frame
	void Update () {
		//sanity check
		if(winCube.colour != "") {
			winCube.CreateWinCube();
		}
	
		if(!hasWon) {
			if(isMatched) {
			var toDestroy = new List<Cube>();
				for(int i = 0; i < cubesList.Count; i++) {
					if(cubesList[i] == null) {
						print ("uhm");
					} else {
						if(cubesList[i].isMatched) {
							toDestroy.Add(cubesList[i]);
						}
					}
				}
				for(int i = 0; i < toDestroy.Count; i++) {
					Destroy(toDestroy[i].gameObject, 0.25f);
					cubesList.Remove(toDestroy[i]);
				}
				isMatched = false;
			}
			else if(isMoving && cube != null) {
				if(player.transform.localPosition == cubePos &&
				   cube.transform.localPosition == playerPos) {
					
					isMoving = false;
					
					if(cube == winCube) {
						hasWon = true;
					} else {
						CheckMatch(cube);
					}
				} else {
					MoveCube(player, cubePos, playerPos);
					MoveCube(cube, playerPos, cubePos);
					
					if(Vector3.Distance(player.transform.localPosition, cubePos) < 0.1f ||
					   Vector3.Distance(cube.transform.localPosition, playerPos) < 0.1f) {
						player.transform.localPosition = cubePos;
						cube.transform.localPosition = playerPos;
					}	
				}
			}
			else if (isMoving && cube == null) {
				player.transform.position = playerPos;
				isMoving = false;
			}
		}
	}
	
	

	void CreateCubes (int num)
	{
		cubes = new Cube[CubeSize,CubeSize,CubeSize];
		for(int x = 0; x < num; x++){
			for(int y = 0; y < num; y++){
				for(int z = 0; z < num; z++){
					GameObject g = Instantiate(cubePrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
					g.transform.parent = gameObject.transform;
					g.transform.localPosition = new Vector3(x,y,z);
					cubes[x,y,z] = g.GetComponent<Cube>();
					cubesList.Add(cubes[x,y,z]);
					_bounds.Encapsulate(g.renderer.bounds);
				}
			}
		}	
	}

	void PutPlayer (int x, int y, int z)
	{
		var c = cubes[x,y,z];
		cubesList.Remove(cubes[x,y,z]);
		Destroy(c.gameObject);
		GameObject g = Instantiate(playerPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
		g.transform.parent = gameObject.transform;
		g.transform.localPosition = new Vector3(x,y,z);
		g.renderer.materials = playerPrefab.renderer.materials;
		player = g.GetComponent<Cube>();
		cubes[x,y,z] = player;
	}
	
	void AssignWinCube ()
	{
		winCube = cubes[(CubeSize-1)/2,(CubeSize-1)/2,(CubeSize-1)/2];
		var winScript = GetComponentInParent<WinMarker>();
		winScript.player = player;
		winScript.winCube = winCube;
	}
	
	
	public void CheckMatch(Cube cube) {
		List<Cube> MatchList = new List<Cube>();
		ConstructMatchList(ref MatchList, cube, cube.colour, cube.XCoord, cube.YCoord, cube.ZCoord);
		FilterMatchList(cube, MatchList);
	}
	
	public void ConstructMatchList(ref List<Cube> MatchList, Cube c, string colour, int x, int y, int z) {
		if( c == null || c.isMoving) {
			return;
		}
		else if ( c.colour != colour){
			return;
		}
		else if (MatchList.Contains(c)){
			return;
		}
		else {
			MatchList.Add (c);
			if(x == c.XCoord || y == c.YCoord || z == c.ZCoord) {
				for(int i = 0; i < c.Neighbours.Count; i++) {
					ConstructMatchList(ref MatchList, c.Neighbours[i], colour,x,y,z);
				}
			}
		}
	}
	
	
	public void FilterMatchList(Cube c, List<Cube> MatchList) {
		
		List<Cube> rows = new List<Cube>();
		List<Cube> columns = new List<Cube>();
		List<Cube> alongs = new List<Cube>();
		
		for (int i = 0; i < MatchList.Count; i ++) {
			if (c.ZCoord == MatchList[i].ZCoord && c.YCoord == MatchList[i].YCoord) {
				rows.Add(MatchList[i]);
			}
			if (c.XCoord == MatchList[i].XCoord && c.ZCoord == MatchList[i].ZCoord) {
				columns.Add(MatchList[i]);
			}
			if (c.YCoord == MatchList[i].YCoord && c.XCoord == MatchList[i].XCoord) {
				alongs.Add(MatchList[i]);
			}
			
		}
		
		if(rows.Count >= MatchNum) {
			for (int i = 0; i < rows.Count; i++) {
				rows[i].isMatched = true;
			}
			isMatched = true;
		}
		if(columns.Count >= MatchNum) {
			for (int i = 0; i < columns.Count; i++) {
				columns[i].isMatched = true;
			}
			isMatched = true;
		}
		if(alongs.Count >= MatchNum) {
			for (int i = 0; i < alongs.Count; i++) {
				alongs[i].isMatched = true;
			}
			isMatched = true;
		}
	}

	void MoveCube (Cube cube, Vector3 toPos, Vector3 fromPos)
	{
		Vector3 center = (fromPos + toPos) * 0.5f;
		Vector3 riseRelCenter = fromPos - center;
		Vector3 setRelCenter = toPos - center;
		float t = Time.time;
		float fracComplete = (t - startTime) / SwapRate;
		
		Vector3 tmp = Vector3.Lerp (riseRelCenter, setRelCenter, fracComplete);
		cube.transform.localPosition = tmp;
		cube.transform.localPosition += center;
	}
	
	public void SwapCubes(Cube player, Cube c) {
		if ( ! isMoving && ! isRotating) {
			playerPos = player.transform.localPosition;
			cubePos = c.transform.localPosition;
			cube = c;
			startTime = Time.time;
			isMoving = true;
		}
	}
	
	public void MovePlayer(Cube c) {
		if(player.Neighbours.Contains (c) ) {
			SwapCubes(player, c);
		}
	}
}
//	void CreateBoards (int num)
//	{
//		var boardSize = CubeSize*2f;
//		for(int x = 0; x < num; x++){
//			GameObject g = Instantiate(boardPrefab, new Vector3(x, 0, 0), Quaternion.identity) as GameObject;
//			g.transform.parent = gameObject.transform;
//			g.transform.localPosition = new Vector3(x,0,0);
//			var col = g.GetComponent<BoxCollider>();
//				col.size = new Vector3(0.5f,boardSize,boardSize);
//			boards.Add(g.GetComponent<Board>());
//		}
//		for(int y = 0; y < num; y++){
//			
//			GameObject g = Instantiate(boardPrefab, new Vector3(0, y, 0), Quaternion.identity) as GameObject;
//			g.transform.parent = gameObject.transform;
//			g.transform.localPosition = new Vector3(0,y,0);
//			var col = g.GetComponent<BoxCollider>();
//				col.size = new Vector3(boardSize,0.5f,boardSize);
//			boards.Add(g.GetComponent<Board>());
//		}
//		for(int z = 0; z < num; z++){
//			
//			GameObject g = Instantiate(boardPrefab, new Vector3(0, 0, z), Quaternion.identity) as GameObject;
//			g.transform.parent = gameObject.transform;
//			g.transform.localPosition = new Vector3(0,0,z);
//			var col = g.GetComponent<BoxCollider>();
//			col.size = new Vector3(boardSize,boardSize,0.5f);
//			boards.Add(g.GetComponent<Board>());
//		}
//	}
