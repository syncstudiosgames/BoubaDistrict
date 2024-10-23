using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModelLoader : MonoBehaviour
{
    [SerializeField] List<MeshFilter> _models = new List<MeshFilter>();

    [SerializeField] MeshFilter _meshFilter;

    private void Start()
    {
        // Asignar un mesh random...
    }
}
