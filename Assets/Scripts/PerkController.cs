using System;
using TMPro;
using UnityEngine;

public class PerkController : MonoBehaviour
{
    public UnityEngine.UI.Button Rage;
    public UnityEngine.UI.Button Barrier;
    public TextMeshProUGUI RageText;
    public TextMeshProUGUI BarrierText;

    private int RageNr;
    private int BarrierNr;

    private void Start()
    {
        RageNr = PlayerPrefs.GetInt("Rage", 0);
        BarrierNr = PlayerPrefs.GetInt("Barrier", 0);
        RageText.SetText("x"+RageNr);
        BarrierText.SetText("x" + BarrierNr);
    }

    public void Activate(bool active)
    {
        gameObject.SetActive(active);
    }    

    public bool CanActivateRage()
    {
        if (RageNr == 0) return false;
        PlayerPrefs.SetInt("Rage", --RageNr);
        RageText.SetText("x" + RageNr);
        return true;
    }

    public bool CanActivateBarrier()
    {
        if (BarrierNr == 0) return false;
        PlayerPrefs.SetInt("Barrier", --BarrierNr);
        BarrierText.SetText("x" + BarrierNr);
        return true;
    }

    public int IncrementRage()
    {
        RageNr = PlayerPrefs.GetInt("Rage", 0) +1;
        PlayerPrefs.SetInt("Rage", RageNr);
        RageText.SetText("x" + RageNr);
        return RageNr;
    }

    public int IncrementBarrier()
    {
        BarrierNr = PlayerPrefs.GetInt("Barrier", 0) + 1;
        PlayerPrefs.SetInt("Barrier", BarrierNr);
        BarrierText.SetText("x" + BarrierNr);
        return BarrierNr;
    }
}
