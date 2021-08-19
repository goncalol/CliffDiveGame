using UnityEngine;

public class SplashAnim : MonoBehaviour
{
    public void ShowStain()
    {
        gameObject.transform.parent.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
    }

    public void HideStain()
    {
        gameObject.transform.parent.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
    }
}
