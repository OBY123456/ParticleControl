using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

[ExecuteInEditMode]
public class Sphere0 : MonoBehaviour
{
    public SpereType spereType;

    private Vector3 Pos;
    private Vector2 ScreenPos;

    private float MinX = -3.6f;
    private float MaxX = 3.6f;
    private float MinY = -2.4f;
    private float MaxY = 4.9f;

    private int finger = -1;
    Touch touch;

    private float Lenth;

    private void Awake()
    {
        GetComponent<PressGesture>().Pressed += pressHandler;
        GetComponent<ReleaseGesture>().Released += releasdHandler;

        Pos = transform.position;
        ScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        switch (spereType)
        {
            case SpereType.Sphere:
                Lenth = Mathf.Sqrt((float)(Math.Pow(MaxY - Pos.y, 2.0f) + Math.Pow(Pos.x - MinX, 2.0f)));
                break;
            case SpereType.Sphere1:
                Lenth = Mathf.Sqrt((float)(Math.Pow(MaxY - Pos.y, 2.0f) + Math.Pow(MaxX - Pos.x, 2.0f)));
                break;
            case SpereType.Sphere2:
                Lenth = Mathf.Sqrt((float)(Math.Pow(Pos.y - MinY, 2.0f) + Math.Pow(Pos.x - MinX, 2.0f))); 
                break;
            case SpereType.Sphere3:
                Lenth = Mathf.Sqrt((float)(Math.Pow(Pos.y - MinY, 2.0f) + Math.Pow(MaxX - Pos.x, 2.0f)));
                break;
            default:
                break;
        }
    }

    private void releasdHandler(object sender, EventArgs e)
    {
        PolygonDrawer.Instance.IsColorSent = false;
        IsStart = false;
        transform.position = Pos;
        finger = -1;
        switch (spereType)
        {
            case SpereType.Sphere:
                PolygonDrawer.Instance.InputPos[0] = 0f;
                PolygonDrawer.Instance.Ratio[0] = 0f;
                break;
            case SpereType.Sphere1:
                PolygonDrawer.Instance.InputPos[1] = 0f;
                PolygonDrawer.Instance.Ratio[1] = 0f;
                break;
            case SpereType.Sphere2:
                PolygonDrawer.Instance.InputPos[2] = 0f;
                PolygonDrawer.Instance.Ratio[2] = 0f;
                break;
            case SpereType.Sphere3:
                PolygonDrawer.Instance.InputPos[3] = 0f;
                PolygonDrawer.Instance.Ratio[3] = 0f;
                break;
            default:
                break;
        }
        PolygonDrawer.Instance.Release();
    }

    private void pressHandler(object sender, EventArgs e)
    {
        Touch[] touches = Input.touches;
        for (int i = 0; i < touches.Length; i++)
        {
            if (touches[i].phase == TouchPhase.Began)
            {
                finger = touches[i].fingerId;
            }
        }

        IsStart = true;
    }

    bool IsStart;

    private void Update()
    {
        if (IsStart && finger != -1)
        {
            try
            {
                for (int i = 0; i < Input.touches.Length; i++)
                {
                    if (Input.touches[i].fingerId == finger)
                    {
                        touch = Input.touches[i];
                    }
                }
            }
            catch
            {
                finger = -1;
            }

            if (!PolygonDrawer.Instance.IsPosSent)
            {
                PolygonDrawer.Instance.IsColorSent = true;
            }

            Vector3 vector3 = Camera.main.ScreenToWorldPoint(touch.position);
            float x = vector3.x;
            float y = vector3.y;

            switch (spereType)
            {
                case SpereType.Sphere:
                    x = Mathf.Clamp(x, MinX, Pos.x);
                    y = Mathf.Clamp(y, Pos.y, MaxY);
                    PolygonDrawer.Instance.Ratio[0] = Mathf.Clamp(Mathf.Sqrt((float)(Math.Pow(x - Pos.x, 2.0f) + Math.Pow(y - Pos.y, 2.0f)))/Lenth,0,1);
                    break;
                case SpereType.Sphere1:
                    x = Mathf.Clamp(x, Pos.x, MaxX);
                    y = Mathf.Clamp(y, Pos.y, MaxY);
                    PolygonDrawer.Instance.Ratio[1] = Mathf.Clamp(Mathf.Sqrt((float)(Math.Pow(x - Pos.x, 2.0f) + Math.Pow(y - Pos.y, 2.0f))) / Lenth, 0, 1);
                    break;
                case SpereType.Sphere2:
                    x = Mathf.Clamp(x, MinX, Pos.x);
                    y = Mathf.Clamp(y, MinY, Pos.y);
                    PolygonDrawer.Instance.Ratio[2] = Mathf.Clamp(Mathf.Sqrt((float)(Math.Pow(x - Pos.x, 2.0f) + Math.Pow(y - Pos.y, 2.0f))) / Lenth, 0, 1);
                    break;
                case SpereType.Sphere3:
                    x = Mathf.Clamp(x, Pos.x, MaxX);
                    y = Mathf.Clamp(y, MinY, Pos.y);
                    PolygonDrawer.Instance.Ratio[3] = Mathf.Clamp(Mathf.Sqrt((float)(Math.Pow(x - Pos.x, 2.0f) + Math.Pow(y - Pos.y, 2.0f))) / Lenth, 0, 1);
                    break;
                default:
                    break;
            }

            PolygonDrawer.Instance.GetColor(spereType, touch, x, y);
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), 0.1f);
        }
    }
}

public enum SpereType
{
    Sphere,
    Sphere1,
    Sphere2,
    Sphere3,
}
