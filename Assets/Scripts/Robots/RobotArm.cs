using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArm : MonoBehaviour
{
	
	private bool cutter;
	private bool cutting;
	private float cutTime = 0f;
	public List<RobotJoint> joints;
	public Plant currentCrop;
	public int currentFruit;
	public List<float> gradients;
	public Transform desiredSpot;
	public float samplingDistance = 0.3f;
	public float lr = 7f;
	public float minDist = 0.03f;
	public float minSpeed = 0.25f;
	
	public bool harvesting;
	
	[System.Serializable]
	public class RobotJoint {
		
		public string name;
		public Vector3 axis;
		public int type; //2 = Grabber joint, 3 = Cutter joint
		public float input;
		public float maxDelta;
		public float min;
		public float max;
		public float curVal = 0f;
		public GameObject obj;
		
	}
	
	[System.Serializable]
	public class ArmMotion {
		public List<Vector2> moves;
	}
	
	
	public void HarvestCrop(Plant p) {
		Debug.Log("Harvesting!");
		harvesting = true;
		currentCrop = p;
		currentFruit = 0;
	}
	
    // Start is called before the first frame update
    void Start()
    {
        gradients = new List<float>();
		for(int i = 0; i < joints.Count; i++) {
			gradients.Add(0);
			if(joints[i].type == 3) {
				cutter = true;
			}
		}
    }
	
	//Idea: Get gradients and do gradient descent on all of the moveable joints to get the desiredSpot as close to the fruit as possible.
	//A form of inverse kinematics?
	float GetGradient(int i) {
		float output = 0f;
		
		if(currentCrop.fruits[currentFruit] != null) {
		
			Vector3 targetPos = currentCrop.fruits[currentFruit].transform.position;
			if(cutter) {
				targetPos += new Vector3(0f,0.03f,0f);
			}
			
			float f_x = Vector3.Distance(desiredSpot.position, targetPos);
			joints[i].obj.transform.Rotate(joints[i].axis * samplingDistance, Space.Self);
			float f_x_d = Vector3.Distance(desiredSpot.position, targetPos);
			joints[i].obj.transform.Rotate(joints[i].axis * -samplingDistance, Space.Self);
			output = (f_x - f_x_d) / samplingDistance;
		}
		
		return output;
	}

    // Update is called once per frame
    void Update()
    {
		
		if(harvesting) {
			
			if(cutTime >= 1.0f) {
				cutting = false;
				cutTime = 0;
				currentFruit += 1;
				if(currentFruit >= currentCrop.fruits.Count) {
					harvesting = false;
				}
			}
			
			for(int i = 0; i < joints.Count; i++) {
				if(joints[i].type < 2) {
					gradients[i] = GetGradient(i);
					float moveAmount = gradients[i] * lr;
					if(moveAmount < 0) {
						if(-moveAmount < minSpeed) {
							moveAmount = -minSpeed;
						}
					}
					if(moveAmount > 0) {
						if(moveAmount < minSpeed) {
							moveAmount = minSpeed;
						}
					}
					joints[i].input = moveAmount;
				}
			}
			
			if(currentCrop.fruits[currentFruit] != null) {
				
				Vector2 fruitPos = new Vector2(currentCrop.fruits[currentFruit].transform.position.x,currentCrop.fruits[currentFruit].transform.position.z);
				Vector2 cropPos = new Vector2(currentCrop.transform.position.x, currentCrop.transform.position.z);
				
				float fruitDist = Vector2.Distance(transform.parent.transform.position, fruitPos);
				float cropDist = Vector2.Distance(transform.parent.transform.position, cropPos);
				
				if(cropDist > fruitDist) {
					currentFruit += 1;
					if(currentFruit >= currentCrop.fruits.Count) {
						harvesting = false;
					}
				}
				
				if(harvesting) {
				
					Vector3 targetPos = currentCrop.fruits[currentFruit].transform.position;
					if(cutter) {
						targetPos += new Vector3(0f,0.03f,0f);
					}
					if(Vector3.Distance(desiredSpot.position, targetPos) <= minDist) {
						joints[joints.Count-1].input = 1.0f;
						cutting = true;
					}
					else {
						joints[joints.Count-1].input = -1.0f;
					}
				}
			}
			if(cutting) {
				cutTime += Time.deltaTime;
			}
			
		}
		
        for(int i = 0; i < joints.Count; i++) {
			if(joints[i].input != 0) {
				
				if(joints[i].type == 0 || joints[i].type == 1) {
				
					joints[i].obj.transform.Rotate(joints[i].axis * joints[i].input * joints[i].maxDelta * Time.deltaTime, Space.Self);
					
				}
				
				if(joints[i].type == 2) {
					
					joints[i].curVal += joints[i].input * joints[i].maxDelta * Time.deltaTime;
					if(joints[i].curVal < 0) {
						joints[i].curVal = 0;
					}
					if(joints[i].curVal > 1) {
						joints[i].curVal = 1;
					}
					joints[i].obj.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, joints[i].curVal * 100f);;
					joints[i].obj.transform.GetComponent<Gripper>().val = joints[i].curVal;
					
				}
				
				if(joints[i].type == 3) {
					
					joints[i].curVal += joints[i].input * joints[i].maxDelta * Time.deltaTime;
					if(joints[i].curVal < 0) {
						joints[i].curVal = 0;
					}
					if(joints[i].curVal > 1) {
						joints[i].curVal = 1;
					}
					joints[i].obj.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, joints[i].curVal * 100f);;
					joints[i].obj.transform.GetComponent<Snipper>().val = joints[i].curVal;
					
				}
				
				joints[i].input = 0;
			}
		}
    }
}
