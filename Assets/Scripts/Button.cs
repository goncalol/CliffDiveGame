using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerDownHandler
{
    private bool cooldown = false;
    public bool ToMoveLeft;
    public GameObject player;
    GameController gameController;
    Player p;

    void Start()
    {
        p=player.GetComponent<Player>();
        gameController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameController.GetGameState() == GameState.Started)
        {
            if (!cooldown)// == false && !p.IsColliding())
            {
                if (ToMoveLeft) p.MoveLeft();
                else p.MoveRight();

            }
        }
        else if(gameController.GetGameState() == GameState.Standby)
        {
            gameController.StartGame();
        }
    }

    public void CoolDownButton()
    {
        cooldown = true;
        Invoke("ResetCooldown", 0.2f);
    }

    void ResetCooldown()
    {
        cooldown = false;
    }
}
