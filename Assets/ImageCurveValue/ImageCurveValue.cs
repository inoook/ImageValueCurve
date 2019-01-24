using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ImageCurveValue : MonoBehaviour {

    public enum FilePath
    {
        dataPath, persistentDataPath, temporaryCachePath, streamingAssetsPath
    }

    public static string GetApplicationPath(FilePath path)
    {
        string str = "";
        switch (path)
        {
            case FilePath.dataPath: str = Application.dataPath; break;
            case FilePath.persistentDataPath: str = Application.dataPath; break;
            case FilePath.temporaryCachePath: str = Application.temporaryCachePath; break;
            case FilePath.streamingAssetsPath: str = Application.streamingAssetsPath; break;
            default: str = Application.dataPath; break;
        }
        return str;
    }

    [SerializeField] FilePath filePath;
    [SerializeField] string fileName;
    [Header("識別する色")]
    [SerializeField] Color32 lineColor = Color.red;
    [Header("同じ色とする範囲")]
    [SerializeField] int colorRange = 10;

    [Header("画像の色を下からチェックするか？上からチェックするか？")]
    [SerializeField] bool isCheckFromBottom = true;

    Texture2D tex;
    float[] values;

    bool isDisposeTexture = false;

    void LoadImage()
    {
        if(tex != null) { return; }

        tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        tex.Apply(true);

        string dir = GetApplicationPath(filePath);
        string path = System.IO.Path.Combine(dir, fileName);
        byte[] bytes = File.ReadAllBytes(path);
        //Debug.Log(bytes.Length);
        try
        {
            ImageConversion.LoadImage(tex, bytes, false);
        }
        catch
        {
            Debug.LogWarningFormat("ImageLayer.ReadTexture({0})が読み込めませんでした", path);
        }

        values = GetImageCurveValues(tex);
        tex.Apply(false, true);
    }

    float[] GetImageCurveValues(Texture2D texture)
    {
        int w = texture.width;
        int h = texture.height;
        float[] values_ = new float[w];
        Color32[] pixels = texture.GetPixels32();
        //Debug.Log(pixels.Length);
        for (int i = 0; i < w; i++)
        {
            values_[i] = GetYValue(pixels, i, w, h);
        }
        return values_;
    }

    float GetYValue(Color32[] pixels, int xIndex, int w, int h)
    {
        float v = -1;
        for (int i = 0; i < h; i++)
        {
            int y = isCheckFromBottom ? i : h-i-1;
            Color32 c = pixels[w*y+xIndex];
            if (IsSameColor(colorRange, c, lineColor))
            {
                v = (float)y/(float)(h-1);
                break;
            }
        }
        if (v < 0)
        {
            Debug.LogErrorFormat("Error: 読み込んだ画像の {0} ピクセル目のラインが繋がっていません", xIndex);
        }
        return v;
    }

    bool IsSameColor(int colorRange_, Color32 currentColor, Color32 checkColor)
    {
        int r = Mathf.Abs((int)currentColor.r - (int)checkColor.r);
        int g = Mathf.Abs((int)currentColor.g - (int)checkColor.g);
        int b = Mathf.Abs((int)currentColor.b - (int)checkColor.b);
        return (r <= colorRange_ && g <= colorRange_ && b <= colorRange_);
    }

    #region public
    /// <summary>
    /// Prepare this instance.
    /// 事前準備する。それぞれの関数でLoadImageは呼ばれるので必ず呼ぶ必要はない。画像のロードが多少処理が重いので予めロードしたいときに使用する。
    /// </summary>
    public void Prepare()
    {
        LoadImage();
    }

    public float Evaluate(float time)
    {
        if (!isDisposeTexture)
        {
            LoadImage();
        }

        float v = 0;
        time = Mathf.Clamp01(time);
        int index = Mathf.FloorToInt((values.Length-1) * time);
        v = values[index];
        return v;
    }

    public Texture GetTexture()
    {
        LoadImage();
        return tex;
    }

    /// <summary>
    /// テクスチャの破棄
    /// Disposes the texture.
    /// </summary>
    public void DisposeTexture()
    {
        Destroy(tex);
        isDisposeTexture = true;
    }
    #endregion
}
