using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    #region Variables

    #region Singleton

    // Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Instance is null referenced");
            return _instance;
        }
        private set 
        {
            _instance = value;
        }
    }

    #endregion

    /// <summary>
    /// Player prefab
    /// </summary>
    public GameObject PlayerPrefab;
    /// <summary>
    /// Start of the road
    /// </summary>
    public GameObject StartRoadPrefab;
    /// <summary>
    /// All road turns to work with
    /// </summary>
    public List<GameObject> TurnPrefabsList;
    /// <summary>
    /// All roads to work with
    /// </summary>
    public List<GameObject> RoadPrefabsList;
    /// <summary>
    /// End of the road
    /// </summary>
    public GameObject FinishRoadPrefab;

    // Temporary list for work with recent level generated roads 
    private List<GameObject> _recentLevelRoads;

    #endregion

    #region UnityStartUpdate

    // Start is called before the first frame update
    void Start()
    {
       // Variables initialization
       Instance = this;
        _recentLevelRoads = new List<GameObject>();
        GenerateLevel(250);
    }

    #endregion

    #region Methods
    
    /// <summary>
    /// Generates level
    /// </summary>
    /// <param name="numberOfRoads">Number of roads to create</param>
    public void GenerateLevel(int numberOfRoads)
    {
        // Checking if the data is valid
        if (numberOfRoads <= 0 
            || StartRoadPrefab == null
            || RoadPrefabsList.Count <= 0
            || FinishRoadPrefab == null)
        {
            Debug.LogError("Incorrect data. Check variables or parameters!");
            return;
        }
        // Checking lists for valid data
        if (CheckGivenRoads() == false)
        {
            return;
        }
        // Clearing previous objects
        _recentLevelRoads.Clear();
        // Temporary variable for working with gameobjects
        GameObject recentRoad;
        // Spawning object with help of the pool
        recentRoad = ObjectPoolManager.Instance.SpawnObject(StartRoadPrefab);
        // Moving player on the start road
        PlayerPrefab.transform.position = StartRoadPrefab.transform.position + Vector3.up * 2;
        PlayerPrefab.transform.rotation = StartRoadPrefab.transform.rotation;
        // Adding start
        _recentLevelRoads.Add(recentRoad);
        // List of turns for creating correct way
        // (Helps us avoding situation, when we turn to many times in the same
        // direction and eventually hit our generated road)
        List<GameObject> turnCheckList = new List<GameObject>();
        // Boolean variable for generating distinctive roads
        bool isPreviousRoadSame;
        // Variable for containing randomly generated index
        int randomIndex = 0;
        // Adding roads according to given number
        for (int i = 1; i <= numberOfRoads; i++)
        {
            // Randomizing building of the road
            // If case will be true, we will add turn
            // else we will add straight road
            switch (Random.Range(0, 5) == 0)
            {
                // Adding turn
                case true:
                    // Setting boolean variable to true 
                    isPreviousRoadSame = true;
                    // While we won't generate distinctive road, we won't exit the cycle
                    while (isPreviousRoadSame)
                    {
                        // Generating random index
                        randomIndex = Random.Range(0, TurnPrefabsList.Count);
                        // Checking if the turn list has any objects in it
                        if ( turnCheckList.Count <= 0 )
                        {
                            // If so, setting variable to false
                            isPreviousRoadSame = false;
                        }
                        else
                        {
                            // Else we check, if last road is the same type as the recent randomly picked.
                            // If not, setting variable to false
                            if (turnCheckList[turnCheckList.Count - 1].GetComponent<RoadScript>().RoadType
                                != TurnPrefabsList[randomIndex].GetComponent<RoadScript>().RoadType)
                            {
                                isPreviousRoadSame = false;
                            }
                        }
                    }
                    // Spawning chosen turn
                    recentRoad = ObjectPoolManager.Instance.SpawnObject(TurnPrefabsList[randomIndex]);
                    // Adding element to temp list
                    turnCheckList.Add(TurnPrefabsList[randomIndex] as GameObject);
                    break;
                // Adding straight road
                case false:
                    randomIndex = Random.Range(0, RoadPrefabsList.Count);
                    // Choosing random road for spawn
                    recentRoad = ObjectPoolManager.Instance.SpawnObject(RoadPrefabsList[randomIndex]) as GameObject;
                    break;
            }
            // Getting position of the previous road joint to adjust our recent road to it
            recentRoad.transform.position = _recentLevelRoads[i - 1].GetComponent<RoadScript>().JointPosition;
            // Getting rotation  of the previous road joint to adjust our recent road to it
            recentRoad.transform.rotation = _recentLevelRoads[i - 1].GetComponent<RoadScript>().JointRotation;
            // Adding object to the list
            _recentLevelRoads.Add(recentRoad);
        }
        // Adding finish
        recentRoad = Instantiate(FinishRoadPrefab) as GameObject;
        // Getting position of the previous road to joint to adjust our recent road to it
        recentRoad.transform.position = _recentLevelRoads[_recentLevelRoads.Count - 1].GetComponent<RoadScript>().JointPosition;
        // Getting rotation  of the previous road joint to adjust our recent road to it
        recentRoad.transform.rotation = _recentLevelRoads[_recentLevelRoads.Count - 1].GetComponent<RoadScript>().JointRotation;
        // Adding object to the list
        _recentLevelRoads.Add(FinishRoadPrefab);
    }

    /// <summary>
    /// Checks roads in lists
    /// </summary>
    /// <returns>Returns false, if given roads are put in incorrect list</returns>
    private bool CheckGivenRoads()
    {
        //------------------------------------------------------------------------------
        // Checking if the road, which was added to specific lists or variables fit them
        //------------------------------------------------------------------------------
        // Variable for check
        RoadScript checkRoad;
        // Checking start road 
        if (StartRoadPrefab.GetComponent<RoadScript>().RoadType != RoadTypes.Start)
        {
            Debug.LogError("Incorrect object in start road prefab!");
            return false;
        }
        // Checking finish road 
        if (FinishRoadPrefab.GetComponent<RoadScript>().RoadType != RoadTypes.Finish)
        {
            Debug.LogError("Incorrect object in finish road prefab!");
            return false;
        }
        // Checking turn list
        for (int i = 0; i < TurnPrefabsList.Count; i++)
        {
            // Getting script 
            checkRoad = TurnPrefabsList[i].GetComponent<RoadScript>();
            // Checking road type to be a turn one 
            if (checkRoad.RoadType != RoadTypes.TurnLeft
                && checkRoad.RoadType != RoadTypes.TurnRight)
            {
                Debug.LogError("Incorrect object in turn list!");
                return false;
            }
        }
        // Checking road list
        for (int i = 0; i < RoadPrefabsList.Count; i++)
        {
            // Getting script 
            checkRoad = RoadPrefabsList[i].GetComponent<RoadScript>();
            // Checking road type to be a turn one 
            if (checkRoad.RoadType != RoadTypes.Straight)
            {
                Debug.LogError("Incorrect object in road list!");
                return false;
            }
        }
        // If all previous cycles didn't find anything wrong
        // return true
        return true;
    }
    
    #endregion
}
