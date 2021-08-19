using System;
using Assets.Scripts;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    public GameObject settingsGroup;
    public GameObject creditsGroup;
    public GameObject coinsGroup;
    public GameObject InstructionsGroup;
    public GameObject CoinButton;
    public GameObject CheatGroup;
    public GameController gameController;
    public SoundController soundController;

    static OptionsController _instance;
    GameObject options;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        options = GameObject.FindGameObjectWithTag("Options");
    }

    public static OptionsController Instance { get { return _instance; } }

    public void OnSettingsClick()
    {
        settingsGroup.SetActive(true);
        soundController.btnClickAudio.Play();
    }

    public void OnCreditsClick()
    {
        settingsGroup.SetActive(false);
        creditsGroup.SetActive(true);
        soundController.btnClickAudio.Play();
    }

    public void OnInstructionsClick()
    {
        settingsGroup.SetActive(false);
        InstructionsGroup.SetActive(true);
        soundController.btnClickAudio.Play();
    }

    public void OnCoinsClick()
    {
        coinsGroup.SetActive(true);
        soundController.btnClickAudio.Play();
    }

    public void OnInstaClick()
    {
        Application.OpenURL("https://www.instagram.com/kixcorreia/");
        soundController.btnClickAudio.Play();
    }

    public void OnTwitterClick()
    {
        Application.OpenURL("https://twitter.com/StudioG42425544");
        soundController.btnClickAudio.Play();
    }

    public void OnCheatClick()
    {
        CheatGroup.SetActive(true);
    }

    public void OnCoinEdited(string coins)
    {
        if (string.IsNullOrWhiteSpace(coins)) return;
        gameController.IncreaseCoins(int.Parse(coins));
    }

    public void OnScoreEdited(string score)
    {
        if (string.IsNullOrWhiteSpace(score)) return;
        gameController.SetCurrentScore(int.Parse(score));
    }

    internal void Deactivate()
    {
        CoinButton.SetActive(false);
        options.SetActive(false);
    }

    internal void Activate()
    {
        CoinButton.SetActive(true);
        options.SetActive(true);
    }
}
