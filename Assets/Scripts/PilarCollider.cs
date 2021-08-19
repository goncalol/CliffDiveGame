using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PilarCollider : MonoBehaviour
{
    Player player;
    ShakeBehaviour shaker;
    Button[] buttons;
    bool IsPlayerInside;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        shaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ShakeBehaviour>();
        buttons = GameObject.FindGameObjectsWithTag("Button").Select(e => e.GetComponent<Button>()).ToArray();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (IsPlayerInside) return;
            IsPlayerInside = true;
            foreach (var btn in buttons)
            {
                btn.CoolDownButton();
            }
            player.EnteredSideCollision(false);
            shaker.TriggerShake();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsPlayerInside = false;
        }
    }
}
