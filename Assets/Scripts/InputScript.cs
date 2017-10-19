//Created by David McKnight
//29 September 2017
//Current version: 29 September 2017
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for handling Virtual Reality inputs. 
/// </summary>
public class InputScript : MonoBehaviour {

    private RaycastHit _raycastHitInfo = new RaycastHit();

    /// <summary>
    /// Determines what the user's gaze is hitting.
    /// </summary>
    public RaycastHit RaycastHitInfo
    {
        get { return _raycastHitInfo; }
        set { _raycastHitInfo = value; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        //Update RaycastHitInfo
        if (Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out _raycastHitInfo,
                20.0f,
                Physics.DefaultRaycastLayers))
        {
            // If the Raycast has succeeded and hit a hologram
            // hitInfo's point represents the position being gazed at
            // hitInfo's collider GameObject represents the hologram being gazed at
        }
    }
}
