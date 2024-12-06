using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
	
	public float weight = 0f;
	
	public void AddFruit(FruitBody f) {
		
		weight += f.weight;
		
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
