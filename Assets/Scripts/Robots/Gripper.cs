using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gripper : MonoBehaviour
{
	
	public float val;
	public float maxDelta;
	
	[System.Serializable]
	public class GripAppendage {
		
		public Vector3 axis;
		public float minVal;
		public float maxVal;
		public GameObject obj;
		
	}
	
	
	public List<GripAppendage> grips;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < grips.Count; i++) {
			grips[i].obj.transform.localRotation = Quaternion.Euler(grips[i].axis * (grips[i].minVal + ((grips[i].maxVal -grips[i]. minVal)*val)));
		}
    }
}
