using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TouchScript.Layers;

public class WaitPanel : BasePanel
{
    public VideoPlayer videoPlayer;
    public Button button,SetBtn;
    public CanvasGroup VideoCanvas;

    public float BackTime = 180;
    private float Back_Time;
    private bool IsBack;

    public SettingPanel settingPanel;
    private int Count = 0;
    private bool IsCount;
    private float CountDown = 10f;

    protected override void Start()
    {
        base.Start();
        if(Config.Instance)
        {
            BackTime = Config.Instance.configData.Backtime;
        }
        settingPanel.Hide();
        
    }

    public override void InitFind()
    {
        base.InitFind();
        videoPlayer = FindTool.FindChildComponent<VideoPlayer>(transform, "Video Player");
        button = FindTool.FindChildComponent<Button>(transform, "Video Player");
        SetBtn = FindTool.FindChildComponent<Button>(transform, "button");
        VideoCanvas = FindTool.FindChildComponent<CanvasGroup>(transform, "Video Player");
        settingPanel = FindTool.FindChildComponent<SettingPanel>(transform, "SettingPanel");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        button.onClick.AddListener(() =>
        {
            VideoCanvas.blocksRaycasts = false;
            VideoCanvas.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() => {
                PolygonDrawer.Instance.StateStr = "1";
                PolygonDrawer.Instance.SentMsg();
                UIState.SwitchPanel(PanelName.MainPanel);
            });    
        });

        SetBtn.onClick.AddListener(() => {
            IsCount = true;
            Count++;
            if(Count >= 10)
            {
                Count = 10;
            }
        });
    }

    public override void Open()
    {
        base.Open();
        VideoCanvas.Open();
        videoPlayer.url = Application.streamingAssetsPath + "/粒子待机.mp4";
        videoPlayer.Play();
        if(Config.Instance)
        {
            Config.Instance.Mesh.SetActive(false);
        }
    }

    public override void Hide()
    {
        base.Hide();
        videoPlayer.Stop();
        IsBack = true;
        Back_Time = BackTime;
        if (Config.Instance)
        {
            Config.Instance.Mesh.SetActive(true);
        }
        videoPlayer.targetTexture.Release();
    }

    private void Update()
    {
        if (Back_Time > 0 && IsBack)
        {
            Back_Time -= Time.deltaTime;
            //LogMsg.Instance.Log(Back_Time.ToString());
            if (Back_Time <= 0)
            {
                IsBack = false;
                if (Config.Instance)
                {
                    Config.Instance.Mesh.SetActive(false);
                }
                PolygonDrawer.Instance.StateStr = "0";
                PolygonDrawer.Instance.SentMsg();
                UIState.SwitchPanel(PanelName.WaitPanel);
                GC.Collect();
            }
            if (Input.touchCount > 0)
            {
                Back_Time = BackTime;
            }
        }

        if(IsCount && CountDown > 0)
        {
            CountDown -= Time.deltaTime;
            if(Count == 10)
            {
                IsCount = false;
                CountDown = 10f;
                Count = 0;
                settingPanel.Open();
            }

            if(CountDown <= 0)
            {
                IsCount = false;
                CountDown = 10f;
                Count = 0;
            }
        }
    }
}
