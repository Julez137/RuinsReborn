using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableCanvas : MonoBehaviour
{
    Canvas canvas;
    Vector2 referenceSize = new Vector2 (1920f, 1080f);
    float refreshTimer = 1f;
    float currentTime = 0f;
    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if (canvas != null)
        {
            if (currentTime <= refreshTimer)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime = 0f;
                Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
                Vector2 sizeDiff = new Vector2(canvasSize.x / referenceSize.x, canvasSize.y / referenceSize.y);
                float smallestSize = sizeDiff.x <= sizeDiff.y ? sizeDiff.x : sizeDiff.y;
                transform.localScale = new Vector2(smallestSize, smallestSize);
            }
        }
    }
}
