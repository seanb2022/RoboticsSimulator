using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArm : MonoBehaviour
{
	
	public List<RobotJoint> joints;
	
	[System.Serializable]
	public class RobotJoint {
		
		public string name;
		public Vector3 axis;
		public int type;
		public float input;
		public float maxDelta;
		public float min;
		public float max;
		public float curVal = 0f;
		public GameObject obj;
		
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
