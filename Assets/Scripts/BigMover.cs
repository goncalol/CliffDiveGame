using UnityEngine;

public class BigMover : Mover
{
    public GameObject wall;
    public GameObject obstacleBase;
    public BoxCollider2D[] colliders;

    public override void ReloadMovers()
    {
        for(int i=0; i < gameObject.transform.childCount; i++)
        {
            var child = gameObject.transform.GetChild(i).gameObject;

            var renderer = child.GetComponent<SpriteRenderer>();
            if(renderer!=null) renderer.enabled = false;

            var mask = child.GetComponent<SpriteMask>();
            if (mask != null) mask.enabled = false;
        }
               
        ps.Play();
    }

    public void PartialDestroyBigMover(Player player)
    {
        IsPartiallyDestroyed = true;

        foreach(var col in colliders)
        {
            col.enabled = false;
        }
        player.IsOnCollision = false;

        wall.GetComponent<SpriteRenderer>().enabled = false;
        var mask = wall.GetComponent<SpriteMask>();
        if (mask != null) mask.enabled = false;


        obstacleBase.GetComponent<SpriteRenderer>().enabled = false;
        var mask2 = obstacleBase.GetComponent<SpriteMask>();
        if (mask2 != null) mask2.enabled = false;

        var psMain = ps.main;
        psMain.gravityModifierMultiplier *= -1;
        var ps2 = obstacleBase.GetComponent<ParticleSystem>();
        var psMain2 = ps2.main;
        psMain2.gravityModifierMultiplier *= -1;

        ps.Play();
        SoundController.Instance.rockBreakAudio.Play();

    }
}
