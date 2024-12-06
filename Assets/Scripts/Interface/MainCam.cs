using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{

	public CharacterController cc;

    public float sensitivity;
    public int mode;
    public bool chaseCam;
    
	private float maxChaseDist = 6f;

    public Vector3 targetPos;
    public Vector3 camPos;
    public GameObject focusedRobot;
	public RobotInfo robotSelection;
	public Vector3 currentFocusPoint;
	
	public Transform camTransform;
	public Transform leftHandTransform;
	
	public float walkSpeed = 5f;
	
	public bool isVRCam;
	
	public bool initialized;
	
	public float height = 1.75f;
    
    // Start is called before the first frame update
    void Start()
    {
		currentFocusPoint = new Vector3(0,0,0);
		
    	//Cursor.lockState = CursorLockMode.Locked;
        camPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		
		
		if(PathMaker.Instance != null) {
			if(isVRCam) {
				if(!PathMaker.Instance.VR) {
					gameObject.SetActive(false);
				}
			}
			else {
				if(PathMaker.Instance.VR) {
					gameObject.SetActive(false);
				}
			}
		}
		
		if(Input.GetKeyDown("p")) {
			if(mode == 2) {
				mode = 1;
			}
			else {
				mode = 2;
				PathMaker.Instance.placeCamOrg.transform.position = focusedRobot.transform.position + new Vector3(0,15,0);
			}
		}
		
		if(mode == 1) {
			currentFocusPoint = Vector3.Lerp(currentFocusPoint, focusedRobot.transform.position, Time.deltaTime * 5.0f);
			transform.LookAt(currentFocusPoint);
		}
		
		if(mode == 2) {
			
			if(!PathMaker.Instance.VR) {
				if(Input.GetKey("up")) {
					PathMaker.Instance.placeCamOrg.transform.position += new Vector3(0,0,5) * Time.deltaTime;
				}
				if(Input.GetKey("down")) {
					PathMaker.Instance.placeCamOrg.transform.position += new Vector3(0,0,-5) * Time.deltaTime;
				}
				
				if(Input.GetKey("left")) {
					PathMaker.Instance.placeCamOrg.transform.position += new Vector3(-5,0,0) * Time.deltaTime;
				}
				if(Input.GetKey("right")) {
					PathMaker.Instance.placeCamOrg.transform.position += new Vector3(5,0,0) * Time.deltaTime;
				}
				
				PathMaker.Instance.placeCamOrg.transform.position += Input.mouseScrollDelta.y * Time.deltaTime * transform.forward * 50f;
				
			}
			
			if(Input.GetKeyDown(KeyCode.Return)) {
				mode = 1;
			}
		}
		
		//Select robot mode
		if(mode == 3) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                //Debug.Log(hit.transform.name);
                //Debug.Log("hit");
				RobotInfo ri = hit.collider.gameObject.GetComponent<RobotInfo>();
				if(ri != null) {
					ri.hovering = true;
					
					if(Input.GetMouseButtonDown(0)) {
						PathMaker.Instance.selectedRobot = ri;
						mode = 0;
						transform.position = Vector3.zero;
						PathMaker.Instance.coordMenu.SetActive(true);
					}
				}
            }
			transform.LookAt(currentFocusPoint);
		}
		
		if(!isVRCam) {
			transform.position = Vector3.Lerp(transform.position, camPos, 0.6f*Time.deltaTime);
		}
    	//float forwardF = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y * moveSpeed;
		//float sideF = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x * turnSpeed;
		Vector2 mv = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
		
		
		Vector3 cForward = Vector3.zero;
		
		if(isVRCam) {
			cForward = leftHandTransform.transform.forward;
			cForward = new Vector3(cForward.x, 0, cForward.z);
			cForward.Normalize();
			
			Vector3 sideMv = Quaternion.Euler(0, 90, 0) * cForward;
			
			//Debug.Log(cForward);
			
			//transform.position += mv.y*cForward * Time.deltaTime;
			
			cc.Move(mv.y*cForward * Time.deltaTime * walkSpeed);
			cc.Move(mv.x*sideMv * Time.deltaTime * walkSpeed);
			cc.Move(-transform.up * Time.deltaTime);
			
			/*
			RaycastHit hit;
			if (Physics.Raycast(transform.position + new Vector3(0,height,0), transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
			{
				transform.position = new Vector3(transform.position.x,
				hit.point.y - height,
				transform.position.z);
			}
			*/
		}
    	
        if(mode == 0) {
        	Vector3 lookRot = transform.localRotation.eulerAngles;
        	lookRot.y += Input.GetAxis("Mouse X") *Time.deltaTime * sensitivity;
        	lookRot.x -= Input.GetAxis("Mouse Y") *Time.deltaTime * sensitivity;
        	//transform.localRotation = Quaternion.Euler(lookRot);
        }
		else if(mode == 2) {
			
				targetPos = PathMaker.Instance.placeCamOrg.transform.position;
				
				transform.position = targetPos;
				/*
				if(!isVRCam) {
					transform.position = Vector3.Lerp(transform.position, targetPos, 0.01f);
				}
				*/
		}
        else {
        	if(chaseCam) {
        		if(focusedRobot != null) {
        			//targetPos = focusedRobot.transform.position;
					targetPos = focusedRobot.GetComponent<DebugRover>().camOrg.transform.position;
        		}
				
				if(!isVRCam) {
					//transform.position = Vector3.Lerp(transform.position, targetPos, 0.01f);
					transform.position = targetPos;
				}
				//Vector3 _off = new Vector3(0,2,0);
				
				/*
				Vector3 _off = Vector3.zero;
				if(Vector3.Distance(targetPos + _off, transform.position) > maxChaseDist) {
					transform.position = Vector3.Lerp(transform.position, targetPos + _off, 0.01f);
				}
				*/
        	}
    		//Quaternion desiredRotation = Quaternion.LookRotation(targetPos - transform.position);
			if(focusedRobot != null) {
				Quaternion desiredRotation = focusedRobot.transform.rotation;
			}
			
    		//transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 0.9f * Time.deltaTime);
        }
    }
}
