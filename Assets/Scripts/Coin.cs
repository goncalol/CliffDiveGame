using UnityEngine;

public class Coin : MonoBehaviour
{
    GameController gameController;
    ParticleSystem ps;
    SpriteRenderer render;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
        ps = gameObject.GetComponent<ParticleSystem>();
        render = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameController.soundController.coinAudio.Play();
            ps.Play();
            gameController.IncreaseCoins();
            render.enabled = false;
        }
    }
}
