using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCurveValueTest : MonoBehaviour {

    [SerializeField] ImageCurveValue imageCurve;

    [SerializeField, Range(0, 1)] float time = 0;
    [SerializeField] public float v = 0;

    [SerializeField] RawImage rawImage;
    [SerializeField] RectTransform pointRectTrans;

    RectTransform rawImageRectTrans;

    // Use this for initialization
    void Start () {
        rawImageRectTrans = rawImage.GetComponent<RectTransform>();

        rawImage.texture = imageCurve.GetTexture();
    }
	
	// Update is called once per frame
	void Update () {
        v = imageCurve.Evaluate(time);

        if (Input.GetKeyDown(KeyCode.D))
        {
            imageCurve.DisposeTexture();
        }

        pointRectTrans.anchoredPosition = new Vector2(rawImageRectTrans.sizeDelta.x * time, rawImageRectTrans.sizeDelta.y* v);
    }
}
