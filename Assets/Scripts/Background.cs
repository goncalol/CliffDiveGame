using UnityEngine;

public class Background : Mover
{
    Transform FirstBackground;
    Transform LastBackground;
    bool MoveOtherWay;

    protected override void ForStarters() {
        LastBackground = gameObject.transform.Find("3").gameObject.transform;
        FirstBackground = gameObject.transform.Find("2").gameObject.transform;
    }
    
    public override void ReloadMovers()
    {
        MoveOtherWay = true;
    }

    public override void OnAnimationEnd()
    {
        Stopped = false;
        MoveOtherWay = false;
    }

    void Update()
    {
        if (!Stopped && !MoveOtherWay)
        {
            if (LastBackground.position.y >= 0)
                transform.position = new Vector2(transform.position.x, 0);
            transform.Translate(Vector3.up * speed * Time.deltaTime / 5);
        }

        if(Stopped && MoveOtherWay)
        {
            if (FirstBackground.position.y <= 0)
                transform.position = new Vector2(transform.position.x, 13f);
            transform.Translate(Vector3.down * speed * Time.deltaTime * 5 );
        }
    }
}
