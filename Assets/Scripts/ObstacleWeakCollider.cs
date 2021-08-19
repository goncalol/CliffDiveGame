using UnityEngine;

public class ObstacleWeakCollider : ObstacleCollider
{ 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<Mover>().PartialDestroyMover();            
            collision.gameObject.GetComponent<Player>().EnteredCollision();
            shaker.TriggerShake();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (collision.gameObject.name == "WarningController" && warningPrefab != null)
        {
            warning = Instantiate(warningPrefab, warningPos, warningRotation, gameController.InstanciationPlace);
        }
        else if (collision.gameObject.tag == "WarningRed" && IsTop)
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "WarningController" && warningPrefab != null)
        {
            Destroy(warning);
        }
    }
}
