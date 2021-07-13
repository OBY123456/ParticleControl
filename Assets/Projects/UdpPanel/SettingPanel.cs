using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SettingPanel : BasePanel
{
    public InputField IpInput, PostInput, TimeInput;
    public Button SaveBtn, CloseBtn;

    protected override void Start()
    {
        base.Start();
        Reset();
    }

    public override void InitFind()
    {
        base.InitFind();
        IpInput = FindTool.FindChildComponent<InputField>(transform, "Group/InputGroup/InputField");
        PostInput = FindTool.FindChildComponent<InputField>(transform, "Group/InputGroup (1)/InputField");
        TimeInput = FindTool.FindChildComponent<InputField>(transform, "Group/InputGroup (2)/InputField");
        SaveBtn = FindTool.FindChildComponent<Button>(transform, "SaveButton");
        CloseBtn = FindTool.FindChildComponent<Button>(transform, "CloseButton");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        SaveBtn.onClick.AddListener(() => {
            if(Config.Instance)
            {
                if(!string.IsNullOrEmpty(IpInput.text))
                {
                    Config.Instance.configData.ip = IpInput.text;
                }

                if (!string.IsNullOrEmpty(PostInput.text))
                {
                    Config.Instance.configData.Port = int.Parse(PostInput.text);
                }

                if (!string.IsNullOrEmpty(TimeInput.text))
                {
                    Config.Instance.configData.Backtime = int.Parse(TimeInput.text);
                    UIManager.GetPanel<WaitPanel>(WindowTypeEnum.ForegroundScreen).BackTime = int.Parse(TimeInput.text);
                }

                StartCoroutine(ResetSocket());
            }
        });

        CloseBtn.onClick.AddListener(() => {
            Hide();
        });
    }



    public override void Open()
    {
        base.Open();
        //EventSystem.current.SetSelectedGameObject(IpInput.gameObject);
    }

    public override void Hide()
    {
        base.Hide();
        Reset();
    }

    private void Reset()
    {
        IpInput.text = "";
        PostInput.text = "";
        TimeInput.text = "";
    }

    private IEnumerator ResetSocket()
    {
        UDPSent.Instance.Close();
        yield return new WaitForSeconds(0.1f);
        UDPSent.Instance.Init();
    }
}
