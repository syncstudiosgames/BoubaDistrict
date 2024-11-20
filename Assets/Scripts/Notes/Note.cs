using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "NewNote", menuName = "ScriptableObjects/Note")]
public class Note : ScriptableObject
{
    public string noteName;
    public InputActionAsset actions;
    [SerializeField] Sprite _sprite;
    [SerializeField] Sprite _highlightSpirte;

    [SerializeField] int actionIndex;

    public InputAction Action { get { return actions.actionMaps[0].actions[actionIndex]; } }
    public Sprite Sprite { get { return _sprite; } }
    public Sprite HighlightSprite { get { return _highlightSpirte; } }

    public override string ToString()
    {
        return noteName;
    }
}
