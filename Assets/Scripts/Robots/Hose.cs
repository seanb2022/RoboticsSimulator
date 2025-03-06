using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hose : MonoBehaviour
{
	
	public Transform endPoint;
	public Transform endPointBone;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        endPointBone.position = endPoint.position;
    }
}
