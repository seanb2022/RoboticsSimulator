using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using TMPro;

public class Manip : MonoBehaviour
{

	public GameObject pointer;
	public SelectMenu selectMenu;
	public MainCam cam;
	public GameObject curRobot;
	public GameObject selectedTerrain;
	public GameObject terrain;
	public Camera botCam;
	public bool freeCam;
	public PosDisplay pd;
	
	public GameObject fullCamScreen;
	private bool fullView;

	public InputDevice rightController;
	public InputDevice leftController;
	public InputDevice HMD;
	
	public GameObject testRob;
	
	public GameObject plotHelper;
	public GameObject plotHelperPrefab;
	public List<GameObject> plotHelpers;
	public bool placingPlot;

	public Vector2 plotTopLeft;
	public Vector2 plotBottomRight;
	
	private bool placeTL;
	private bool placeBR;
	
	public bool quickStart;
	public GameObject defaultRobot;
	private bool started;
	
	public GameObject weedDensityDisplay;
	public float weedDensity;
	
	private bool validPlot;
	
	public RectTransform cropPanel;
	
	public bool MouseInPlotPanel() {
		
		Vector3 mousePos = Input.mousePosition;
		//Debug.Log(cropPanel.sizeDelta);
		
		if(mousePos.x >= Screen.width - cropPanel.rect.width && mousePos.y <= cropPanel.rect.height) {
			//Debug.Log("INPANEL");
			return true;
		}
		
		return false;
	}

    // Start is called before the first frame update
    void Start()
    {
		
    }
	
	private void InitializeInputDevice(InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice) {
		
		List<InputDevice> devices = new List<InputDevice>();
		InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);
		
		if(devices.Count > 0) {
			inputDevice = devices[0];
		}
		
	}
	
	private void InitializeInputDevices() {
		
		if(!rightController.isValid) {
			InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref rightController);
		}
		if(!leftController.isValid) {
			InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref leftController);
		}
		if(!rightController.isValid) {
			InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.HeadMounted, ref HMD);
		}
		
	}
    
    public void StartRobot(RobotInfo robotInfo) {
		cam.mode = 1;
		Cursor.lockState = CursorLockMode.None;
    	cam.chaseCam = robotInfo.chaseCam;
    	curRobot = Instantiate(robotInfo.robot);
    	curRobot.transform.position = Vector3.zero;
    	cam.camPos = new Vector3(0.4f,1,-0.4f);
    	cam.targetPos = new Vector3(0,1.0f,0);
    	cam.focusedRobot = curRobot;
    	if(robotInfo.terrain) {
    		terrain = Instantiate(selectedTerrain);
    		curRobot.GetComponent<DebugRover>().mapInfo = terrain.GetComponent<MapInfo>();
    		curRobot.transform.position = curRobot.GetComponent<DebugRover>().mapInfo.spawn.position;
    		cam.camPos = curRobot.GetComponent<DebugRover>().mapInfo.spawn.position;
    	}
    	botCam = curRobot.GetComponent<DebugRover>().camera;
    	pd.rover = curRobot.GetComponent<DebugRover>();
		if(!PathMaker.Instance.VR) {
			cam.transform.position = curRobot.GetComponent<DebugRover>().camOrg.transform.position;
		}
		else {
			cam.cc.enabled = false;
			cam.transform.position = curRobot.GetComponent<DebugRover>().camOrg.transform.position;
			cam.cc.enabled = true;
		}
		
		PathMaker.Instance.placeCamOrg.transform.position = curRobot.transform.position + new Vector3(0,20,0);
		if(SocketInterace.Instance != null) {
			SocketInterace.Instance.Connect();
			SocketInterace.Instance.rover = curRobot.GetComponent<DebugRover>();
		}
    }

    // Update is called once per frame
    void Update()
    {
		
		if(PathMaker.Instance != null) {
			if(!started && PathMaker.Instance.mapReady) {
				if(quickStart) {
					started = true;
					StartRobot(PathMaker.Instance.selectedRobot.GetComponent<RobotInfo>());
					PathMaker.Instance.map = terrain.GetComponent<OnlineMaps>();
					PathMaker.Instance.map.SetPosition(PathMaker.Instance.mapCoords.x,PathMaker.Instance.mapCoords.y);
				}
			}
		}
		
		//Place plot
		if(Input.GetKeyDown("p")) {
			placingPlot = !placingPlot;
		}
		
		if(placingPlot) {
			
			if(Input.GetKeyDown("[+]")) {
				PathMaker.Instance.weedDensity += 0.5f;
			}
			if(Input.GetKeyDown("[-]")) {
				PathMaker.Instance.weedDensity -= 0.5f;
			}
			
			//weedDensityDisplay.SetActive(true);
			
			//weedDensityDisplay.GetComponent<TMP_Text>().text = "Weed Density: " + weedDensity;
			
			RaycastHit hit;
			
			if(Input.GetMouseButtonDown(0)) {
				
				if(!MouseInPlotPanel()) {
				
					validPlot = false;
					
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit, 4000f)) {
						
						plotTopLeft = new Vector2(hit.point.x, hit.point.z);
						plotHelper = Instantiate(plotHelperPrefab);
						plotHelper.GetComponent<PlotHelper>().plant = PathMaker.Instance.selectedCrop;
						plotHelpers.Add(plotHelper);
						//placeTL = true;
						placeBR = true;
						
						
						validPlot = true;
					}
				}
				
				
			}
			if(Input.GetMouseButtonUp(0) && validPlot) {
				
				placeTL = false;
				placeBR = false;
				
			}
			
			if(placeBR && validPlot) {
				
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit, 4000f)) {
					//Debug.Log (hit.transform.name);
					//Debug.Log ("hit");
				}
				
				plotBottomRight = new Vector2(hit.point.x, hit.point.z);
				//plotHelper = Instantiate(plotHelper);
				
			}
			
			if(plotHelper != null && validPlot) {
			
				plotHelper.transform.position = new Vector3((plotTopLeft.x+plotBottomRight.x)/2,500,(plotTopLeft.y+plotBottomRight.y)/2);
				plotHelper.GetComponent<PlotHelper>().size = new Vector2(Mathf.Abs(plotTopLeft.x-plotBottomRight.x)/2, Mathf.Abs(plotTopLeft.y-plotBottomRight.y)/2);
			
			}
			
			if(Input.GetKeyDown(KeyCode.Return)) {
				placingPlot = false;
				foreach(GameObject plot in plotHelpers) {
					plot.GetComponent<PlotHelper>().weedDensity = PathMaker.Instance.weedDensity;
					plot.GetComponent<PlotHelper>().PlacePlot();
				}
				while(plotHelpers.Count > 0) {
					GameObject p = plotHelpers[0];
					plotHelpers.RemoveAt(0);
					Destroy(p);
				}
			}
			
		}
		
		
		
        
		if(!rightController.isValid || !leftController.isValid || !HMD.isValid) {
			InitializeInputDevices();
		}
		
		if(Input.GetKeyDown("[0]")) {
			ToggleView();
		}
		
        //Mouseover
		/*
        RaycastHit hit1;
		if (Physics.Raycast(pointer.transform.position, pointer.transform.TransformDirection(Vector3.forward), out hit1, Mathf.Infinity))
		{
			if(hit1.collider.gameObject.tag == "SelectableRobot") {
				hit1.collider.GetComponent<RobotInfo>().hovering = true;
			}
		}
		*/
        
        if (Input.GetMouseButtonDown(0) || OVRInput.Get(OVRInput.RawButton.RIndexTrigger)) {
		
			/*
			RaycastHit hit;
			if (Physics.Raycast(pointer.transform.position, pointer.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
			{
				if(hit.collider.gameObject.tag == "SelectableRobot") {
						//Debug.Log("Did Hit");
						RobotInfo ri = hit.collider.GetComponent<RobotInfo>();
						StartRobot(ri);
						selectMenu.HideSelections();
						
						
					}
			}
			*/
			
			
        	
        }
        
        //Switch camera
        if(Input.GetKeyDown("space")) {
        	if(freeCam) {
        		freeCam = false;
        		cam.tag = "None";
        		botCam.tag = "MainCamera";
        		cam.enabled = false;
        		botCam.enabled = true;
        	}
        	else {
        		freeCam = true;
        		botCam.tag = "MainCamera";
        		cam.tag = "None";
        		cam.enabled = true;
        		botCam.enabled = false;
        	}
        }
        
    }
	
	public void ToggleView() {
		if(fullView) {
				fullView = false;
				fullCamScreen.SetActive(false);
		}
		else {
			fullView = true;
			fullCamScreen.SetActive(true);
			fullCamScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width,Screen.height);
		}
	}
}
