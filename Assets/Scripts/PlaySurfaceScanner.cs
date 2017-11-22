//Developer: Joseph Allen
//Script: PlaySurfaceScanner
//Version: 0.5
//Purpose: This script scans for the best play area and creates an appropriate grid for it.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlaySurfaceScanner : MonoBehaviour {
    /*These values are temporary and will be removed once this script 
    reaches parity with the new UnityEngine.XR namespace*/
    private static float unitLength = 0.305F;
    private static Vector3[][][] gridPoints;
    private static bool initializationComplete = false;

	void Awake () {
        //Set Tracking Space to RoomScale. If successful the floor will be at y = 0.
		if (XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale))
        {
            Debug.Log("Tracking space set to RoomScale successfully.");
            //The temporary code below will eventually be replaced by XR spatial mapping code
            /*Temporary Code starts here*/
            //initialize values to temp
            float tempX = 5 * -unitLength;
            float tempY = 8 * unitLength;
            float tempZ = 5 * unitLength;
            float accX = tempX;
            float accY = tempY;
            float accZ = tempZ;
            gridPoints = new Vector3[10][][];
            for (int set = 0; set < 10; ++set)
            {
                gridPoints[set] = new Vector3[8][];
                for (int row = 0; row < 8; ++row)
                {
                    gridPoints[set][row] = new Vector3[10];
                    for (int column = 0; column < 10; ++column)
                    {
                        gridPoints[set][row][column] = new Vector3(accX, accY, accZ);
                        accX += unitLength;
                    }
                    accX = tempX;
                    accY -= unitLength;
                }
                accY = tempY;
                accZ -= unitLength;
            }
            /*Temporary Code ends here */
            initializationComplete = true;
        }
        else
        {
            Debug.Log("Tracking space setting failed. World Space incorrect.");
        }
	}

    //This can be used to find out if the grid has been created. Can be useful if the user has
    //moved into a different environment since starting the program.
    public static bool CheckInitializationComplete()
    {
        return initializationComplete;
    }

    //get the maximum values for grid points
    public static int[] GetGridBounds()
    {
        int[] gridBounds = new int[3];
        gridBounds[0] = 10;
        gridBounds[1] = 8;
        gridBounds[2] = 10;
        return gridBounds;
    }

    //given a grid space point (x,y,z) this will return a Vector3 world space point
    public static Vector3 GetWorldPosition(int x, int y, int z)
    {
        //make sure the Vector3 is within the current grid. Otherwise throw an ArgumentException
        if ((x < 10 && x >= 0) && (y < 8 && y >= 0) && (z < 10 && z >= 0))
        {
            return gridPoints[x][y][z];
        }
        else
        {
            throw new System.ArgumentException("Grid points must be within the grid volume.");
        }
    }

    //given a grid space point Vector3 this will return a Vector3 world space point
    public static Vector3 GetWorldPosition(Vector3 gridPosition)
    {
        int x, y, z;
        //make sure the Vector3 is composed of int values. Otherwise throw an ArgumentException
        if (int.TryParse(gridPosition.x.ToString(), out x) && int.TryParse(gridPosition.y.ToString(), out y) && int.TryParse(gridPosition.z.ToString(), out z))
        {
            //make sure the Vector3 is within the current grid. Otherwise throw an ArgumentException
            if ((x < 10 && x >= 0) && (y < 8 && y >= 0) && (z < 10 && z >= 0))
            {
                return gridPoints[x][y][z];
            }
            else
            {
                throw new System.ArgumentException("Grid points must be within the grid volume.");
            }
        }
        else
        {
            throw new System.ArgumentException("Grid points must be composed of int values.");
        }
    }

    //given a world space point (x,y,z) this will return  the closest point to it in grid space
    public static Vector3 GetGridPosition(float x, float y, float z)
    {
        //initialize to a large value so that the first distance will likely be less
        float leastDistance = float.MaxValue;
        float temp = 0F;
        int gridX = 0, gridY = 0, gridZ = 0;

        //loop through all points and calculate distance to find the closet one
        for (int set = 0; set < 10; ++set)
        {
            gridPoints[set] = new Vector3[8][];
            for (int row = 0; row < 8; ++row)
            {
                gridPoints[set][row] = new Vector3[10];
                for (int column = 0; column < 10; ++column)
                {
                    temp = Vector3.Distance(gridPoints[set][row][column], new Vector3(x, y, z));
                    if (leastDistance > temp)
                    {
                        leastDistance = temp;
                        gridX = set;
                        gridY = row;
                        gridZ = column;
                    }
                }
            }
        }

        return new Vector3(gridX, gridY, gridZ);
    }

    //does the same as above, albeit with a Vector3
    public static Vector3 GetGridPosition(Vector3 vector)
    {
        return GetGridPosition(vector.x, vector.y, vector.z);
    }
}
