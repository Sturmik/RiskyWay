using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for storing objects and optimizing
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    #region Variables

    #region Singleton

    // Singleton
    public static ObjectPoolManager _instance;
    public static ObjectPoolManager Instance
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

    // Where all the spawned objects will be hold
    public GameObject ObjectsHolder;
    // Objects pool
    private List<GameObject> _objectsPool;

    #endregion

    #region Constructor

    public ObjectPoolManager()
    {
        Instance = this;
        _objectsPool = new List<GameObject>();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Spawns specific object
    /// </summary>
    /// <param name="toSpawn">Object, which should be spawned</param>
    public GameObject SpawnObject(GameObject toSpawn)
    {
        // First, we try to find object in our pool
        for (int i = 0; i < _objectsPool.Count; i++)
        {
            // Comparing objects
            if (_objectsPool[i].name.Replace("(Clone)", "").Equals(toSpawn.name))
            {
                // Checking the object is not active
                if (!_objectsPool[i].gameObject.activeSelf)
                {
                    // If objects are same, activate this object 
                    _objectsPool[i].SetActive(true);
                    return _objectsPool[i];
                }
            }
        }
        GameObject toCreate;
        // If there is no object like that in the pool, when we will just instantiate it
        toCreate = Instantiate(toSpawn) as GameObject;
        _objectsPool.Add(toCreate);
        toCreate.transform.parent = ObjectsHolder.transform;
        return toCreate;
    }

    /// <summary>
    /// Disables all objects, by setting them to false 
    /// </summary>
    public void DisableAllObjects()
    {
        for (int i = 0; i < _objectsPool.Count; i++)
        {
            _objectsPool[i].SetActive(false);
        }
    }

    #endregion
}
