using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;

public class UIButton : BaseButton
{
    public CanvasGroup canvasGroup;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = transform.GetChild(0).GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public override void TriggerClick()
    {
        base.TriggerClick();
        if (Input.touchCount <= 1)
        {
            transform.localScale = Vector3.one * 1.2f;
            canvasGroup.alpha = 1;
        }
    }

    public void ResetButton()
    {
        if(Input.touchCount <= 1)
        {
            transform.localScale = Vector3.one;
            canvasGroup.alpha = 0;
        }
    }
}
