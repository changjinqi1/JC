using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation; // For NavMeshSurface

public class DynamicNavmeshUpdate : MonoBehaviour

{
    public NavMeshSurface navMeshSurface;  // Assign in Inspector
    public GameObject hiddenBridge;        // The object that becomes walkable later

    public void UnlockPath()
    {
        // Enable the new walkable object
        hiddenBridge.SetActive(true);

        // Rebuild NavMesh to include the new area
        navMeshSurface.BuildNavMesh();
    }
}
