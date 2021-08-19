using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool IsGrouped;
    public GameObject CoinRight;
    public GameObject CoinLeft;

    GameObject player;
    bool playerPassed;
    GameController gameController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
    }

    void Update()
    {
        if (!playerPassed && transform.position.y>=player.transform.position.y && gameController.GetGameState()==Assets.Scripts.GameState.Started)
        {
            playerPassed = true;
            gameController.IncrementScore();
            if (IsGrouped)
            {
                gameController.ActivateGroupSpwan();
            }
        }
    }

    internal Mover GetMover()
    {
        if (IsGrouped)
        {
            return gameObject.GetComponentInParent<Mover>();
        }

        return gameObject.GetComponent<Mover>();
    }

    internal void Destroy()
    {
        if (IsGrouped)
        {
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }

        Destroy(gameObject);
    }

   public void SpawnCoin(int spawnIndex)//1 -> right; 0-> left; 2-> middle
    {
        if (spawnIndex == 2) return;

        if (UnityEngine.Random.Range(0, 3) == 0)
        {
            if (spawnIndex == 0)
            {
                CoinRight.SetActive(true);
            }
            else
            {
                CoinLeft.SetActive(true);
            }
        }
    }

    public void Hide()
    {
        if (CoinLeft != null && CoinRight != null)
        {
            CoinLeft.SetActive(false);
            CoinRight.SetActive(false);
        }
    }
}
