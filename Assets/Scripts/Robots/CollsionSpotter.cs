using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollsionSpotter : MonoBehaviour
{
	
	private int collisions = 0;
	public DebugRover rover;
	
    // Start is called before the first frame update
    void Start()
    {
        rover = transform.parent.gameObject.GetComponent<DebugRover>();
    }

    // Update is called once per frame
    void Update()
    {
        if(collisions > 0) {
			rover.blocked = true;
		}
		else {
			rover.blocked = false;
		}
    }
	
	private void OnTriggerEnter(Collider other) {
		
		if(other.gameObject.layer == 8) {
			
			collisions++;
			
		}
		
	}
	
	private void OnTriggerExit(Collider other) {
		
		if(other.gameObject.layer == 8) {
			
			collisions--;
			
		}
		
	}
	
}
