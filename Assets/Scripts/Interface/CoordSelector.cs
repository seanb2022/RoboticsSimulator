using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class CoordSelector : MonoBehaviour
{
	
	public class CoordinateEntry {
		
		public string name;
		public Vector2 coords;
		
		public CoordinateEntry(string n, float x, float y) {
			name = n;
			coords = new Vector2(x,y);
		}
		
	}
	
	public Button b;
	public Button selectRobotButton;
	public TMP_InputField lat;
	public TMP_InputField lng;
	public Dropdown dropdown;
	private string dataPath;
	private string coordListPath;
	public List<CoordinateEntry> locations;
	public GameObject ContextMenu;
	public TMP_Text curRobotDisplay;
	public string currentLocation;
	
	
    // Start is called before the first frame update
    void Start()
    {
		
		dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
		
		locations = new List<CoordinateEntry>();
		
		dataPath = Application.dataPath;
		coordListPath = dataPath + "/Data/locations.csv";
		//Debug.Log(Application.dataPath);
        b.onClick.AddListener(LoadCoords);
		selectRobotButton.onClick.AddListener(SelectRobot);
		
		StreamReader reader = new StreamReader(coordListPath); 
        string fData = reader.ReadToEnd();
		int line = 0;
		foreach(string f in fData.Split('\n')) {
			if(line > 0) {
				string[] fentries = f.Split(',');
				CoordinateEntry newC = new CoordinateEntry(fentries[0], float.Parse(fentries[1]), float.Parse(fentries[2]));
				locations.Add(newC);
			}
			line += 1;
		}
        reader.Close();
		
		foreach(CoordinateEntry e in locations) {
			
			Dropdown.OptionData o = new Dropdown.OptionData();
			o.text = e.name;
			dropdown.options.Add(o);
			
		}
		
		if(locations.Count > 0) {
			lat.text = locations[0].coords.x.ToString();
			lng.text = locations[0].coords.y.ToString();
			dropdown.captionText.text = locations[0].name;
		}
		
    }

    // Update is called once per frame
    void Update()
    {
        curRobotDisplay.text = "Current Robot: " + PathMaker.Instance.selectedRobot.robot.GetComponent<DebugRover>().robotName;
    }
	
	public void SelectRobot() {
		
		PathMaker.Instance.mainCam.GetComponent<MainCam>().mode = 3;
		PathMaker.Instance.mainCam.GetComponent<MainCam>().currentFocusPoint = Vector3.zero;
		PathMaker.Instance.mainCam.GetComponent<MainCam>().camPos = new Vector3(4,2,0);
		gameObject.SetActive(false);
		
	}
	
	public void LoadCoords() {
		
		float _lat = float.Parse(lat.text);
		float _lng = float.Parse(lng.text);
		
		//PathMaker.Instance.map.SetPosition(_lng, _lat);
		PathMaker.Instance.mapCoords = new Vector2(_lng,_lat);
		PathMaker.Instance.LoadSubscene(currentLocation);
		PathMaker.Instance.mapReady = true;
		
		gameObject.SetActive(false);
		ContextMenu.SetActive(true);
	}
	
	void DropdownValueChanged(Dropdown d)
    {
        //Debug.Log(locations[d.value].name);
		lat.text = locations[d.value].coords.x.ToString();
		lng.text = locations[d.value].coords.y.ToString();
		currentLocation = locations[d.value].name;
    }
}
