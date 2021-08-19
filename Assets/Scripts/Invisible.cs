using UnityEngine;

public class Invisible : MonoBehaviour
{
    public bool IsLeft;

    Vector3 initialPos;
    Player player;
    float speed = 4f;
    bool StartMove;

    void Start()
    {
        initialPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (StartMove && transform.position.y < 11)
            transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    public void ResetPos()
    {
        StartMove = false;
        transform.position = initialPos;
    }

    public void StartMovement()
    {
        StartMove = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (IsLeft)
                player.StopMovingLeft();
            else
                player.StopMovingRight();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (IsLeft)
                player.AllowMovingLeft();
            else
                player.AllowMovingRight();
        }
    }
}
