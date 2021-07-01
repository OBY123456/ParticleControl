using System;
using System.Collections;
using System.Collections.Generic;
using MTFrame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class WaitPanel : MonoBehaviour
{
    public UIButton[] buttons;
    
    //public Text text;

    private void Awake()
    {
        buttons = transform.Find("Buttons").GetComponentsInChildren<UIButton>();
        //text = transform.Find("Text").GetComponent<Text>();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].OnClick += Onclick;
        }
        //text.text = Screen.currentResolution.ToString();
    }

    private void Onclick(BaseButton obj)
    {
        if(PolygonDrawer.Instance && Input.touchCount <= 1)
        {
            int num = int.Parse(obj.name);
            PolygonDrawer.Instance.SetMaterial(num, obj.GetComponent<Image>().color);
            foreach (var item in buttons)
            {
                if(!obj.name.Contains(item.name))
                {
                    item.ResetButton();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Input.mousePosition.y > (Screen.height - Screen.width))
            {
                Vector2 vector2 = new Vector2(Input.mousePosition.x / Screen.width, (Input.mousePosition.y - (Screen.height - Screen.width))/ Screen.width);
                //Debug.Log(vector2.ToString());
            }

        }
    }
}
