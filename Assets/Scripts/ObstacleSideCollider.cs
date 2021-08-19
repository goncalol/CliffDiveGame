using System;
using System.Linq;
using UnityEngine;

public class ObstacleSideCollider : MonoBehaviour
{
    Player player;
    ShakeBehaviour shaker;
    Button[] buttons;
    public bool PushPlayerRight;
    public bool IsWall;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        shaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ShakeBehaviour>();
        buttons = GameObject.FindGameObjectsWithTag("Button").Select(e => e.GetComponent<Button>()).ToArray();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsWall)
        {
            if (!player.IsOnCollision && collision.gameObject.tag == "Player")
                Collide();
        }
        else
        {
            if (!player.IsOnCollision && collision.gameObject.tag == "Player" && !player.IsSuperSpeed())
                Collide();
        }
    }

    private void Collide()
    {
        player.IsOnCollision = true;
        foreach (var btn in buttons)
        {
            btn.CoolDownButton();
        }
        player.EnteredSideCollision(PushPlayerRight);
        shaker.TriggerShake();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.IsOnCollision = false;
        }
    }
}
