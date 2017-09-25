using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour {

    GameObject heldItem;

	// Use this for initialization
	void Start () {
		
	}

    static GameObject FindTagInParents(Transform obj, string tag)
    {
        while (true)
        {
            if (obj.tag == tag)
            {
                return obj.gameObject;
            }
            else if (obj.parent == null)
            {
                return null;
            }
            obj = obj.parent;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (heldItem != null)
        {
            Debug.Log(heldItem.name);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left Click");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = FindTagInParents(hit.transform, "Moveable");
                if (obj)
                {
                    heldItem = obj;
                    foreach (Collider c in heldItem.GetComponentsInChildren<Collider>())
                    {
                        c.enabled = false;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            foreach (Collider c in heldItem.GetComponentsInChildren<Collider>())
            {
                c.enabled = true;
            }
            heldItem = null;
        }
        else if (heldItem != null && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = FindTagInParents(hit.transform, "Moveable");
                if (obj == null)
                {
                    heldItem.transform.position = hit.point;
                }
            }
        }
	}
}
