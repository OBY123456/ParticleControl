using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;
using System;

public class LineMove : MonoBehaviour
{
    Vector3 trans0;
    public LineRenderer line;
    GameObject obj;
    public Material ma;

    private float MinX = -3.6f;
    private float MaxX = 3.6f;
    private float MinY = -2.4f;
    private float MaxY = 4.9f;

    private int finger = -1;
    Touch touch;

    private void Awake()
    {
        trans0 = transform.position;

        GetComponent<PressGesture>().Pressed += pressHandler;
        GetComponent<ReleaseGesture>().Released += releasdHandler;
        
    }

    void Start()
    {
        line.material = ma;
        line.SetPosition(0,trans0);
        line.SetPosition(1,transform.position);
    }

    private void releasdHandler(object sender, EventArgs e)
    {
        PolygonDrawer.Instance.IsPosSent = false;
        PolygonDrawer.Instance.PosStr = "a";
        Debug.Log("ËÉ¿ª");
        IsStart = false;
        transform.position = trans0;
        finger = -1;
        line.SetPosition(1, transform.position);
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
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    Vector3 ve = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        //    transform.position = new Vector3(ve.x, ve.y, 0);
        //    line.SetPosition(1, transform.position);
        //}
        if (IsStart && finger != -1)
        {

            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == finger)
                {
                    touch = Input.touches[i];
                }
            }

            if (!PolygonDrawer.Instance.IsColorSent)
            {
                PolygonDrawer.Instance.IsPosSent = true;
            }

            Vector3 vector3 = Camera.main.ScreenToWorldPoint(touch.position);

            float x = vector3.x;
            float y = vector3.y;

            x = Mathf.Clamp(x, MinX, MaxX);
            y = Mathf.Clamp(y, MinY, MaxY);

            PolygonDrawer.Instance.GetPos(touch);

            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), 0.1f);
            line.SetPosition(1, transform.position);
        }
    }

}
