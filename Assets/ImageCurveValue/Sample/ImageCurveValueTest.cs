using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCurveValueTest : MonoBehaviour {

    [SerializeField] ImageCurveValue imageCurve;

    [SerializeField, Range(0, 1)] float time = 0;
    [SerializeField] public float v = 0;

    [SerializeField] float speed = 0.1f;
    [SerializeField] RawImage rawImage;
    [SerializeField] RectTransform pointRectTrans;

    RectTransform rawImageRectTrans;

    // Use this for initialization
    void Start () {
        rawImageRectTrans = rawImage.GetComponent<RectTransform>();

        rawImage.texture = imageCurve.GetTexture();

    }

    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime * speed;
        time = Mathf.Repeat(time, 1);

        //float t_v = imageCurve.Evaluate(time);
        //float t_v = imageCurve.EvaluateWithAvg(time, 14, 14);
        float t_v = imageCurve.EvaluateWithRCFilter(time, 0.7f);
        v = t_v;
        //v = Mathf.Lerp(v, t_v, Time.deltaTime * 50);// 多少スムーズに

        if (Input.GetKeyDown(KeyCode.D))
        {
            imageCurve.DisposeTexture();
        }

        pointRectTrans.anchoredPosition = new Vector2(rawImageRectTrans.sizeDelta.x * time, rawImageRectTrans.sizeDelta.y * v);
    }
}
