using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsB : MonoBehaviour
{

    // Referencias a los GameObjects
    public GameObject settingsPrefab;
    public GameObject fade;

    public void ToggleSettings()
    {
        if (settingsPrefab != null)
        {
            settingsPrefab.SetActive(true);
            fade.SetActive(true);
        } else
        {
            settingsPrefab.SetActive(!false);
            fade.SetActive(false);
        }

    }










}
