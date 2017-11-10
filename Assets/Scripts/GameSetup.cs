//Developer: Noah Zemlin
//Script: GameSetup
//Version: 1.0
//Purpose: This script spawns the game objects onto the map as well as checking for win conditions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSetup : MonoBehaviour
{

    private Dictionary<Goal, bool> goals;
    private bool mapMade = false;

    private bool won = false;

    public LaserEmitter laserPrefab;
    public LaserRefractor beaconPrefab;
    public Goal goalPrefab;

    public GameSetup()
    {
        goals = new Dictionary<Goal, bool>();
    }

    public void SetVictory(Goal g, bool victory)
    {
        if (goals.ContainsKey(g))
            goals[g] = victory;
    }

    //Spawns all the game objecst
    //This may need to be set to public if some other script is going to setup the world
    private void SetupWorld()
    {
        if (laserPrefab != null && beaconPrefab != null && goalPrefab != null)
        {
            //spawn the map
            mapMade = true;

            //example spawn stuff (will come from json in future)
            Vector3 pos1 = PlaySurfaceScanner.GetWorldPosition(1, 1, 1);
            Vector3 pos2 = PlaySurfaceScanner.GetWorldPosition(1, 3, 1);
            Vector3 pos3 = PlaySurfaceScanner.GetWorldPosition(1, 1, 3);

            Instantiate(laserPrefab, pos1, Quaternion.Euler(0, 0, 0));
            Instantiate(beaconPrefab, pos2, Quaternion.Euler(0, 0, 0));
            Goal goal = Instantiate<Goal>(goalPrefab, pos3, Quaternion.Euler(0, 0, 0));
            goals.Add(goal, false);

        }
        else
        {
            Debug.LogError("The GameSetup prefabs must be set!");
        }
    }

    private void Update()
    {
        //if the map exists, assume game is in play
        if (mapMade)
        {
            won = true;

            //find any unsatisfied win conditions
            foreach (bool goalVictory in goals.Values)
            {
                if (!goalVictory)
                {
                    won = false;
                    break;
                }
            }

            if (won)
            {
                Debug.Log("You won! Wow! Amazing! Nice shot!");
            }
        }
        //if the PSS is done, then we can make the map using it's grid
        else if (PlaySurfaceScanner.CheckInitializationComplete())
        {
            //we can make the map
            SetupWorld();
        }
    }

}
