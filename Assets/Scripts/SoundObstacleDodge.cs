using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObstacleDodge : MonoBehaviour
{
    GameController gameController;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameController.PlayDodgeSound();
        }
    }
}
