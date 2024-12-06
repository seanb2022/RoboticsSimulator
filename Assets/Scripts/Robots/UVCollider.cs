using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVCollider : MonoBehaviour
{
	
	public DebugRover rover;
	
    // Start is called before the first frame update
    void Start()
    {
        rover = transform.parent.gameObject.GetComponent<DebugRover>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	private void OnTriggerEnter(Collider other) {
		
		if(other.gameObject.layer == 6) {
			
			other.gameObject.GetComponent<Plant>().killingMold = true;
			other.gameObject.GetComponent<Plant>().rover = rover;
			
		}
		
	}
	
	private void OnTriggerExit(Collider other) {
		
		if(other.gameObject.layer == 6) {
			
			other.gameObject.GetComponent<Plant>().killingMold = false;
			
		}
		
	}
	
}
