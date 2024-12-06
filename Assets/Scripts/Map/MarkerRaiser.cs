using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerRaiser : MonoBehaviour
{
	
	public Transform camT;
	
    // Start is called before the first frame update
    void Start()
    {
        camT = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, camT.position.y,transform.position.z);
    }
}
