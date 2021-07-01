using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCon : MonoBehaviour
{
    public Texture2D[] ds;
    public List<Color[]> lisColor;

    private void Awake()
    {
        ds = Resources.LoadAll<Texture2D>("Art");
    }

    void Start()
    {
        lisColor = new List<Color[]>();
        for (int i = 0; i < ds.Length; i++)
        {
            ReadTex(ds[i]);
        }

        Debug.Log(lisColor[3].Length);
    }

    void ReadTex(Texture2D tex)
    {
        Color[] colors = tex.GetPixels();
        lisColor.Add(colors);
    }

    void Update()
    {
        
    }
}


public class RGBVer2
{
    public Color color;
    public Vector2 ver;
    public RGBVer2(Color co,Vector2 v2)
    {
        color = co;
        ver = v2;
    }
}