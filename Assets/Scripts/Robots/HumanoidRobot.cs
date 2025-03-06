using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidRobot : MonoBehaviour

{
	
	float curTime = 0f;
	public int NN_x = 0; //how many hidden layers
	public int NN_y = 0; //how many 
	
	[System.Serializable]
	public class HumanoidJoint {
		public GameObject gameObject;
		public Rigidbody rb;
	};
	
	
	public HumanoidJoint[] joints;
	
	public void GetAllJoints() {
		
		List<HumanoidJoint> j = new List<HumanoidJoint>();
		List<GameObject> objs = GetChildren(gameObject);
		foreach(GameObject o in objs) {
			if(o.GetComponent<Rigidbody>()) {
				HumanoidJoint newJoint = new HumanoidJoint();
				newJoint.gameObject = o;
				newJoint.rb = o.GetComponent<Rigidbody>();
				j.Add(newJoint);
			}
		}
		
		joints = j.ToArray();
		
	}
	
	public List<GameObject> GetChildren(GameObject o) {
		List<GameObject> output = new List<GameObject>();
		
		output.Add(o);
		
		foreach(Transform child in o.transform) {
			output.AddRange(GetChildren(child.gameObject));
		}
		
		return output;
	}
	
    // Start is called before the first frame update
    void Start()
    {
        GetAllJoints();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		curTime += Time.deltaTime;
        for(int i = 1; i < joints.Length; i++) {
			joints[i].rb.AddTorque(0,0,0);
		}
    }
}
