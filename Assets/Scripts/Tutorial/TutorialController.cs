using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    BeatManager _beatManager;

    [SerializeField] List<PopUpTextBoxController> _popUps = new List<PopUpTextBoxController>();

    private void Start()
    {
        _beatManager = GameObject.Find("@BeatManager").GetComponent<BeatManager>();
    }
}
