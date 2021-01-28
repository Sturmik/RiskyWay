using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// Start of the road
    /// </summary>
    public GameObject StartRoadPrefab;
    /// <summary>
    /// All roads to work with
    /// </summary>
    public List<GameObject> RoadPrefabsPool;
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
        GenerateLevel(10);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            || RoadPrefabsPool.Count <= 0
            || FinishRoadPrefab == null)
        {
            Debug.LogError("Incorrect data. Check variables or parameters!");
            return;
        }
        // Clearing previous objects
        _recentLevelRoads.Clear();
        // Temporary variable for working with gameobjects
        GameObject recentRoad = Instantiate(StartRoadPrefab) as GameObject;
        // Adding start
        _recentLevelRoads.Add(recentRoad);
        // Adding roads according to given number
        for (int i = 1; i <= numberOfRoads; i++)
        {
            // Choosing random road for spawn
            recentRoad = 
                Instantiate(RoadPrefabsPool[Random.Range(0, RoadPrefabsPool.Count)]) as GameObject;
            // Getting position of the previous road joint to adjust our recent road to it
            recentRoad.transform.position = _recentLevelRoads[i - 1].GetComponent<RoadScript>().JointPosition;
            // Getting rotation  of the previous road joint to adjust our recent road to it
            recentRoad.transform.rotation =  _recentLevelRoads[i - 1].GetComponent<RoadScript>().JointRotation;
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
    
    #endregion
}
