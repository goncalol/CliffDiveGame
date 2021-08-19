using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public Sprite SoundOnTexture;
    public Sprite SoundOffTexture;
    public Sprite MusicOnTexture;
    public Sprite MusicOffTexture;
    public GameObject SoundIcon;
    public GameObject MusicIcon;

    public AudioSource rocksAudio;
    public AudioSource windAudio;
    public AudioSource dodgeAudio;
    public AudioSource smashAudio;
    public AudioSource btnClickAudio;
    public AudioSource coinAudio;
    public AudioSource rockBreakAudio;
    public AudioSource barrierHitAudio;
    public AudioSource smokeAudio;
    public AudioSource apitoAudio;
    public AudioSource caixaAudio;

    public AudioSource initialMusic;
    public AudioSource mainMusic;

    private static SoundController _instance;
    GameController gameController;
    Player player;
    bool isSoundOff;
    bool isMusicOff;
    Image soundBtnImg;
    Image musicBtnImg;
    IEnumerator FadeOutCoroutine;
    IEnumerator FadeInCoroutine;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        soundBtnImg = SoundIcon.GetComponent<Image>();
        musicBtnImg = MusicIcon.GetComponent<Image>();
        isSoundOff = PlayerPrefs.GetInt("Sound", 1) == 0 ? true : false;
        isMusicOff = PlayerPrefs.GetInt("Music", 1) == 0 ? true : false;
        ChangeSoundIcon(true);
        ChangeMusicIcon(true);
    }


    public static SoundController Instance { get { return _instance; } }

    public void OnSoundIconClick()
    {
        isSoundOff = !isSoundOff;
        ChangeSoundIcon(false);
    }

    public void OnMusicIconClick()
    {
        isMusicOff = !isMusicOff;
        ChangeMusicIcon(false);
    }

    private void ChangeSoundIcon(bool isInit)
    {
        if (isSoundOff)
        {
            soundBtnImg.overrideSprite = SoundOffTexture;
            SoundOff();
        }
        else
        {
            soundBtnImg.overrideSprite = SoundOnTexture;
            SoundOn(isInit);
        }
    }

    private void ChangeMusicIcon(bool isInit)
    {
        if (isMusicOff)
        {
            musicBtnImg.overrideSprite = MusicOffTexture;
            MusicOff(isInit);
        }
        else
        {
            musicBtnImg.overrideSprite = MusicOnTexture;
            MusicOn(isInit);
        }
    }

    private void SoundOn(bool isInit)
    {
        if (!isInit) btnClickAudio.Play();
        player.SoundOn();
        rocksAudio.mute = false;
        windAudio.mute = false;
        dodgeAudio.mute = false;
        smashAudio.mute = false;
        btnClickAudio.mute = false;
        coinAudio.mute = false;
        rockBreakAudio.mute = false;
        barrierHitAudio.mute = false;
        smokeAudio.mute = false;
        apitoAudio.mute = false;
        caixaAudio.mute = false;

        PlayerPrefs.SetInt("Sound", 1);
    }

    private void SoundOff()
    {
        player.SoundOff();
        btnClickAudio.mute = true;
        rocksAudio.mute = true;
        windAudio.mute = true;
        dodgeAudio.mute = true;
        smashAudio.mute = true;
        coinAudio.mute = true;
        rockBreakAudio.mute = true;
        barrierHitAudio.mute = true;
        smokeAudio.mute = true;
        apitoAudio.mute = true;
        caixaAudio.mute = true;

        PlayerPrefs.SetInt("Sound", 0);
    }

    private void MusicOn(bool isInit)
    {
        if (!isSoundOff && !isInit) btnClickAudio.Play();

        if (!isInit)
        {
            mainMusic.volume = 1;
            mainMusic.Play();
        }
        else
        {
            initialMusic.volume = 1;
            initialMusic.Play();
        }

        PlayerPrefs.SetInt("Music", 1);
    }

    private void MusicOff(bool isInit)
    {
        if (!isSoundOff && !isInit) btnClickAudio.Play();

        if (FadeOutCoroutine != null) StopCoroutine(FadeOutCoroutine);
        if (FadeInCoroutine != null) StopCoroutine(FadeInCoroutine);
        mainMusic.Stop();
        initialMusic.Stop();
        PlayerPrefs.SetInt("Music", 0);
    }

    public void StartInitialMusic()
    {
        if (PlayerPrefs.GetInt("Music", 1) == 0) return;
        initialMusic.Play();
    }

    public void StartMainMusic()
    {
        if (PlayerPrefs.GetInt("Music", 1) == 0) return;

        if (FadeOutCoroutine != null)
        {
            StopCoroutine(FadeOutCoroutine);
            if (FadeInCoroutine != null) StopCoroutine(FadeInCoroutine);
            FadeInCoroutine = FadeIn();
            StartCoroutine(FadeInCoroutine);
        }
        else
        {
            initialMusic.Stop();
            mainMusic.Play();
        }

    }

    public void FadeOutMusic()
    {
        FadeOutCoroutine = FadeOut();
        StartCoroutine(FadeOutCoroutine);
    }


    public IEnumerator FadeOut()
    {
        float startVolume = mainMusic.volume;

        while (mainMusic.volume > 0.2)
        {
            mainMusic.volume -= startVolume * Time.deltaTime / 3f;

            yield return null;
        }

    }

    public IEnumerator FadeIn()
    {
        float startVolume = mainMusic.volume;

        while (mainMusic.volume < 1)
        {
            mainMusic.volume += startVolume * Time.deltaTime / 0.5f;

            yield return null;
        }

    }
}
