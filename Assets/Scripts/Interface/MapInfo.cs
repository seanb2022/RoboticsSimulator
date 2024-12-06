using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
	
	public float latitude;
	public float longitude;
	public Transform spawn;
	private float circumference = 40075000f;
	
	public Vector2 PosToCoords(Vector3 pos) {
		float lat = 0;
		float lng = 0;
		
		float cDeg = circumference/360f;
		
		
		lat = latitude + (pos.z/cDeg);
		lng = longitude + (pos.x/((Mathf.Cos((latitude/180f)*Mathf.PI))*cDeg));

		//lat = latitude;
		//lng = longitude;
		
		return new Vector2(lng, lat);
	}
	
	public Vector2 CoordsToPos(Vector2 latLng) {
		float lat = latLng.x;
		float lng = latLng.y;
		
		float newX = 0;
		float newY = 0;
		
		float cDeg = circumference/360f;
		
		
		newX = (lng - longitude) * cDeg;
		newY = (lat - latitude)*(((Mathf.Cos((latitude/180f)*Mathf.PI))*cDeg));

		//lat = latitude;
		//lng = longitude;
		
		return new Vector2(lng, lat);
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
