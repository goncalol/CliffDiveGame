using UnityEngine;

public class ObstacleCollider : MonoBehaviour
{
    protected GameController gameController;
    protected GameObject warning;
    protected GameObject warningPrefab;
    protected Vector3 warningPos;
    protected Quaternion warningRotation;
    public bool IsTop;
    protected ShakeBehaviour shaker;
    private Mover mover;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
        shaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ShakeBehaviour>();
        mover =gameObject.transform.parent.GetComponent<Mover>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameController.isSuperSpeed)
            {
                if (IsTop || mover == null)//if is the special wall prefab --> should have is own class
                {
                    collision.gameObject.GetComponent<Player>().IsOnCollision = true;
                    PartialDestroyBigMover(collision);
                }
                else
                    mover.PartialDestroyMover();
                shaker.TriggerShake();
            }
            else
                gameController.GameOver();
        }
        else if(collision.gameObject.name == "WarningController" && warningPrefab!=null)
        {
            warning = Instantiate(warningPrefab, warningPos, warningRotation, gameController.InstanciationPlace);
        }else if(collision.gameObject.tag == "WarningRed" && IsTop)
        {
            Destroy(collision.gameObject);
        }
    }

    public void SetupWarnings(GameObject warningPrefab, Vector3 position, Quaternion rotation)
    {
        this.warningPrefab = warningPrefab;
        warningPos = position;
        warningRotation = rotation;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "WarningController" && warningPrefab != null)
        {
            Destroy(warning);
        }
    }

    private void PartialDestroyBigMover(Collider2D collision)
    {
        if (IsTop)
        {
            gameObject.transform.parent.GetComponent<BigMover>().PartialDestroyBigMover(collision.gameObject.GetComponent<Player>());
        }
        else if (mover == null)
        {
            gameObject.transform.parent.parent.GetComponent<BigMover>().PartialDestroyBigMover(collision.gameObject.GetComponent<Player>());
        }
    }
}
