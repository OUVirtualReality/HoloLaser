using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour {
    
    GameObject laser;

    // Use this for initialization
    void Start () {
        laser = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        laser.name = "Laser Beam";
        laser.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        laser.transform.parent = this.transform;
    }
	
	void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.up);
        Debug.DrawRay(transform.position, fwd, Color.green);

        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, fwd, out hit)) {
            laser.transform.localScale = new Vector3(0.1f, hit.distance * 5, 0.1f);
        }
        else
        {
            laser.transform.localScale = new Vector3(0.1f, 100f, 0.1f);
        }

        laser.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + laser.transform.localScale.y, transform.localPosition.z);
    }

}
