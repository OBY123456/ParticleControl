using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;
using System;

public class LineMove : MonoBehaviour
{
    public Transform trans0;
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
        GetComponent<PressGesture>().Pressed += pressHandler;
        GetComponent<ReleaseGesture>().Released += releasdHandler;       
    }

    void Start()
    {
        line.material = ma;
        line.SetPosition(0,trans0.position);
        line.SetPosition(1,transform.position);
        trans0.gameObject.SetActive(false);
    }

    private void releasdHandler(object sender, EventArgs e)
    {
        if(PolygonDrawer.Instance)
        {
            PolygonDrawer.Instance.IsPosSent = false;
            PolygonDrawer.Instance.PosStr = "0.000-0.000";
        }

        //Debug.Log("ËÉ¿ª");
        IsStart = false;
        transform.position = trans0.position;
        trans0.gameObject.SetActive(false);
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
        trans0.gameObject.SetActive(true);
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

            if (PolygonDrawer.Instance &&!PolygonDrawer.Instance.IsColorSent)
            {
                PolygonDrawer.Instance.IsPosSent = true;
            }

            Vector3 vector3 = Camera.main.ScreenToWorldPoint(touch.position);

            float x = vector3.x;
            float y = vector3.y;

            x = Mathf.Clamp(x, MinX, MaxX);
            y = Mathf.Clamp(y, MinY, MaxY);

            if(PolygonDrawer.Instance)
            PolygonDrawer.Instance.GetPos(touch);

            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), 0.09f);
            //transform.position = new Vector3(x, y, 0);
            line.SetPosition(1, transform.position);
        }
    }

}
