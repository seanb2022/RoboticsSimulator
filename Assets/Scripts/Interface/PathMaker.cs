using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PathMaker : MonoBehaviour
{
	
	public class Waypoint {
		public Vector2 pos;
		public bool light;
		public bool checkWater;
		public bool aimOnly;
		public bool pickFruit;
		public GameObject plant;
	};
	
	public static PathMaker Instance;
	public DebugRover rover;
	public RoverControls roverControls;
	
	public string currentLocationName;
	
	[System.Serializable]
	public class Subscene {
		public string name;
		public GameObject prefab;
	};
	
	public List<Subscene> subscenes;
	
	public List<Waypoint> waypoints;
	//public List<bool> lightPoints;
	public bool loaded;
	public GameObject waypointPrefab;
	public GameObject currentWayPointObj;
	//public bool lightWayPoint;
	
	public Vector2 mapCoords;
	
	public string mqttIp = "127.0.0.1";
	
	public bool mapReady;
	
	public OnlineMaps map;
	
	public MainCam mainCam;
	public GameObject placeCamOrg;
	public GameObject coordMenu;
	
	public Manip manip;
	public GameObject plotControls;
	
	public List<GameObject> cropList;
	
	public GameObject selectedCrop;
	public int selectedCropId;
	public RobotInfo selectedRobot;
	
	public bool VR;
	
	
	public float weedDensity;
	public float xDensity;
	public float yDensity;
	
    // Start is called before the first frame update
    void Start()
    {
		
		Instance = this;
        waypoints = new List<Waypoint>();
		Instance.selectedRobot = GetComponent<SelectMenu>().robotPrefabs[0].GetComponent<RobotInfo>();
    }
	
	public Waypoint GetNextWaypoint(Vector3 pos) {
		Vector2 pos2 = new Vector2(pos.x,pos.z);
		int least = 0;
		for(int i = 0; i < waypoints.Count; i++) {
			
			if(Vector2.Distance(pos2, waypoints[i].pos) < Vector2.Distance(pos2, waypoints[least].pos)) {
				least = i;
			}
			
		}
		
		least = 0;
		
		Waypoint le = waypoints[least];
		if(currentWayPointObj != null) {
			Destroy(currentWayPointObj);
		}
		currentWayPointObj = Instantiate(waypointPrefab);
		currentWayPointObj.transform.position = new Vector3(le.pos.x,0,le.pos.y);
		waypoints.RemoveAt(least);
		
		return le;
	}
	
	public void LoadSubscene(string n) {
		currentLocationName = n;
		foreach(Subscene s in subscenes) {
			if(s.name == n) {
				GameObject newSubscene = Instantiate(s.prefab);
				newSubscene.transform.position = Vector3.zero;
			}
		}
	}
	
    // Update is called once per frame
    void Update()
    {
		
		
		
		//Debug.Log(rStick);
		
		//Debug.Log(controls.Gameplay.rightStick.ReadValue<Vector2>());
		
		/*
		if(Input.GetKeyDown("c")) {
			selectedCropId += 1;
			if(selectedCropId >= cropList.Count) {
				selectedCropId = 0;
			}
		}
		*/
		
        selectedCrop = cropList[selectedCropId];
    }
}
