using Assets.Scripts;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject ShopGroup;
    public PerkController perks;
    public TextMeshProUGUI Heart50Price;
    public TextMeshProUGUI Heart100Price;
    public TextMeshProUGUI BarrierPrice;
    public TextMeshProUGUI RagePrice;
    public TextMeshProUGUI Heart50Indicator;
    public TextMeshProUGUI Heart100Indicator;
    public TextMeshProUGUI BarrierIndicator;
    public TextMeshProUGUI RageIndicator;
    public GameObject ConsentGroup;
    public GameObject NoteGroup;
    public TextMeshProUGUI NoteText;
    public TextMeshProUGUI CoinText;

    public int Coins;
    private int heart50Price;
    private int heart100Price;
    private int barrierPrice;
    private int ragePrice;
    private ItemType itemChosen;
    private Player player;

    private void Start()
    {
        Coins = PlayerPrefs.GetInt("Coins",0);
        CoinText.text = Coins.ToString();
        heart50Price = int.Parse(Heart50Price.text);
        heart100Price = int.Parse(Heart100Price.text);
        barrierPrice = int.Parse(BarrierPrice.text);
        ragePrice = int.Parse(RagePrice.text);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void ActivateShop()
    {
        SoundController.Instance.btnClickAudio.Play();
        Heart50Indicator.SetText("X" + PlayerPrefs.GetInt("Heart50", 0).ToString());
        Heart100Indicator.SetText("X" + PlayerPrefs.GetInt("Heart100", 0).ToString());
        BarrierIndicator.SetText("X" + PlayerPrefs.GetInt("Barrier", 0).ToString());
        RageIndicator.SetText("X"+PlayerPrefs.GetInt("Rage", 0).ToString());
        ShopGroup.SetActive(true);
    }

    public void Heart50()
    {
        SoundController.Instance.btnClickAudio.Play();
        if (PlayerPrefs.GetInt("Heart50", 0) == 1)
        {
            NoteText.SetText("You already bought a 50 heart container");
            NoteGroup.SetActive(true);
        }
        else
        {
            if (Coins >= heart50Price)
            {
                itemChosen = ItemType.Heart50;
                ConsentGroup.SetActive(true);
            }
            else
            {
                NoteText.SetText("You don't have enough coins to buy this item");
                NoteGroup.SetActive(true);
            }
        }
    }

    public void Heart100()
    {
        SoundController.Instance.btnClickAudio.Play();
        if (PlayerPrefs.GetInt("Heart100", 0) == 1)
        {
            NoteText.SetText("You already bought a 100 heart container");
            NoteGroup.SetActive(true);
        }
        else
        {
            if (Coins >= heart100Price)
            {
                itemChosen = ItemType.Heart100;
                ConsentGroup.SetActive(true);
            }
            else
            {
                NoteText.SetText("You don't have enough coins to buy this item");
                NoteGroup.SetActive(true);
            }
        }
    }

    public void Barrier()
    {
        SoundController.Instance.btnClickAudio.Play();
        if (PlayerPrefs.GetInt("Barrier", 0) ==99)
        {
            NoteText.SetText("You exceed the maximum items allowed");
            NoteGroup.SetActive(true);
        }
        else
        {
            if (Coins >= barrierPrice)
            {
                itemChosen = ItemType.Barrier;
                ConsentGroup.SetActive(true);
            }
            else
            {
                NoteText.SetText("You don't have enough coins to buy this item");
                NoteGroup.SetActive(true);
            }
        }
    }

    public void Rage()
    {
        SoundController.Instance.btnClickAudio.Play();
        if (PlayerPrefs.GetInt("Rage", 0) == 99)
        {
            NoteText.SetText("You exceed the maximum items allowed");
            NoteGroup.SetActive(true);
        }
        else
        {
            if (Coins >= ragePrice)
            {
                itemChosen = ItemType.Rage;
                ConsentGroup.SetActive(true);
            }
            else
            {
                NoteText.SetText("You don't have enough coins to buy this item");
                NoteGroup.SetActive(true);
            }
        }
    }

    public void HideConsentGroup()
    {
        SoundController.Instance.btnClickAudio.Play();
        ConsentGroup.SetActive(false);
        NoteGroup.SetActive(false);
    }

    public void AcceptedConsent()
    {
        ConsentGroup.SetActive(false);
        SoundController.Instance.caixaAudio.Play();

        switch (itemChosen)
        {
            case ItemType.Heart50:
                DecreaseCoins(heart50Price);
                player.IncreaseLife(50);
                Heart50Indicator.SetText("X1");
                break;
            case ItemType.Heart100:
                player.IncreaseLife(100);
                DecreaseCoins(heart100Price);
                Heart100Indicator.SetText("X1");
                break;
            case ItemType.Barrier:
                DecreaseCoins(barrierPrice);
                BarrierIndicator.SetText("X"+perks.IncrementBarrier());
                break;
            case ItemType.Rage:
                DecreaseCoins(ragePrice);
                RageIndicator.SetText("X" + perks.IncrementRage());
                break;
        }

    }

    public void IncreaseOneCoin()
    {
        PlayerPrefs.SetInt("Coins", ++Coins);
        CoinText.text = Coins.ToString();
    }

    private void DecreaseCoins(int c)
    {
        Coins -= c;
        PlayerPrefs.SetInt("Coins", Coins);
        CoinText.text = Coins.ToString();
    }


    public void IncreaseCoins(int c)
    {
        Coins += c;
        PlayerPrefs.SetInt("Coins", Coins);
        CoinText.text = Coins.ToString();
        SoundController.Instance.caixaAudio.Play();
    }
}
