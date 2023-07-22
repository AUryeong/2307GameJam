using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : Singleton<TitleManager>
{

    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Image settingWindow;

    [SerializeField] private Slider settingSfxSlider;
    [SerializeField] private Slider settingBgmSlider;
    [SerializeField] private Button settingCloseButton;

    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Image howToPlayWindow;
    [SerializeField] private Button howToPlayCloseButton;

    [SerializeField] private Button gameExitButton;

    protected override void OnCreated()
    {
        base.OnCreated();

        gameStartButton.onClick.RemoveAllListeners();
        gameStartButton.onClick.AddListener(GameStart);

        settingButton.onClick.RemoveAllListeners();
        settingButton.onClick.AddListener(Setting);

        settingCloseButton.onClick.RemoveAllListeners();
        settingCloseButton.onClick.AddListener(SettingClose);

        howToPlayButton.onClick.RemoveAllListeners();
        howToPlayButton.onClick.AddListener(HowToPlay);

        howToPlayCloseButton.onClick.RemoveAllListeners();
        howToPlayCloseButton.onClick.AddListener(HowToPlayClose);

        gameExitButton.onClick.RemoveAllListeners();
        gameExitButton.onClick.AddListener(Exit);

        settingSfxSlider.onValueChanged.RemoveAllListeners();
        settingSfxSlider.onValueChanged.AddListener((value) => SoundManager.Instance.UpdateVolume(ESoundType.SFX, value));

        settingBgmSlider.onValueChanged.RemoveAllListeners();
        settingBgmSlider.onValueChanged.AddListener((value) => SoundManager.Instance.UpdateVolume(ESoundType.BGM, value));

    }

    private void Start()
    {
        SoundManager.Instance.PlaySound("title", ESoundType.BGM);
    }
    private void GameStart()
    {
        SceneManager.Instance.SceneLoad(SceneType.InGame);
        SoundManager.Instance.PlaySound("button");
    }

    private void Setting()
    {
        settingWindow.gameObject.SetActive(true);
        SoundManager.Instance.PlaySound("button");
    }

    private void SettingClose()
    {
        settingWindow.gameObject.SetActive(false);
        SoundManager.Instance.PlaySound("button");
    }

    private void HowToPlay()
    {
        howToPlayWindow.gameObject.SetActive(true);
        SoundManager.Instance.PlaySound("button");
    }
    private void HowToPlayClose()
    {
        howToPlayWindow.gameObject.SetActive(false);
        SoundManager.Instance.PlaySound("button");
    }

    private void Exit()
    {
        SoundManager.Instance.PlaySound("button");
        Application.Quit();
    }
}
