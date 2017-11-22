//Developer: Noah Zemlin
//Script: GameSetup
//Version: 1.0
//Purpose: This script spawns the game objects onto the map as well as checking for win conditions

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;


public class GameSetup : MonoBehaviour
{

    private Dictionary<GameObject, bool> goals;
    private bool mapMade = false;

    private bool won = false;

    private String dataPath;

    public GameSetup()
    {
        goals = new Dictionary<GameObject, bool>();
    }

    private void Awake()
    {
        dataPath = Application.dataPath;
    }

    public void SetVictory(GameObject g, bool victory)
    {
        if (goals.ContainsKey(g))
            goals[g] = victory;
    }

    //Spawns all the game objecst
    //This may need to be set to public if some other script is going to setup the world
    private void SetupWorld(String mapName)
    {

        if (File.Exists(dataPath + mapName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath + mapName, FileMode.Open);
            Map map = (Map)bf.Deserialize(file);
            file.Close();

            if (File.Exists(dataPath + map.skin))
            {
                bf = new BinaryFormatter();
                file = File.Open(dataPath + map.skin, FileMode.Open);
                Skin skin = (Skin)bf.Deserialize(file);
                file.Close();

                GameObject beaconSkin = (GameObject)AssetDatabase.LoadAssetAtPath("Assets" + skin.beacon, typeof(GameObject));
                GameObject goalSkin = (GameObject)AssetDatabase.LoadAssetAtPath("Assets" + skin.goal, typeof(GameObject));
                GameObject emitterSkin = (GameObject)AssetDatabase.LoadAssetAtPath("Assets" + skin.emitter, typeof(GameObject));

                Debug.Log(beaconSkin != null);


                foreach (MapObject obj in map.objects)
                {
                    switch (obj.type)
                    {
                        case ObjectType.Beacon:
                            Instantiate(beaconSkin, PlaySurfaceScanner.GetWorldPosition(obj.posx, obj.posy, obj.posz), Quaternion.identity);
                            break;
                        case ObjectType.Emitter:
                            Instantiate(emitterSkin, PlaySurfaceScanner.GetWorldPosition(obj.posx, obj.posy, obj.posz), Quaternion.identity);
                            break;
                        case ObjectType.Goal:
                            GameObject goal = Instantiate(goalSkin, PlaySurfaceScanner.GetWorldPosition(obj.posx, obj.posy, obj.posz), Quaternion.identity);
                            goals.Add(goal, false);
                            break;

                    }
                }
            } else
            {
                Debug.Log("(" + dataPath + map.skin + ")   does not exist. :c");
            }

        }

        else
        {
            Debug.Log("(" + dataPath + mapName + ")   does not exist. :c");
        }

        /*foreach (MapObject obj in map.objects)
        {
            Vector3 pos = PlaySurfaceScanner.GetWorldPosition(obj.position);

            GameObject gobj = Instantiate(, pos, obj.rotation);
            if (obj.type == ObjectType.Goal)
            {
                goals.Add(gobj, false);
            }
        }*/

        mapMade = true;
    }

    int count = 0;
    private void Update()
    {
        //if the map exists, assume game is in play
        if (mapMade && goals.Count > 0)
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
        else if (PlaySurfaceScanner.CheckInitializationComplete() && count % 100 == 0)
        {
            count++;
            //we can make the map
            SetupWorld("/Maps/level1.map");
        }
    }

    public void CreateExampleMap()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(dataPath + "/Maps/level1.map", FileMode.OpenOrCreate);

        Map map = new Map();

        MapObject obj1 = new MapObject
        {
            type = ObjectType.Emitter
        };

        MapObject obj2 = new MapObject
        {
            type = ObjectType.Beacon,
            posx = 1,
            posz = 3
        };

        MapObject obj3 = new MapObject
        {
            type = ObjectType.Goal,
            posx = 3,
            posz = 1
        };

        map.objects.Add(obj1);
        map.objects.Add(obj2);
        map.objects.Add(obj3);

        bf.Serialize(file, map);
        file.Close();
    }

    public void CreateExampleSkin()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(dataPath + "/Maps/default.skin", FileMode.OpenOrCreate);

        Skin skin = new Skin
        {
            goal = "/Prefabs/laser goal test.prefab",
            beacon = "/Prefabs/laser goal test.prefab",
            emitter = "/Prefabs/laser goal test.prefab"
        };

        bf.Serialize(file, skin);
        file.Close();
    }

}

[Serializable]
class Skin
{
    public String emitter;
    public String beacon;
    public String goal;
}

[Serializable]
class Map
{
    public List<MapObject> objects = new List<MapObject>();
    public String skin = "/Maps/default.skin";
}

[Serializable]
class MapObject
{
    public ObjectType type = ObjectType.Emitter;
    public int posx, posy, posz = 0; //Vector3 isnt serializable, so we have to use ints
    public int rotx, roty, rotz = 0;
}

[Serializable]
enum ObjectType
{
    Emitter, Beacon, Goal
}