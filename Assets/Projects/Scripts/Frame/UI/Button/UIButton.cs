using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class UIButton : BaseButton
{
    public Sprite[] sprites;
    private Vector2 LocalSize,LocalPos;
    private Vector2 NewSize = new Vector2(160,249);
    private Vector2 NewPos;

    protected override void Awake()
    {
        base.Awake();
        LocalSize = transform.GetComponent<RectTransform>().sizeDelta;
        LocalPos = transform.GetComponent<RectTransform>().localPosition;
        NewPos = new Vector2(LocalPos.x, -36);
        ResetButton();
    }

    public override void TriggerClick()
    {
        base.TriggerClick();
        if (Input.touchCount <= 1)
        {
            transform.GetComponent<Image>().sprite = sprites[1];
            transform.GetComponent<RectTransform>().sizeDelta = NewSize;
            transform.GetComponent<RectTransform>().localPosition = NewPos;
        }
    }

    public void ResetButton()
    {
        transform.GetComponent<Image>().sprite = sprites[0];
        transform.GetComponent<RectTransform>().sizeDelta = LocalSize;
        transform.GetComponent<RectTransform>().localPosition = LocalPos;
    }
}
