using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Image[] hpHearts;

    [Header("Level")]
    [SerializeField] private Image expGaugeBar;
    [SerializeField] private Text lvText;
    [SerializeField] private Text expText;

    [SerializeField] private Image levelupBlack;
    [SerializeField] private Text levelupText;

    [SerializeField] private Image durationGauge;

    [Header("Game Over")]
    [SerializeField] private Image warning;
    [SerializeField] private Image gameOverWindow;
    [SerializeField] private Text gameOverText;

    [Header("Skill")]
    [SerializeField] private Image skillGauge;
    [SerializeField] private Image skillOverlay;

    [SerializeField] private Text comboText;

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
        levelupBlack.gameObject.SetActive(true);
        levelupBlack.color = new Color(0, 0, 0, 0);
        levelupBlack.DOFade(0.3f, 0.75f);

        levelupText.rectTransform.anchoredPosition = new Vector2(-1500, 0);
        levelupText.rectTransform.DOAnchorPosX(0, 0.75f).OnComplete(() =>
        {
            levelupText.rectTransform.DOAnchorPosX(1500, 0.75f);
            levelupBlack.DOFade(0, 0.75f).OnComplete(() =>
            {
                levelupBlack.gameObject.SetActive(false);
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
        durationGauge.fillAmount = timerAmount;
    }
    public void UpdateSkill(float gaugeAmount)
    {
        skillGauge.fillAmount = gaugeAmount * 0.78f + 0.22f;
    }

    public void ActiveSkill()
    {
        skillOverlay.gameObject.SetActive(true);
        skillOverlay.color = new Color(1, 208 / 255f, 0, 0.1f);
        skillOverlay.DOFade(0, 0.75f).SetLoops(-1, LoopType.Yoyo);
    }
    public void DeActiveSkill()
    {
        skillOverlay.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        gameOverWindow.gameObject.SetActive(true);
        gameOverText.text = $"SSS급 천재헌터 김준우는\r\n레벨을 {Player.Instance.level} 밖에 달성하지 못했습니다.";
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
