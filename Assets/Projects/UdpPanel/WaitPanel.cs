using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;

public class WaitPanel : BasePanel
{
    public VideoPlayer videoPlayer;
    public Button button;
    public CanvasGroup VideoCanvas;

    private float BackTime = 180;
    private float Back_Time;
    private bool IsBack;

    public override void InitFind()
    {
        base.InitFind();
        videoPlayer = FindTool.FindChildComponent<VideoPlayer>(transform, "Video Player");
        button = FindTool.FindChildComponent<Button>(transform, "Video Player");
        VideoCanvas = FindTool.FindChildComponent<CanvasGroup>(transform, "Video Player");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        button.onClick.AddListener(() =>
        {
            VideoCanvas.blocksRaycasts = false;
            VideoCanvas.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() => {
                UIState.SwitchPanel(PanelName.MainPanel);
            });    
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
            LogMsg.Instance.Log(Back_Time.ToString());
            if (Back_Time <= 0)
            {
                IsBack = false;
                if (Config.Instance)
                {
                    Config.Instance.Mesh.SetActive(false);
                }
                UIState.SwitchPanel(PanelName.WaitPanel);
            }
            if (Input.touchCount > 0)
            {
                Back_Time = BackTime;
            }
        }
    }
}
