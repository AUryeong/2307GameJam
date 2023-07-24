using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Image[] hpHearts;

    [Header("Level")]
    [SerializeField] private Image expGaugeBar;
    [SerializeField] private Text lvText;
    [SerializeField] private Text expText;

    [FormerlySerializedAs("levelupBlack")] [SerializeField] private Image backgroundBlack;
    [SerializeField] private Text levelupText;

    [Header("Clock")]
    [SerializeField] private Image clockDurationGauge;
    [SerializeField] private Image clockArrow;

    [Header("Game Over")]
    [SerializeField] private Image warning;
    [SerializeField] private Image gameOverWindow;
    [SerializeField] private Text gameOverText;

    [SerializeField] private Button restartButton;
    [SerializeField] private Button titleButton;

    [Header("Skill")]
    [SerializeField] private Image skillGauge;
    [SerializeField] private Image skillOverlay;

    [SerializeField] private Text comboText;
    [SerializeField] private Text feverOnText;
    [SerializeField] private Text feverOffText;

    protected override void OnCreated()
    {
        base.OnCreated();
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() =>
        {
            SceneManager.Instance.SceneLoad(SceneType.InGame);
            SoundManager.Instance.PlaySound("button");
        });

        titleButton.onClick.RemoveAllListeners();
        titleButton.onClick.AddListener(() =>
        {
            SceneManager.Instance.SceneLoad(SceneType.Title);
            SoundManager.Instance.PlaySound("button");
        });
    }

    public void UpdateHp(int hp)
    {
        for (int i = 0; i < hpHearts.Length; i++)
        {
            hpHearts[i].gameObject.SetActive(i < hp);
        }
    }

    public void HitWarning()
    {
        warning.gameObject.SetActive(true);
        warning.DOKill();

        warning.color = new Color(1,0,0,0.4f);
        warning.DOFade(0, 0.5f).OnComplete(() =>
        {
            warning.gameObject.SetActive(false);
        });
    }

    public void LevelUP()
    {
        backgroundBlack.gameObject.SetActive(true);
        backgroundBlack.color = new Color(0, 0, 0, 0);
        backgroundBlack.DOFade(0.3f, 0.75f);

        levelupText.rectTransform.anchoredPosition = new Vector2(-1500, 0);
        levelupText.rectTransform.DOAnchorPosX(0, 0.75f).OnComplete(() =>
        {
            levelupText.rectTransform.DOAnchorPosX(1500, 0.75f);
            backgroundBlack.DOFade(0, 0.75f).OnComplete(() =>
            {
                backgroundBlack.gameObject.SetActive(false);
            });
        });
    }

    public void UpdateLevel(float level, float exp, float maxExp)
    {
        lvText.text = "LV. " + level;
        expText.text = Mathf.RoundToInt(exp / maxExp * 100) + "%";
        expGaugeBar.DOFillAmount(exp / maxExp, 0.2f);
    }

    public void UpdaeTimer(float timerAmount)
    {
        clockDurationGauge.fillAmount = timerAmount;
        clockArrow.rectTransform.rotation = Quaternion.Euler(0, 0, -360 * timerAmount);
    }
    public void UpdateSkill(float gaugeAmount)
    {
        skillGauge.fillAmount = gaugeAmount * 0.78f + 0.22f;
    }

    public void ActiveSkill()
    {
        feverOnText.gameObject.SetActive(true);
        backgroundBlack.gameObject.SetActive(true);
        
        backgroundBlack.DOKill();
        backgroundBlack.color = new Color(0, 0, 0, 0);
        backgroundBlack.DOFade(0.3f, 0.75f);

        feverOnText.rectTransform.anchoredPosition = new Vector2(-1700, 0);
        feverOnText.rectTransform.DOAnchorPosX(0, 0.75f).OnComplete(() =>
        {
            feverOnText.rectTransform.DOAnchorPosX(1700, 0.75f);
            backgroundBlack.DOFade(0, 0.75f).OnComplete(() =>
            {
                backgroundBlack.gameObject.SetActive(false);
                skillOverlay.gameObject.SetActive(true);
                skillOverlay.color = new Color(1, 208 / 255f, 0, 0.1f);
                skillOverlay.DOFade(0, 0.75f).SetLoops(-1, LoopType.Yoyo);
            });
        });
    }
    public void DeActiveSkill()
    {
        skillOverlay.gameObject.SetActive(false);
        
        feverOffText.gameObject.SetActive(true);
        backgroundBlack.gameObject.SetActive(true);
        
        backgroundBlack.DOKill();
        backgroundBlack.color = new Color(0, 0, 0, 0);
        backgroundBlack.DOFade(0.3f, 0.75f);

        feverOffText.rectTransform.anchoredPosition = new Vector2(-1500, 0);
        feverOffText.rectTransform.DOAnchorPosX(0, 0.75f).OnComplete(() =>
        {
            feverOffText.rectTransform.DOAnchorPosX(1500, 0.75f);
            backgroundBlack.DOFade(0, 0.75f).OnComplete(() =>
            {
                backgroundBlack.gameObject.SetActive(false);
            });
        });
    }

    public void GameOver()
    {
        gameOverWindow.gameObject.SetActive(true);
        gameOverText.text = $"고블린 슬레이엄 김준식은\r\n레벨을 {Player.Instance.level} 밖에 달성하지 못했습니다.";
    }

    public void UpdateCombo(int comboCount)
    {
        comboText.gameObject.SetActive(true);
        comboText.text = comboCount.ToString();

        comboText.rectTransform.DOKill();
        comboText.rectTransform.localScale = Vector3.one * 0.75f;
        comboText.rectTransform.DOScale(1.5f, 0.7f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            comboText.rectTransform.DOScale(0, 0.5f).OnComplete(() => comboText.gameObject.SetActive(false));
        });
        comboText.rectTransform.DORotate(new Vector3(0, 0, Random.Range(-30f, 30f)), 0.4f);

    }
}
