using UnityEngine;

public class WallMover : Mover {

    Vector3 initialPosition;

    protected override void ForStarters()
    {
        Stopped = true;
        initialPosition = gameObject.transform.position;
    }

    public override void OnAnimationEnd()
    {
       
    }


    void Update()
    {
        if (!Stopped)
        {
            if (transform.position.y >= 6.67)
                transform.position = new Vector2(transform.position.x, -3.257624f);
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }

    public override void ReloadMovers()
    {
        gameObject.transform.position = initialPosition;
    }
}
