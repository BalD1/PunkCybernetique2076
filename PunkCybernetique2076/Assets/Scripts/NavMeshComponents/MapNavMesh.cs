using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapNavMesh : MonoBehaviour
{
    [SerializeField] private NavMeshSurface[] surfaces;

    private void Awake()
    {
        GameManager.Instance.BuildNavMesh(surfaces);
    }
}
