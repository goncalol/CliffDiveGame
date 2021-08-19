using UnityEngine;

public class SideSplash : Mover
{
    protected override void ForStarters()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Splash");
    }

    void Update()
    {
        if (!Stopped)
        {
            if (transform.position.y >= 6.5)
            {
                gameController.DeleteMover(this);
                Destroy(gameObject);
            }
            else
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
        }
    }

    public override void ReloadMovers()
    {
    }
}