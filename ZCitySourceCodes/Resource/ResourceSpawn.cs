using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceSpawn : MonoBehaviour
{
    [Header("Resource Lists")]
    [SerializeField] private List<GameObject> ResourceObjects;
    [SerializeField] private List<RootableBuilding> Buildings;
    [SerializeField] private List<RootableBuilding> SmallHouse;

    [Header("SpawnDelay")]
    [SerializeField] private float ResourceSpawnDelay;
    [SerializeField] private float BuildingCoolDownDelay;
    [SerializeField] private float HouseCoolDownDelay;

    [Header("SpawnAmount")]
    [SerializeField] private int ResourceSpawnAmount;

    [Header("InitSpawnAmount")]
    [SerializeField] private int InitResourceSpawnAmount;
    [SerializeField] private int InitBuildingSpawnAmount;
    [SerializeField] private int InitHouseSpawnAmount;

    private WaitForSeconds ResourceDelay;
    private WaitForSeconds BuildingDelay;
    private WaitForSeconds HouseDelay;

    private void Start()
    {
        StartCoroutine(InitSpawnCoroutine());
        
        ResourceDelay = new WaitForSeconds(ResourceSpawnDelay);
        BuildingDelay = new WaitForSeconds(BuildingCoolDownDelay);
        HouseDelay = new WaitForSeconds(HouseCoolDownDelay);
        
        StartCoroutine(ResourceSpawnCoroutine());
        StartCoroutine(BuildingCoolCoroutine());
        StartCoroutine(HouseCoolCoroutine());
    }

    private bool NotInCamera(int index)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(ResourceObjects[index].transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
            return false;
        else
            return true;
    }
    
    private bool RandomResourceSpawn(bool init)
    {
        bool end = false;
        
        int randomIndex = Random.Range(0, ResourceObjects.Count);
        
        if (!init && !ResourceObjects[randomIndex].activeInHierarchy && NotInCamera(randomIndex))
        {
            ResourceObjects[randomIndex].SetActive(true);
            end = true;
        }
        else if (init && !ResourceObjects[randomIndex].activeInHierarchy)
        {
            ResourceObjects[randomIndex].SetActive(true);
            end = true;
        }

        return end;
    }

    private bool RandomBuildingNHouse(ref List<RootableBuilding> list)
    {
        bool end = false;
        
        int randomIndex = Random.Range(0, list.Count);
        if (list[randomIndex].CoolDown)
        {
            list[randomIndex].CoolDown = false;
            end = true;
        }

        return end;
    }

    private IEnumerator InitSpawnCoroutine()
    {
        for (int i = 0; i < InitResourceSpawnAmount; i++)
        {
            while (!RandomResourceSpawn(true))
                yield return null;
        }

        for (int i = 0; i < InitBuildingSpawnAmount; i++)
        {
            while (RandomBuildingNHouse(ref Buildings))
                yield return null;
        }

        for (int i = 0; i < InitHouseSpawnAmount; i++)
        {
            while (RandomBuildingNHouse(ref SmallHouse))
                yield return null;
        }
    }
    
    private IEnumerator ResourceSpawnCoroutine()
    {
        while (true)
        {
            yield return ResourceDelay;
            for (int i=0; i<ResourceSpawnAmount; i++)
                RandomResourceSpawn(false);
        }
    }

    private IEnumerator BuildingCoolCoroutine()
    {
        while (true)
        {
            yield return BuildingDelay;
            while (!RandomBuildingNHouse(ref Buildings))
                yield return null;
        }
    }

    private IEnumerator HouseCoolCoroutine()
    {
        while (true)
        {
            yield return HouseDelay;
            while (!RandomBuildingNHouse(ref SmallHouse))
                yield return null;
        }
    }
}
