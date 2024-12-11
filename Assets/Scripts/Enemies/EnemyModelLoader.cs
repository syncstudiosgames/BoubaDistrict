using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModelLoader : MonoBehaviour
{
    [SerializeField] public GameObject[] enemyModels;
    [SerializeField] public GameObject[] enemySplash;
    [SerializeField] public RuntimeAnimatorController[] enemyAnimations;


    [SerializeField] private Transform modelHolder;

    public GameObject splash;
    public RuntimeAnimatorController controller;

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
        controller = enemyAnimations[randomIndex];

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
