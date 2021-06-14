using System.Collections;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AreaFloorBaker : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface[] Surfaces;
    [SerializeField]
    private Player Player;
    [SerializeField]
    private float UpdateRate = 0.1f;
    [SerializeField]
    private float MovementThreshold = 3f;
    [SerializeField]
    private Vector3 NavMeshSize = new Vector3(20, 20, 20);

    public delegate void NavMeshUpdatedEvent(Bounds Bounds);
    public NavMeshUpdatedEvent OnNavMeshUpdate;

    private Vector3 WorldAnchor;
    private NavMeshData[] NavMeshDatas;
    private List<NavMeshBuildSource> Sources = new List<NavMeshBuildSource>();

    private void Awake()
    {
        NavMeshDatas = new NavMeshData[Surfaces.Length];
        for (int i = 0; i < Surfaces.Length; i++)
        {
            NavMeshDatas[i] = new NavMeshData();
            NavMesh.AddNavMeshData(NavMeshDatas[i]);
        }

        BuildNavMesh(false);
        StartCoroutine(CheckPlayerMovement());
    }

    private IEnumerator CheckPlayerMovement()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        while (true)
        {
            if (Vector3.Distance(WorldAnchor, Player.transform.position) > MovementThreshold)
            {
                BuildNavMesh(true);
                WorldAnchor = Player.transform.position;
            }

            yield return Wait;
        }
    }

    private void BuildNavMesh(bool Async)
    {
        Bounds navMeshBounds = new Bounds(Player.transform.position, NavMeshSize);
        List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();

        List<NavMeshModifier> modifiers;
        for (int index = 0; index < Surfaces.Length; index++)
        {
            if (Surfaces[index].collectObjects == CollectObjects.Children)
            {
                modifiers = new List<NavMeshModifier>(GetComponentsInChildren<NavMeshModifier>());
            }
            else
            {
                modifiers = NavMeshModifier.activeModifiers;
            }

            for (int i = 0; i < modifiers.Count; i++)
            {
                if (((Surfaces[index].layerMask & (1 << modifiers[i].gameObject.layer)) == 1)
                    && modifiers[i].AffectsAgentType(Surfaces[index].agentTypeID))
                {
                    markups.Add(new NavMeshBuildMarkup()
                    {
                        root = modifiers[i].transform,
                        overrideArea = modifiers[i].overrideArea,
                        area = modifiers[i].area,
                        ignoreFromBuild = modifiers[i].ignoreFromBuild
                    });
                }
            }

            if (Surfaces[index].collectObjects == CollectObjects.Children)
            {
                NavMeshBuilder.CollectSources(transform, Surfaces[index].layerMask, Surfaces[index].useGeometry, Surfaces[index].defaultArea, markups, Sources);
            }
            else
            {
                NavMeshBuilder.CollectSources(navMeshBounds, Surfaces[index].layerMask, Surfaces[index].useGeometry, Surfaces[index].defaultArea, markups, Sources);
            }

            Sources.RemoveAll(RemoveNavMeshAgentPredicate);

            if (Async)
            {
                AsyncOperation navMeshUpdateOperation = NavMeshBuilder.UpdateNavMeshDataAsync(NavMeshDatas[index], Surfaces[index].GetBuildSettings(), Sources, navMeshBounds);
                navMeshUpdateOperation.completed += HandleNavMeshUpdate;
            }
            else
            {
                NavMeshBuilder.UpdateNavMeshData(NavMeshDatas[index], Surfaces[index].GetBuildSettings(), Sources, navMeshBounds);
                OnNavMeshUpdate?.Invoke(navMeshBounds);
            }
        }
    }

    private bool RemoveNavMeshAgentPredicate(NavMeshBuildSource Source)
    {
        return Source.component != null && Source.component.gameObject.GetComponent<NavMeshAgent>() != null;
    }

    private void HandleNavMeshUpdate(AsyncOperation Operation)
    {
        OnNavMeshUpdate?.Invoke(new Bounds(WorldAnchor, NavMeshSize));
    }
}