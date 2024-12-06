using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//using System.Time;

public class DebugRover : MonoBehaviour
{

	public string robotName; // Name of the robot to be displayed
	public int robotType; //0 = Moisture only, 1 = Weed killer, 2 = Grabber
	public Rigidbody rb;  //?
	public float moveSpeed = 4f; //move setting
	public float turnSpeed; //turn speed setting
	public Camera camera;   //camera class
	public RobotCamera rCam; // robot camera class seperate from camera class?
	public bool recording; // evaluates whether or not its recording
	public string curRecordDir;  // current recording directory
	public int frame; //frame number
	public MapInfo mapInfo; //class map info
	public MqNode mqNode; //connects robot components together 
	public float longitude; //latitude coordinates
	public float latitude; // longitude coordinates
	public float heading; // compass reading
	public float vel; // velocity 
	public float charge;
	public float powerUsage;
	public List<AxleInfo> axles; //list of axles
	public PathMaker.Waypoint currentWaypoint;   //attribute of pathmaker attribute named current waypoint 
	public Vector2 currentWaypointLATLNG; // vector of current lat/lang
	public GameObject camOrg; //?
	public bool wantsToTeleport;
	public RobotArm robotArm;
	public int selectedJoint = -1;
	
	public bool selfDriving;
	private float turnInput;
	private float forwardInput;
	private float probeInput;
	
	public bool netControlled;
	public float netForwardInput;
	public float netTurnInput;
	public float netLightInput;
	
	private float minDistance = 0.19f;
	private float checkDistance = 0.2f;
	private float closeSpeed = 0.3f;
	private float cruiseSpeed = 5f;
	
	private float maxLightTime = 5f;
	private float lightTime;
	
	public float torqueVal = 5f;
	
	public GameObject uvLight;
	public bool lightOn;
	
	private bool oneLineRecord = false;
	private StreamWriter infoFile;
	private bool gotWaypoint;
	
	public Text outGui;
	
	public float recordSpeed;
	public bool scienceQA;
	private float recordTime;
	
	private bool triggerDown;
	
	private string dataHeader = "Forward, Turn, Light, Latitude, Longitude, Heading, Velocity, WaypointLatitude, WaypointLongitude\n";
	private string lmHeader = "Data:\n";
	
	public int lightMode = 1; //0 = Shine light when close to a waypoint, 1 = shine light along path to next node
	
	public bool blocked;
	private float blockTime;
	
	public Transform probe;
	public float measuredWater;
	public float timeSinceMeasure;
	
	//Used for using unity as a "display," where the physical gps will update the virtual rover's position, rotation, etc
	public void Sync(float _longitude, float _latitude, float _heading) {
		
		Vector2 _dPos = mapInfo.CoordsToPos(new Vector2(_latitude, _longitude));
		transform.position = new Vector3(_dPos.x, transform.position.y, _dPos.y);
		
	}
	
	public void TestPlant() {
		RaycastHit hit;
		//instantiates a raycast object
		timeSinceMeasure = 0;
		
		int layer = 6;
		//
		
		int layerMask = 1 << layer;   // means take 1 and rotate it left by "layer" bit positions
		
		//layerMask = ~layerMask;
		
		if (Physics.Raycast(probe.position, transform.TransformDirection(probe.forward), out hit, 10, layerMask))
		{
			Plant p = hit.collider.gameObject.GetComponent<Plant>();
			measuredWater = p.water;
			Debug.Log(measuredWater);
			
		}
	}
	
	//Returns a string of the current information about the rover, which is writtern to a frame .txt file
	//If scienceQA is true, it will be output in the form of a series of sentences about the rovers information
	//Otherwise, the rovers current inputs, lat, lng, etc will be output as a series of plaintext numbers.
	private string GetInfo(string delimeter) {
		
		if(scienceQA) {
			
			
			Vector2 lp = new Vector2(Mathf.Infinity, Mathf.Infinity);
			if(currentWaypoint != null) {
				lp = mapInfo.PosToCoords(new Vector3(currentWaypoint.pos.x, 0, currentWaypoint.pos.y));
			}
			
			string question = "Question: What actions should the rover perform given the current image and situation?";
			
			string context = "Context: ";
			context += "The current latitude is " + latitude.ToString("F13") + " degrees, and the longitude is " + longitude.ToString("F13") + " degrees.";
			context += " The current heading is " + heading.ToString("F13") + " degrees, with a velocity of " + vel + " meters per second.";
			context += " The waypoints latitude is " + lp.y.ToString("F13") + " degrees, and the longitude of the waypoint is " + lp.x.ToString("F13") + " degrees.";
			
			string answer = "Answer: ";
			if(forwardInput > 0) {
				answer += "Accelerate forward. ";
			}
			if(forwardInput < 0) {
				answer += "Accelerate backward. ";
			}
			if(turnInput > 0) {
				answer += "Turn left. ";
			}
			if(turnInput < 0) {
				answer += "Turn right. ";
			}
			if(lightOn) {
				answer += "Turn light on.";
			}
			else {
				answer += "Turn light off.";
			}
			
			string explanation = "Explanation: ";
			if(forwardInput > 0) {
				explanation += "We accelerate forward, because the rover is going slower than either the cruise speed, or the speed when it is close to the waypoint.";
			}
			else if(forwardInput < 0) {
				explanation += "We accelerate backwards, because the rover is going faster than the cruise speed or speed when it is close to the waypoint. ";
			}
			else {
				explanation += "The rover is going faster than the desired speed, thus it does not accelerate.";
			}
			if(turnInput > 0) {
				explanation += " The rover is turning left, because the rover is not facing the waypoint, and the waypoint is to the rovers left.";
			}
			else if(turnInput < 0) {
				explanation += " The rover is turning right, because the rover is not facing the waypoint, and the waypoint is to the rovers right.";
			}
			else {
				explanation += " The rover is not turning, because it is currently facing the waypoint.";
			}
			
			if(lightOn) {
				explanation += " The rover turns its light on, because it is driving in between the rows of soybean, or other crop.";
			}
			else {
				explanation += " The rover turns its light off, because it is not driving in between the rows of soybean, or other crop.";
			}
			
			return context + '\n' + question + '\n' + answer + '\n' + explanation;
			
		}
		else {
		
			int l = 0;
			if(lightOn) {
				l = 1;
			}
			
			Vector2 lp = new Vector2(Mathf.Infinity, Mathf.Infinity);
			if(currentWaypoint != null) {
				lp = mapInfo.PosToCoords(new Vector3(currentWaypoint.pos.x, 0, currentWaypoint.pos.y));
			}
			currentWaypointLATLNG = lp;
			
			//return "" + Input.GetAxis("Vertical") + delimeter + Input.GetAxis("Horizontal") + delimeter + l + delimeter + latitude.ToString("F13") + delimeter + longitude.ToString("F13") + delimeter + heading.ToString("F13") + delimeter + vel;
			return "" + forwardInput + delimeter + turnInput + delimeter + l + delimeter + latitude.ToString("F13") + delimeter + longitude.ToString("F13") + delimeter + heading.ToString("F13") + delimeter + vel
			+ delimeter + lp.y.ToString("F13") + delimeter + lp.x.ToString("F13")
			;
		}
		return "Error getting input string.";
		
	}
	
	//Defunct self drive function
	private void SelfDriveUpdate() {
		
		forwardInput = 0;
		turnInput = 0;
		lightOn = false;
		
		if(currentWaypoint != null) {
			
			Vector2 rPos = new Vector2(transform.position.x, transform.position.z);
			
			float targetHeading = Mathf.Atan2(currentWaypoint.pos.x - rPos.x,
			currentWaypoint.pos.y - rPos.y
			) * Mathf.Rad2Deg;
			
			if(targetHeading < 0) {
				targetHeading += 360;
			}
			
			//Debug.Log(targetHeading);
			
			float deltaHeading = heading - targetHeading;
			if(deltaHeading > 180) {
				targetHeading += 360;
			}
			if(deltaHeading < -180) {
				heading += 360;
			}
			
			if(heading < targetHeading) {
				turnInput = 1;
			}
			if(heading > targetHeading) {
				turnInput = -1;
			}
			
			bool cruising = false;
			
			if(Vector2.Distance(rPos, currentWaypoint.pos) > 310) {
				cruising = true;
			}
			
			deltaHeading = Mathf.Abs(heading - targetHeading)/180;
			
			float closeMult = (Vector2.Distance(rPos, currentWaypoint.pos)/(minDistance*3))*5000*(deltaHeading/(Vector2.Distance(rPos, currentWaypoint.pos)))*
						(rb.velocity.magnitude + 0.03f);
			
			var localVelocity = transform.InverseTransformDirection(rb.velocity);
			
			float forwardSpeed = localVelocity.z;
			
			if(Vector2.Distance(rPos, currentWaypoint.pos) > minDistance) {
				
				if(cruising) {
					if(forwardSpeed < cruiseSpeed) {
						forwardInput = 1;
					}
				}
				else {
					if(forwardSpeed < closeSpeed) {
						
						float mult = closeMult;
						if(mult > 1) {
							mult = 1;
						}
						
						forwardInput = 1*mult;
					}
				}
			}
			else {
				
				currentWaypoint = PathMaker.Instance.GetNextWaypoint(transform.position);
				
				float mult = closeMult;
				if(mult > 1) {
					mult = 1;
				}
				//forwardInput = -1  * mult;
			}
			
			if(Vector2.Distance(rPos, currentWaypoint.pos) < minDistance+1.5f) {
				if(Mathf.Abs(heading - targetHeading) < 10) {
					lightOn = true;
				}
			}
			
			if(forwardSpeed < 0) {
				//turnInput *= -1;
			}
			
			deltaHeading = Mathf.Abs(heading - targetHeading)/30;
			if(deltaHeading > 1) {
				deltaHeading = 1;
			}
			turnInput *= deltaHeading;
			
			if(blocked) {
				blockTime = 3.5f;
			}
			
			if(blockTime > 0) {
				forwardInput *= -0.35f;
				turnInput = .35f;
				
				blockTime -= Time.deltaTime;
				
			}
			
			forwardInput = Mathf.Abs(forwardInput);
			
			//Debug.Log("" + heading + ", " + targetHeading + " | " + forwardInput + ", " + turnInput);
			
		}
		
	}
	
	//This function is performed every frame if the rover is set to drive to waypoints of its own volition in the simulation.
	private void SelfDriveUpdate_2() {
		
		if(netControlled) {
			forwardInput = netForwardInput;
			turnInput = netTurnInput;
			
			if(forwardInput > 1) {
				forwardInput = 1;
			}
			if(forwardInput < -1) {
				forwardInput = -1;
			}
			
			if(turnInput > 1) {
				turnInput = 1;
			}
			if(turnInput < -1) {
				turnInput = -1;
			}
			
			if(netLightInput >= 0.5f) {
				lightOn = true;
			}
			else {
				lightOn = false;
			}
		}
		else {
			
			forwardInput = 0;
			turnInput = 0;
			lightOn = false;
			
			
			
			if(currentWaypoint != null) {
				
				Vector2 rPos = new Vector2(transform.position.x, transform.position.z);
				
				float targetHeading = Mathf.Atan2(currentWaypoint.pos.x - rPos.x,
				currentWaypoint.pos.y - rPos.y
				) * Mathf.Rad2Deg;
				
				if(targetHeading < 0) {
					targetHeading += 360;
				}
				
				//Debug.Log(targetHeading);
				
				float deltaHeading = heading - targetHeading;
				if(deltaHeading > 180) {
					targetHeading += 360;
				}
				if(deltaHeading < -180) {
					heading += 360;
				}
				
				deltaHeading = heading - targetHeading;
				
				
				if(currentWaypoint.light) {
					lightOn = true;
				}
				
				var localVelocity = transform.InverseTransformDirection(rb.velocity);
					
				float forwardSpeed = localVelocity.z;
				
				
				
				bool reached = false;
				if(currentWaypoint.checkWater) {
					if(currentWaypoint.aimOnly) {
						if(Mathf.Abs(deltaHeading) <= 1.2f) {
							reached = true;
						}
					}
					else {
						if(Vector3.Distance(rPos, currentWaypoint.pos) < checkDistance) {
							if(Mathf.Abs(deltaHeading) <= 1.2f) {
								reached = true;
							}
						}
					}
				}
				else {
					if(Vector3.Distance(rPos, currentWaypoint.pos) < minDistance) {
						reached = true;
					}
				}
				
				
				//Reached Waypoint
				if(reached) {
					if(currentWaypoint.checkWater) {
						TestPlant();
					}
					currentWaypoint = PathMaker.Instance.GetNextWaypoint(transform.position);
				}
				
				
				
				
				if(Mathf.Abs(deltaHeading) > 1.2f) {
					
					if(heading < targetHeading) {
						turnInput = 1;
					}
					if(heading > targetHeading) {
						turnInput = -1;
					}
					
					if(forwardSpeed > 0) {
						forwardInput = -1;
					}
					if(forwardSpeed < 0) {
						forwardInput = 1;
					}
					
				}
				else {
					
					
					if(forwardSpeed < closeSpeed) {
						forwardInput = 1;
					}
				}
			}
			
			if(blocked) {
				blockTime = 3.5f;
			}
			
			if(blockTime > 0) {
				forwardInput = -0.35f;
				turnInput = .35f;
				
				blockTime -= Time.deltaTime;
				
			}
		}
		
	}
	
	private void PlaceDown()
	{
		float initialHeight = 250f; // Start height for the raycast
		float stepHeight = 10f; // Step height for each progressive raycast
		int maxSteps = 50; // Maximum number of steps to attempt
		Vector3 rayPos = new Vector3(512, initialHeight, 512);
		RaycastHit hit;

		for (int i = 0; i < maxSteps; i++)
		{
			// Check if the raycast hits the terrain
			if (Physics.Raycast(rayPos, Vector3.down*stepHeight, out hit))
			{
				// Place the rover slightly above the hit point
				transform.position = new Vector3(rayPos.x, hit.point.y + 1.4f, rayPos.z);
				// Ensure the Rigidbody is properly settled
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				return;
			}
			// Decrease the raycast height for the next step
			rayPos.y -= stepHeight;
		}

    // If no hit is found after the maximum steps, log an error
    Debug.LogError("Failed to place the rover on the terrain.");
}
	private float timeSpawned = 0;
	private bool placed = false;
	
    // Start is called before the first frame update
    void Start()
    {
		PathMaker.Instance.rover = this;
		
		if(mqNode != null) {
			mqNode.PublishInfo(forwardInput, turnInput, lightOn);
		}
        rb = GetComponent<Rigidbody>();
		
    }

    // Update is called once per frame
    void Update()
    {
		
		
		
		if(wantsToTeleport) {
			
			if(PathMaker.Instance.waypoints.Count > 0) {
			
				wantsToTeleport = false;
				//Debug.Log("TELEPORTING...");
				
				rb.isKinematic = true;
				Vector2 tPos = PathMaker.Instance.waypoints[0].pos;
				transform.position = new Vector3(tPos.x, transform.position.y + 10.0f, tPos.y + 5.0f);
				rb.isKinematic = false;
				rb.MovePosition(new Vector3(tPos.x, transform.position.y + 10.0f, tPos.y + 5.0f));
			}
		}
		
		//Debug.Log("Connected: " + mClient.mqttClientConnected);
		
		timeSinceMeasure += Time.deltaTime;
		
		if(currentWaypoint != null) {
			currentWaypointLATLNG = mapInfo.PosToCoords(new Vector3(currentWaypoint.pos.x, 0, currentWaypoint.pos.y));
		}
		else {
			currentWaypointLATLNG = new Vector2(0,0);
		}
		
		if(timeSpawned >= 1.4f) {
			if(!placed) {
				placed = true;
				PlaceDown();
			}
		}
		else {
			timeSpawned += Time.deltaTime;
			Debug.Log("TS: " + timeSpawned);
		}
		
		if(!gotWaypoint) {
			if(PathMaker.Instance.loaded) {
				gotWaypoint = true;
				currentWaypoint = PathMaker.Instance.GetNextWaypoint(transform.position);
			}
		}
		
		
		
		charge -= (Time.deltaTime*powerUsage);
		
	vel = rb.velocity.magnitude;
        //float forwardF = Time.deltaTime * moveSpeed * Input.GetAxis("Vertical");
        //float sideF = Time.deltaTime * turnSpeed * Input.GetAxis("Horizontal");
		
		

	heading = transform.eulerAngles.y;
	
		powerUsage = 0;
		powerUsage += (Mathf.Abs(forwardInput) * Time.deltaTime * 0.5f);
		if(lightOn) {
			powerUsage += Time.deltaTime * 2.7f;
		}
        
	/*
        rb.AddForce(transform.forward * forwardF);
        //rb.AddForce(transform.right * sideF);
        rb.AddTorque(transform.up * sideF);
    	*/

		if(Input.GetMouseButtonDown(0) || 
		(!triggerDown && OVRInput.Get(OVRInput.RawButton.RIndexTrigger))) {
			triggerDown = true;
			if(lightOn) {
				lightOn = false;
				//uvLight.SetActive(false);
			}
			else {
				lightOn = true;
				//uvLight.SetActive(true);
			}
		}
		
		uvLight.SetActive(lightOn);
		
		
		if(!OVRInput.Get(OVRInput.RawButton.RIndexTrigger)) {
			triggerDown = false;
		}
		
		if(Input.GetKeyDown("[3]")) {
			Sync(30,30,0);
		}
		
    	if(Input.GetKeyDown("[5]")) {
    		if(recording) {
    			StopRecording();
				
    		}
    		else {
    			StartRecording();
    		}
    	}
    	
		
		/*
    	if(recording) {
		if(recordTime >= 1f/recordSpeed) {
    			rCam.CaptureFrame(curRecordDir);
				
				if(oneLineRecord) {
					infoFile.Write(GetInfo(", ")+'\n');
				}
				else {
					//Debug.Log(Application.dataPath + "/Recordings/" + curRecordDir);
					string moveOutput = GetInfo(", ");
					//File.WriteAll(Application.dataPath + "/Recordings/" + curRecordDir + "/" + frame + ".txt", moveOutput);
					
					StreamWriter writer = new StreamWriter(Application.dataPath + "/Recordings/" + curRecordDir + "/" + frame + ".txt", true);
					
					writer.Write(dataHeader);
					writer.Write(moveOutput);
					
					writer.Close();
				}
				frame++;
    			recordTime = 0;
		}
		recordTime += Time.deltaTime;
    	}
		*/
    	
    	if(mapInfo != null) {
    		//Debug.Log(mapInfo.PosToCoords(transform.position));
    		Vector2 lp = mapInfo.PosToCoords(transform.position);
    		longitude = lp.x;
    		latitude = lp.y;
    	}
	
		
		if(mqNode != null) {
			mqNode.PublishInfo(forwardInput, turnInput, lightOn);
		}
    
    }
	
	public void StartRecording() {
		string hdr = dataHeader;
		if(scienceQA) {
			hdr = lmHeader;
		}
		
		recording = true;
				
		curRecordDir = System.DateTime.Now.ToString("yyyy_MM_ddTHH_mm_ss", System.Globalization.CultureInfo.InvariantCulture);
		
		Directory.CreateDirectory(Application.dataPath + "/Recordings/" + curRecordDir);
		
		if(oneLineRecord) {
			infoFile = new StreamWriter(Application.dataPath + "/Recordings/" + curRecordDir + "/" + "posInfo" + ".txt", true);
			infoFile.Write(hdr);
		}
	}
	public void StopRecording() {
		recording = false;
		if(infoFile != null) {
			infoFile.Close();
		}
	}
	
	public void ManualDriveUpdate() {
		
		//Robot arm controls
		selectedJoint = -1;
		if(robotArm != null) {
			for(int i = 0; i < 9; i++) {
				string c = (i+1).ToString();
				if(i < robotArm.joints.Count) {
					if(Input.GetKey(c)) {
						selectedJoint = i;
					}
				}
			}
		}
		
		
		if(PathMaker.Instance.VR) {
			forwardInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;
			turnInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
		}
		else {
			if(selectedJoint == -1) {
				forwardInput = Input.GetAxis("Vertical");
				turnInput = Input.GetAxis("Horizontal");
			}
			else {
				robotArm.joints[selectedJoint].input = Input.GetAxis("Vertical");
				forwardInput = turnInput = 0f;
			}
		}
		
	}
	
	public void FixedUpdate() {
		
		if(selfDriving) {
			//SelfDriveUpdate();
			SelfDriveUpdate_2();
		}
		else {
			ManualDriveUpdate();
		}
		
		float leftInput = forwardInput;
		float rightInput = forwardInput;
		
		//Debug.Log(forwardInput);
		
		leftInput += turnInput*1f;
		rightInput -= turnInput*1f;
		
		if(leftInput > 1) {
			leftInput = 1f;
		}
		if(rightInput > 1) {
			rightInput = 1f;
		}
		
		
		if(leftInput < -1) {
			leftInput = -1f;
		}
		if(rightInput < -1) {
			rightInput = -1f;
		}
		
		//Debug.Log("Forward " + forwardInput + "| Turn: " + leftInput + ", " + rightInput);
		
		//Wheels
		foreach(AxleInfo axle in axles) {
			axle.wheels[0].motorTorque = leftInput * moveSpeed;
			axle.wheels[1].motorTorque = rightInput * moveSpeed;
		}
		
		if(recording) {
			if(recordTime >= 1f/recordSpeed) {
					rCam.CaptureFrame(curRecordDir);
					
					if(oneLineRecord) {
						infoFile.Write(GetInfo(", ")+'\n');
					}
					else {
						
						string hdr = dataHeader;
						if(scienceQA) {
							hdr = lmHeader;
						}
						
						string moveOutput = GetInfo(", ");
						
						StreamWriter writer = new StreamWriter(Application.dataPath + "/Recordings/" + curRecordDir + "/" + frame + ".txt", true);
						
						writer.Write(hdr);
						writer.Write(moveOutput);
						
						writer.Close();
					}
					frame++;
					recordTime = 0;
			}
			recordTime += Time.deltaTime;
    	}
		
	}

}