using System;
using System.Collections;
using System.Collections.Generic;
using MTFrame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    public UIButton[] buttons;

    public override void InitFind()
    {
        base.InitFind();
        buttons = FindTool.FindChildNode(transform,"Buttons").GetComponentsInChildren<UIButton>();
    }

    public override void InitEvent()
    {
        base.InitEvent();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].OnClick += Onclick;
        }
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
}
