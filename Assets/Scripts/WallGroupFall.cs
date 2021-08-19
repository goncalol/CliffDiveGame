using UnityEngine;

public class WallGroupFall : MonoBehaviour
{
    Vector2 initialPos;
    GameController gameController;

    private void Start()
    {
        initialPos = gameObject.transform.position;
        gameController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
    }

    public void ShowPlayer()
    {
        gameController.ShowPlayer();
    }

    public void OnAnimationComplete()
    {
        gameObject.GetComponent<Animator>().SetBool("Fall", false);
        gameObject.transform.position = initialPos;
        gameObject.SetActive(false);
        gameController.OnAnimationEnd();
    }
}
