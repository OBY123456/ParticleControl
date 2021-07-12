using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PolygonDrawer : MonoBehaviour
{
    public static PolygonDrawer Instance;
    public Material material;
    /// <summary>
    /// 构建图形的点
    /// </summary>
    public Transform[] vertices;
    private MeshRenderer mRenderer;
    private MeshFilter mFilter;

    private Color[] colors;

    public Texture2D[] ds;
    public Material[] materials;
    public List<Color[]> lisColor;

    private int CurrentValue = 0;

    public Color[] InputColor;
    public float[] InputPos;
    public float[] Ratio;

    Vector2 Center;

    public bool IsColorSent, IsPosSent;

    public string ColorStr, PosStr,TouchStr, RatioStr,StateStr;

    public Color CurrentColor;

    //贴图大小
    float value = 256f;

    private void Awake()
    {
        Instance = this;
        ds = Resources.LoadAll<Texture2D>("Art");
        materials = Resources.LoadAll<Material>("Materials");
        lisColor = new List<Color[]>();
        for (int i = 0; i < ds.Length; i++)
        {
            ReadTex(ds[i]);
        }
        InputPos = new float[4];
        InputColor = new Color[4];
        Ratio = new float[4];
        Center = transform.Find("圈圈").position;
        PosStr = "0.000-0.000";
        CurrentColor = Color.red;
        ColorStr = FormatColor(CurrentColor);
        TouchStr = "0";
        RatioStr = "0.0000";
        StateStr = "0";
        
    }

    void Start()
    {
        Draw();
        UDPSent.Instance.Send("B-" + ColorStr);
    }

    void Update()
    {
        Draw();
    }

    [ContextMenu("Draw")]
    public void Draw()
    {
        Vector2[] vertices2D = new Vector2[vertices.Length];
        Vector3[] vertices3D = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertice = vertices[i].localPosition;
            vertices2D[i] = new Vector2(vertice.x, vertice.y);
            vertices3D[i] = vertice;
        }

        Triangulator tr = new Triangulator(vertices2D);
        int[] triangles = tr.Triangulate();

        Mesh mesh = new Mesh();
        mesh.vertices = vertices3D;
        mesh.triangles = triangles;
        mesh.uv = vertices2D;

        if (mRenderer == null)
        {
            mRenderer = gameObject.GetOrAddComponent<MeshRenderer>();
        }
        mRenderer.material = material;
        if (mFilter == null)
        {
            mFilter = gameObject.GetOrAddComponent<MeshFilter>();
        }
        mFilter.mesh = mesh;
    }

    public void SetMaterial(int num,Color color)
    {
        material = materials[num];
        CurrentValue = num;
        CurrentColor = color;
        ColorStr = FormatColor(CurrentColor);
        UIManager.GetPanel<MainPanel>(WindowTypeEnum.ForegroundScreen).SetImageColor(color);
        SentMsg();
        UDPSent.Instance.Send( "B-" + ColorStr);
    }

    void ReadTex(Texture2D tex)
    {
        Color[] colors = tex.GetPixels();
        lisColor.Add(colors);
    }

    float distance;
    Color color;
    public void GetColor(SpereType spereType, Touch touch, float a,float b)
    {
        distance = Distance(a, b);
        switch (spereType)
        {
            case SpereType.Sphere:
                InputPos[0] = distance;
                break;
            case SpereType.Sphere1:
                InputPos[1] = distance;
                break;
            case SpereType.Sphere2:
                InputPos[2] = distance;
                break;
            case SpereType.Sphere3:
                InputPos[3] = distance;
                break;
            default:
                break;
        }

        if (distance == InputPos.Max())
        {
            float x = touch.position.x / Screen.width;
            float y = (touch.position.y - (Screen.height - Screen.width)) / Screen.width;

            x = Mathf.Clamp(x, 0f, 1f);
            y = Mathf.Clamp(y, 0f, 1f);

            int num = Mathf.RoundToInt((Mathf.Clamp(value * y, 1, value) - 1) * value + (Mathf.Clamp(value * x, 1, value) - 1));
            color = lisColor[CurrentValue][num];
            switch (spereType)
            {
                case SpereType.Sphere:
                    InputColor[0] = color;
                    break;
                case SpereType.Sphere1:
                    InputColor[1] = color;
                    break;
                case SpereType.Sphere2:
                    InputColor[2] = color;
                    break;
                case SpereType.Sphere3:
                    InputColor[3] = color;
                    break;
                default:
                    break;
            }
        }
        else
        {
            distance = InputPos.Max();
            for (int i = 0; i < InputPos.Length; i++)
            {
                if(distance == InputPos[i])
                {
                    color = InputColor[i];
                }
            }
        }
        UIManager.GetPanel<MainPanel>(WindowTypeEnum.ForegroundScreen).SetImageColor(color);
        ColorStr = FormatColor(color);
        RatioStr = (Ratio.Sum() / 4).ToString("0.0000");
        if (!IsPosSent)
            SentMsg();
        //Debug.Log(color.ToString());
    }

    public void GetPos(Touch touch)
    {
        float x = touch.position.x / Screen.width;
        float y = (touch.position.y - (Screen.height - Screen.width)) / Screen.width;

        x = Mathf.Clamp(x, 0f, 1f);
        y = Mathf.Clamp(y, 0f, 1f);

        PosStr = x.ToString("0.000") + "-"+y.ToString("0.000");
        if (!IsColorSent)
            SentMsg();
    }

    private float Distance(float x, float y)
    {
        return Vector2.Distance(new Vector2(x, y), Center);
    }

    public void Release()
    {
        if(!IsColorSent)
        {
            ColorStr = FormatColor(CurrentColor);
            UIManager.GetPanel<MainPanel>(WindowTypeEnum.ForegroundScreen).SetImageColor(CurrentColor);
            RatioStr = "0.0000";
            SentMsg();       
        }
        
    }

    private string FormatColor(Color color)
    {
        return color.r.ToString("0.000") + "-" + color.g.ToString("0.000") + "-" + color.b.ToString("0.000") + "-" + color.a.ToString("0.000");
    }

    public void LineRelease()
    {
        IsPosSent = false;
        PosStr = "0.000-0.000";
        TouchStr = "0";
        if (!IsColorSent && !IsPosSent)
        {
            SentMsg();
        }
    }

    public void SentMsg()
    {
        UDPSent.Instance.Send(PosStr + "-" + ColorStr + "-" + TouchStr + "-" + RatioStr + "-" + StateStr);
    }
}
