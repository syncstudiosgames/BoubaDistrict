using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicEffect : MonoBehaviour
{
    [SerializeField] Enemy _enemy;
    [SerializeField] Canvas _canvas;

    [SerializeField] List<Sprite> _effectsSprites;

    [SerializeField] float _effectProbability;

    private void Start()
    {
        _enemy.OnRestore += () =>
        {
            if (UnityEngine.Random.Range(0, 100) < _effectProbability)
            {
                PlayEffect();
            }
        };
    }

    void PlayEffect()
    {
        var imageGameObject = AddImageGameObject(_canvas, _effectsSprites[UnityEngine.Random.Range(0, _effectsSprites.Count)]);
        var rectTransform = imageGameObject.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;
        LeanTween.scale(imageGameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack).setOnComplete(() =>
        {
            LeanTween.alpha(rectTransform, 0f, 0.5f).setEase(LeanTweenType.easeOutQuad);
        });
    }

    private void Update()
    {
        OrientateCanvasTowardsMainCamera(_canvas);
    }

    GameObject AddImageGameObject(Canvas canvas, Sprite _sprite)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        GameObject imageObject = new GameObject("EffectImage");
        imageObject.transform.SetParent(canvas.transform);

        var image = imageObject.AddComponent<Image>();
        image.sprite = _sprite;

        RectTransform rectTransform = image.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;

        // For some reason images appeared inverted. Fix that:
        var canvasRot = canvasRect.rotation;
        canvasRot.y = 180;
        rectTransform.localRotation = canvasRot;

        rectTransform.sizeDelta = new Vector2(canvasWidth, canvasHeight);

        return imageObject;
    }

    void OrientateCanvasTowardsMainCamera(Canvas canvas)
    {
        if (Camera.main != null)
        {
            canvas.transform.LookAt(Camera.main.transform);
            var newRotation = Quaternion.LookRotation(Camera.main.transform.position - canvas.transform.position);
            newRotation.y = 0;
            newRotation.z = 0;
            canvas.transform.rotation = newRotation;
        }
    }

}
