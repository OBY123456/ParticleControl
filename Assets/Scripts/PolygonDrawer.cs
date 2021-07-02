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

    Vector2 Center;

    public bool IsColorSent, IsPosSent;

    public string ColorStr, PosStr,TouchStr;

    public Color CurrentColor;

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
        Center = transform.Find("圈圈").position;
        PosStr = "00";
        CurrentColor = Color.red;
        ColorStr = CurrentColor.ToString();
        TouchStr = "0";
    }

    void Start()
    {
        Draw();
    }

    void Update()
    {
        Draw();

        if (Input.mousePosition.y > (Screen.height - Screen.width))
        {
            TouchStr = "1";
        }
        else
        {
            TouchStr = "0";
            UDPSent.Instance.Send(PosStr + "j" + ColorStr + "j" + TouchStr);
        }
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

            int num = Mathf.RoundToInt((Mathf.Clamp(700 * y, 1, 700) - 1) * 700 + (Mathf.Clamp(700 * x, 1, 700) - 1));
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

        ColorStr = color.ToString();
        if(!IsPosSent)
        UDPSent.Instance.Send(PosStr + "j"+ ColorStr + "j" +TouchStr);
        //Debug.Log(color.ToString());
    }

    public void GetPos(Touch touch)
    {
        float x = touch.position.x / Screen.width;
        float y = (touch.position.y - (Screen.height - Screen.width)) / Screen.width;

        x = Mathf.Clamp(x, 0f, 1f);
        y = Mathf.Clamp(y, 0f, 1f);

        PosStr = "("+x.ToString("0.000") + ","+y.ToString("0.000" + ")");
        if (!IsColorSent)
        UDPSent.Instance.Send(PosStr + "j"+ ColorStr + "j" +TouchStr);
    }

    private float Distance(float x, float y)
    {
        return Vector2.Distance(new Vector2(x, y), Center);
    }

    public void Release()
    {
        ColorStr = CurrentColor.ToString();
    }
}
