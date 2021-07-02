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
    public Transform Sphere, Sphere1, Sphere2, Sphere3;

    public SpereType spereType;

    private Vector3 Pos;

    private float MinX = -3.6f;
    private float MaxX = 3.6f;
    private float MinY = -2.4f;
    private float MaxY = 4.9f;

    private int finger = -1;
    Touch touch;

    private void Awake()
    {
        GetComponent<PressGesture>().Pressed += pressHandler;
        GetComponent<ReleaseGesture>().Released += releasdHandler;

        Pos = transform.position;
    }

    private void Start()
    {
        if(PolygonDrawer.Instance)
        {
            Sphere = PolygonDrawer.Instance.vertices[0];
            Sphere1 = PolygonDrawer.Instance.vertices[1];
            Sphere2 = PolygonDrawer.Instance.vertices[3];
            Sphere3 = PolygonDrawer.Instance.vertices[2];
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
                break;
            case SpereType.Sphere1:
                PolygonDrawer.Instance.InputPos[1] = 0f;
                break;
            case SpereType.Sphere2:
                PolygonDrawer.Instance.InputPos[2] = 0f;
                break;
            case SpereType.Sphere3:
                PolygonDrawer.Instance.InputPos[3] = 0f;
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

            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == finger)
                {
                    touch = Input.touches[i];
                }
            }

            if (!PolygonDrawer.Instance.IsPosSent)
            {
                PolygonDrawer.Instance.IsColorSent = true;
            }

            Vector3 vector3 = Camera.main.ScreenToWorldPoint(touch.position);
            //Debug.Log("ID ==" + touch.fingerId);
            float x = vector3.x;
            float y = vector3.y;

            switch (spereType)
            {
                case SpereType.Sphere:
                    if (x >= Pos.x && y >= Pos.y)
                    {
                        x = Mathf.Clamp(x, Pos.x, Sphere1.position.x);
                        y = Mathf.Clamp(y, Pos.y, MaxY);

                        PolygonDrawer.Instance.GetColor(spereType,touch,x,y);
                    }
                    else if (x <= Pos.x && y >= Pos.y)
                    {
                        x = Mathf.Clamp(x, MinX, Sphere1.position.x);
                        y = Mathf.Clamp(y, Pos.y, MaxY);
                        PolygonDrawer.Instance.GetColor(spereType, touch, x, y);
                    }
                    else if (x <= Pos.x && y <= Pos.y)
                    {
                        x = Mathf.Clamp(x, MinX, Pos.x);
                        y = Mathf.Clamp(y, Sphere2.position.y, Pos.y);
                        PolygonDrawer.Instance.GetColor(spereType, touch, x, y);
                    }
                    else if (x > Pos.x && y < Pos.y)
                    {

                    }

                    break;
                case SpereType.Sphere1:

                    if (x >= Pos.x && y >= Pos.y)
                    {
                        x = Mathf.Clamp(x, Pos.x, MaxX);
                        y = Mathf.Clamp(y, Pos.y, MaxY);
                        PolygonDrawer.Instance.GetColor(spereType, touch, x, y);
                    }
                    else if (x <= Pos.x && y >= Pos.y)
                    {
                        x = Mathf.Clamp(x, Sphere1.position.x, MaxX);
                        y = Mathf.Clamp(y, Pos.y, MaxY);
                        PolygonDrawer.Instance.GetColor(spereType, touch, x, y);
                    }
                    else if (x <= Pos.x && y <= Pos.y)
                    {
                        //x = Mathf.Clamp(x, MinX, Pos.x);
                        //y = Mathf.Clamp(y, Down.position.y, Pos.y);
                    }
                    else if (x > Pos.x && y < Pos.y)
                    {
                        x = Mathf.Clamp(x, Pos.x, MaxX);
                        y = Mathf.Clamp(y, Sphere3.position.y, Pos.y);
                        PolygonDrawer.Instance.GetColor(spereType, touch, x, y);
                    }

                    break;
                case SpereType.Sphere2:

                    if (x >= Pos.x && y >= Pos.y)
                    {
                        //x = Mathf.Clamp(x, Pos.x, MaxX);
                        //y = Mathf.Clamp(y, Pos.y, Up.position.y);
                    }
                    else if (x <= Pos.x && y >= Pos.y)
                    {
                        x = Mathf.Clamp(x, MinX, Pos.x);
                        y = Mathf.Clamp(y, Pos.y, Sphere1.position.y);
                        PolygonDrawer.Instance.GetColor(spereType, touch, x, y);
                    }
                    else if (x <= Pos.x && y <= Pos.y)
                    {
                        x = Mathf.Clamp(x, MinX, Pos.x);
                        y = Mathf.Clamp(y, MinY, Pos.y);
                        PolygonDrawer.Instance.GetColor(spereType, touch, x, y);
                    }
                    else if (x > Pos.x && y < Pos.y)
                    {
                        x = Mathf.Clamp(x, Pos.x, Sphere3.position.x);
                        y = Mathf.Clamp(y, MinY, Pos.y);
                        PolygonDrawer.Instance.GetColor(spereType, touch, x, y);
                    }

                    break;
                case SpereType.Sphere3:

                    if (x >= Pos.x && y >= Pos.y)
                    {
                        x = Mathf.Clamp(x, Pos.x, MaxX);
                        y = Mathf.Clamp(y, Pos.y, Sphere1.position.y);
                        PolygonDrawer.Instance.GetColor(spereType, touch, x, y);
                    }
                    else if (x <= Pos.x && y >= Pos.y)
                    {
                        //x = Mathf.Clamp(x, Left.position.x, MaxX);
                        //y = Mathf.Clamp(y, Pos.y, MaxY);
                    }
                    else if (x <= Pos.x && y <= Pos.y)
                    {
                        x = Mathf.Clamp(x, Sphere2.position.x, Pos.x);
                        y = Mathf.Clamp(y, MinY, Pos.y);
                        PolygonDrawer.Instance.GetColor(spereType, touch, x, y);
                    }
                    else if (x > Pos.x && y < Pos.y)
                    {
                        x = Mathf.Clamp(x, Pos.x, MaxX);
                        y = Mathf.Clamp(y, MinY, Pos.y);
                        PolygonDrawer.Instance.GetColor(spereType, touch, x, y);
                    }

                    break;
                default:
                    break;
            }
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
