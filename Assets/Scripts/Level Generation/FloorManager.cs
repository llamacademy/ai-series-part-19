using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FloorManager : MonoBehaviour
{
    [SerializeField]
    private Transform WorldGeometry;
    [SerializeField]
    private Transform Player;
    public FloorSection[] FloorPrefabs;
    [SerializeField]
    [Range(3, 100)]
    private int TargetFloorSections = 3;
    private int HalfTargetFloorSections;
    public int ActiveSection = 0;

    private const int FloorSize = 10;
    private ObjectPool[] Pools;
    private NavMeshSurface[] Surfaces;
    public Dictionary<int, FloorSection> ActiveFloorSections = new Dictionary<int, FloorSection>();

    private void Awake()
    {
        if (WorldGeometry == null || WorldGeometry.GetComponent<NavMeshSurface>() == null)
        {
            Debug.LogError("World Geometry of Floor Manager must be assigned and must have at least 1 NavMeshSurface!");
            gameObject.SetActive(false);
            return;
        }

        Surfaces = WorldGeometry.GetComponentsInChildren<NavMeshSurface>();

        Pools = new ObjectPool[FloorPrefabs.Length];

        for (int i = 0; i < FloorPrefabs.Length; i++)
        {
            Pools[i] = ObjectPool.CreateInstance(FloorPrefabs[i], TargetFloorSections + 2);
        }
    }

    private void Start()
    {
        HalfTargetFloorSections = TargetFloorSections / 2;
        for (int i = -1 * HalfTargetFloorSections; i < HalfTargetFloorSections + 1; i++)
        {
            SpawnFloorSection(i == HalfTargetFloorSections, i);
        }
    }

    private void SpawnFloorSection(bool BakeNavMesh, int FloorIndex)
    {
        if (ActiveFloorSections.ContainsKey(FloorIndex))
        {
            return;
        }

        int index = Random.Range(0, Pools.Length);

        FloorSection spawnedObject = Pools[index].GetObject() as FloorSection;

        if (spawnedObject != null)
        {
            ActiveFloorSections.Add(FloorIndex, spawnedObject);
            spawnedObject.transform.position = new Vector3(FloorSize * FloorIndex, 0, 0);
            spawnedObject.OnReachBeginning += HandleReachBeginning;
            spawnedObject.OnReachEnd += HandleReachEnd;
            spawnedObject.Index = FloorIndex;
            spawnedObject.transform.SetParent(WorldGeometry, true);

            if (BakeNavMesh)
            {
                for (int i = 0; i < Surfaces.Length; i++)
                {
                    Surfaces[i].BuildNavMesh();
                }
            }
        }
    }

    private void HandleReachEnd(FloorSection Section)
    {
        if (Section.Index == ActiveSection)
        {
            SpawnFloorSection(true, ActiveSection + HalfTargetFloorSections + 1);
        }
        else if (Section.Index < ActiveSection)
        {
            TrimFloors();
            SpawnFloorSection(true, ActiveSection - HalfTargetFloorSections - 1);
            ActiveSection = Section.Index;
        }
    }

    private void HandleReachBeginning(FloorSection Section)
    {
        if (Section.Index > ActiveSection)
        {
            ActiveSection = Section.Index;
            TrimFloors();
        }
    }

    private void TrimFloors()
    {
        foreach (int key in ActiveFloorSections.Keys)
        {
            if (key > HalfTargetFloorSections + ActiveSection || key < ActiveSection - HalfTargetFloorSections)
            {
                ActiveFloorSections[key].gameObject.SetActive(false);
                ActiveFloorSections.Remove(key);
                return;
            }
        }
    }
}
