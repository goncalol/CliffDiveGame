using System;
using UnityEngine;

public class Mover : MonoBehaviour
{
    protected float speed;
    protected GameController gameController;
    Guid Id;
    protected bool Stopped;
    protected ParticleSystem ps;
    protected bool IsPartiallyDestroyed;
    Obstacle obstacle;

    // Start is called before the first frame update
    void Start()
    {
        Id = Guid.NewGuid();
        ps = gameObject.GetComponent<ParticleSystem>();
        gameController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
        speed = gameController.GetSpeed();
        obstacle = gameObject.GetComponent<Obstacle>();
        ForStarters();
    }

    protected virtual void ForStarters() { }

    public void ChangeSpeed(float newSpeed) => speed = newSpeed;

    public Guid GetId() => Id;

    void Update()
    {
        if (!Stopped)
            transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    public virtual void ReloadMovers()
    {
        if (!IsPartiallyDestroyed)
        {
            if (obstacle != null) obstacle.Hide();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            var mask = gameObject.GetComponent<SpriteMask>();
            if (mask != null) mask.enabled = false;
            ps.Play();
        }
    }

    public void PartialDestroyMover()
    {
        IsPartiallyDestroyed = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        var mask = gameObject.GetComponent<SpriteMask>();
        if (mask != null) mask.enabled = false;
        var psMain = ps.main;
        psMain.gravityModifierMultiplier *= -1;
        ps.Play();
        SoundController.Instance.rockBreakAudio.Play();
    }

    public virtual void StopMovement()
    {
        Stopped = true;
    }

    public virtual void StartMovement()
    {
        Stopped = false;
    }

    public virtual void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
