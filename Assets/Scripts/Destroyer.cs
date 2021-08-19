using UnityEngine;

public class Destroyer : MonoBehaviour
{
    GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            var obstacle = collision.gameObject.transform.parent.GetComponent<Obstacle>();
            obstacle.Destroy();
            gameController.DeleteMover(obstacle.GetMover());
        }
    }
}
