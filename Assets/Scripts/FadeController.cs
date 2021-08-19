using UnityEngine;

public class FadeController : MonoBehaviour
{
    public Animator PlayerAnimator;

    public void OnFadeEnd()
    {
        gameObject.SetActive(false);
        PlayerAnimator.SetTrigger("Fall");
    }
}
