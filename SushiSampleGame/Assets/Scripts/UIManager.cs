using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using CASP.SoundManager;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text ScoreText;
    [SerializeField] TMP_Text EarnedMoneyText;
    [SerializeField] TMP_Text MoneyWin;

    [SerializeField] Image VolumeImage;
    [SerializeField] Sprite VolumeONImage;
    [SerializeField] Sprite VolumeOFFImage;
    [SerializeField] Image VibrationImage;


    [SerializeField] GameObject SettingsPanel;
    [SerializeField] GameObject SettingsMenu;
    [SerializeField] Image SettingPanelImage;

    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject WinMenu;
    [SerializeField] Image UnlockPercentImage;
    [SerializeField] TMP_Text PercentText;
    [SerializeField] GameObject Tap2Play;
    [SerializeField] GameObject GetLabel;

    [SerializeField] GameObject loadingScreen;


    public PlayerController playerScript;


    bool volumeActive = true;
    bool vibrationActive = true;



    public int Score;
    public int EarnedMoney;


    public static UIManager instance;
    int a = 74;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        StartCoroutine(LoadingScreenOff());
        //Tap2Play.transform.DOScale(1.1f,0.5f).SetLoops(-1,LoopType.Yoyo);
        EarnedMoney = 0;
        UpdateScore(2);
        playerScript.SpeedMultiplier = 0;

    }


IEnumerator LoadingScreenOff()
{
    yield return new WaitForSeconds(2f);
    loadingScreen.SetActive(false);
}


    public void UpdateScore(int coin)
    {
        Score += coin;
        //ScoreText.text = Score.ToString() + " $";
    }

    public void UpdateEarnedMoney(int Money)
    {
        EarnedMoney += Money;
        EarnedMoneyText.text = EarnedMoney.ToString();
        MoneyWin.text = EarnedMoney.ToString();
    }


    public void Volume()
    {
        volumeActive = !volumeActive;
        if (volumeActive)
        {
            VolumeImage.sprite = VolumeONImage;
        }
        if (!volumeActive)
        {
            VolumeImage.sprite = VolumeOFFImage;

        }
    }

    public void Vibration()
    {
        vibrationActive = !vibrationActive;
        if (vibrationActive)
        {
            VibrationImage.sprite = VolumeONImage;
        }
        if (!vibrationActive)
        {
            VibrationImage.sprite = VolumeOFFImage;

        }
    }




    public void OpenSettingsPanel()
    {
        SettingsPanel.SetActive(true);
        SettingsMenu.transform.DOScale(1f, 0.2f);
        playerScript.SpeedMultiplier = 0;
        DOTween.To(() => SettingPanelImage.color, x => SettingPanelImage.color = x, new Color32(32, 32, 32, 180), 0.3f);
    }
    public void CloseSettingsPanel()
    {
        DOTween.To(() => SettingPanelImage.color, x => SettingPanelImage.color = x, new Color32(32, 32, 32, 0), 0.3f);

        SettingsMenu.transform.DOScale(0f, 0.2f).OnComplete(() =>
        {
            SettingsPanel.SetActive(false);
            playerScript.SpeedMultiplier = 30;


        });
    }

    public void OpenWinPanel()
    {
        var seq = DOTween.Sequence();
        seq.Append(WinPanel.transform.DOScale(0f, 0f)).OnComplete(() =>
        {
            WinPanel.SetActive(true);
        SoundManager.instance.Play("OpenWinPanel", true);

            WinPanel.transform.DOScale(1f, 0.1f).OnComplete(() =>
            {
                DOTween.To(() => UnlockPercentImage.fillAmount, x => UnlockPercentImage.fillAmount = x, 0.74f, 0.5f);
                PercentText.transform.DOScale(new Vector3(PercentText.transform.localScale.x+0.2f,PercentText.transform.localScale.y+0.2f,PercentText.transform.localScale.z+0.2f),0.2f).SetLoops(1,LoopType.Yoyo);
                GetLabel.transform.DOScale(1.1f,0.5f).SetLoops(-1,LoopType.Yoyo);
                DOTween.To(() => int.Parse(PercentText.text), x => PercentText.text = x.ToString(),a, 1f);
            });

        });

    }
    public void CloseWinPanel()
    {
        WinPanel.transform.DOScale(0f, 0.1f).OnComplete(() =>
        {

        });


    }

    public void tap2Play()
    {
        Tap2Play.SetActive(false);
        playerScript.SpeedMultiplier = 25;
    }


}

