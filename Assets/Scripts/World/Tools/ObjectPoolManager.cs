using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject objectPoolHolder;

    private GameObject gameObjectsMap;
    private GameObject gameObjectsEmpty;

    public enum PoolType
    {
        Ammo,
        Map,    
        None
    }

    public PoolType PoolingType;

    private void Awake()
    {
        SetUpEmpties();
    }

    private void SetUpEmpties()
    {
        objectPoolHolder = new GameObject("Pooled Objects");

        gameObjectsEmpty = new GameObject("Empty");
        gameObjectsEmpty.transform.SetParent(objectPoolHolder.transform);

        gameObjectsMap = new GameObject("Map");
        gameObjectsMap.transform.SetParent(objectPoolHolder.transform);
    }

    public GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        // If it is the same name, assign pool to it
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name};
            ObjectPools.Add(pool);
        }

        // Check for any inactive objects
        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            // Find the parent of the empty object
            GameObject parentObject = SetParentObject(poolType);

            // If there are no inactive objects, create new
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if (parentObject != null)
            {
                spawnableObject.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            // If there is an inactive object, activate it
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    public IEnumerator ReturnObjectToPool(GameObject obj, float timeBeforeReturn = 0.0f)
    {
        yield return new WaitForSeconds(timeBeforeReturn);

        // By taking off 7 letters of the string, we are removing (Clone) from the name of the passed inobj
        Debug.Log(obj.name);
        string goName = obj.name.Substring(0, obj.name.Length - 7); 

        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

        // Remove this later most likely, only for testing
        if (pool == null)
        {
            // pool = new PooledObjectInfo() { LookupString = goName};
            // ObjectPools.Add(pool);

            // obj.SetActive(false);
            // pool.InactiveObjects.Add(obj);
            Debug.LogWarning("Trying to release an object that is not pooled: " + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }


    private GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.None:
                return gameObjectsEmpty;
            case PoolType.Map:
                return gameObjectsMap;
            default:
                return null;
        }
    }
}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
