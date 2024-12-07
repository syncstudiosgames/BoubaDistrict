using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModelLoader : MonoBehaviour
{
    [SerializeField] public GameObject[] enemyModels;
    [SerializeField] public GameObject[] enemySplash;


    [SerializeField] private Transform modelHolder;

    public GameObject splash;

    private void Start()
    {
    }

    public void AssignRandomModel()
    {
        if (enemyModels.Length == 0)
        {
            Debug.LogError("No hay modelos asignados");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, enemyModels.Length);
        GameObject randomModel = enemyModels[randomIndex];
        splash = enemySplash[randomIndex];

        if (modelHolder == null)
        {
            return;
        }

        GameObject modelInstance = Instantiate(randomModel, modelHolder.position, modelHolder.rotation, modelHolder);

        modelInstance.transform.localPosition = Vector3.zero;
        modelInstance.transform.localRotation = Quaternion.identity;
        modelInstance.transform.localScale = randomModel.transform.localScale;
    }

}
