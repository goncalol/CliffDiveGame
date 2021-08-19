using UnityEngine;
using UnityEngine.EventSystems;

public class Instructions : ExitUI
{
    public AudioSource btnClickAudio;

    public GameObject[] instructions;
    private int currentIndex = 0;


    protected override void PointerExtend()
    {
        currentIndex = 0;
        btnClickAudio.Play();
    }

    public void Next()
    {
        if(currentIndex<(instructions.Length-1))
        {
            instructions[currentIndex++].SetActive(false);
            instructions[currentIndex].SetActive(true);
        }

        btnClickAudio.Play();
    }

    public void Previous()
    {
        if (currentIndex > 0)
        {
            instructions[currentIndex--].SetActive(false);
            instructions[currentIndex].SetActive(true);
        }
        btnClickAudio.Play();
    }
}
