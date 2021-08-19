using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public AudioSource hitAudio;
    public GameObject LiquidLife;
    public GameObject splashAnim;
    public GameObject SplashLeftSidePrefab;
    public GameObject SplashRightSidePrefab;
    public TextMeshProUGUI LifeText;
    public ParticleSystem SmokeParticles;
    public GameObject Swirl;
    public GameObject Barrier;

    [HideInInspector]
    public bool IsOnCollision;
    
    GameController gameController;
    CircleCollider2D _collider;
    PlayerGroup group;
    Animator ParentAnimator;
    Animator Animator;
    Animator SplashAnimator;
    Rigidbody2D rigid;
    SpriteRenderer playerSprite;
    ParticleSystem WindParticles;
    bool CanMoveLeft = true;
    bool CanMoveRight = true;
    bool IsDead;
    int MaxLife = 50;
    float HeightDecreaseStep;
    int Life ;
    bool stopRotate;
    bool isLifeIncreased;

    private void Start()
    {
        LifeText.text = MaxLife.ToString();
        gameController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
        _collider = gameObject.GetComponent<CircleCollider2D>();
        group = transform.parent.GetComponentInParent<PlayerGroup>();
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
        group.SetSpeed(speed);
        ParentAnimator = gameObject.transform.parent.GetComponent<Animator>();
        SplashAnimator = splashAnim.GetComponent<Animator>();
        WindParticles = gameObject.transform.parent.GetComponent<ParticleSystem>();
        Life = MaxLife;
        HeightDecreaseStep = 97.3f / (MaxLife/10);
        Animator = gameObject.GetComponent<Animator>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        if (PlayerPrefs.GetInt("Heart100", 0) == 1)
            IncreaseLife(100);
        if (PlayerPrefs.GetInt("Heart50", 0) == 1)
            IncreaseLife(50);
    }

    internal void StopMovingRight()
    {
        group.StopRightTranslation();
        CanMoveRight = false;
    }

    internal void AllowMovingLeft()
    {
        group.ContinueLeftTranslation();
        CanMoveLeft = true;
    }

    internal void AllowMovingRight()
    {
        group.ContinueRightTranslation();
        CanMoveRight = true;
    }

    internal void StopMovingLeft()
    {
        group.StopLeftTranslation();
        CanMoveLeft = false;
    }

    public void MoveRight()
    {
        Swirl.SetActive(!stopRotate);
        if (CanMoveRight)
        {
            group.MoveRight();
        }
    }

    public void MoveLeft()
    {
        Swirl.SetActive(!stopRotate);
        if (CanMoveLeft)
        {
            group.MoveLeft();
        }
    }

    void Update()
    {
        if (!stopRotate)
        {
            if (IsMovingLeft())
            {
                rigid.MoveRotation(rigid.rotation + 1500 * Time.deltaTime);
            }
            else if (IsMovingRight())
            {
                rigid.MoveRotation(rigid.rotation - 1500 * Time.deltaTime);
            }
        }
    }

    internal bool IsMovingLeft() => group.IsMovingLeft();

    internal bool IsMovingRight() => group.IsMovingRight();
    
    internal void EnteredSideCollision(bool pushPlayerRight)
    {
        if (IsDead) return;

        if (!gameController.isSuperSpeed && !gameController.isBarrier)
        {
            hitAudio.Play();

            DecreaseLife();
            if (Life <= 0)
            {
                gameController.GameOver();
                return;
            }

            Animator.SetTrigger("Hit");

            if (IsMovingLeft())
            {
                var i = Instantiate(SplashLeftSidePrefab, transform.position, Quaternion.identity, gameController.InstanciationPlace);
                gameController.AddMover(i.GetComponent<Mover>());
            }
            else if (IsMovingRight())
            {
                var i = Instantiate(SplashRightSidePrefab, transform.position, Quaternion.identity, gameController.InstanciationPlace);
                gameController.AddMover(i.GetComponent<Mover>());
            }
        }

        if (gameController.isBarrier) SoundController.Instance.barrierHitAudio.Play();
        group.ChangeMoveDirection(pushPlayerRight);
    }

    internal void EnteredCollision()
    {
        if (IsDead) return;

        if (!gameController.isSuperSpeed)
        {
            hitAudio.Play();

            DecreaseLife();
            if (Life <= 0)
            {
                gameController.GameOver();
                return;
            }

            Animator.SetTrigger("Hit");
        }
    }

    public void HidePosition()
    {
        gameObject.transform.rotation = Quaternion.identity;
        _collider.enabled = false;
        group.HidePosition();
        gameController.SetGameOverDisplay();
    }

    public void ResetPosition()
    {
        if (!isLifeIncreased)
        {
            LiquidLife.transform.localPosition = new Vector3(LiquidLife.transform.localPosition.x, -2.7f);
            LifeText.text = MaxLife.ToString();
        }
        IsDead = false;
        playerSprite.enabled = true;
        IsOnCollision = false;
        WindParticles.Play();
        SmokeParticles.Stop();
        AllowMovingLeft();
        AllowMovingRight();
        gameObject.transform.parent.localPosition = Vector2.zero;
        ParentAnimator.SetTrigger("Fall");
    }

    public void Die()
    {
        Swirl.SetActive(false);
        IsDead = true;
        WindParticles.Stop();
        _collider.enabled = false;
        splashAnim.transform.parent.position = gameObject.transform.position;
        SplashAnimator.SetTrigger("Splash");
        ParentAnimator.SetTrigger("Squish");
        group.StopRightMove();
        group.StopLeftMove();
        PlayerPrefs.SetInt("Heart100", 0);
        PlayerPrefs.SetInt("Heart50", 0);
        HeightDecreaseStep = 97.3f / (MaxLife / 10);
        Life = MaxLife;
        isLifeIncreased = false;
    }

    public void CleanStains()
    {
        splashAnim.GetComponent<SplashAnim>().HideStain();
    }

    public void SoundOn()
    {
        hitAudio.mute = false;
    }

    public void SoundOff()
    {
        hitAudio.mute = true;
    }

    public void EnableCollisions()
    {
        _collider.enabled = true;
    }

    public void DecreaseLife()
    {
        Life -= 10;
        LifeText.text = Life.ToString();
        LiquidLife.transform.localPosition -= new Vector3(0, HeightDecreaseStep);
    }

    public void IncreaseLife(int quantity)
    {
        isLifeIncreased = true;
        Life += quantity;
        LifeText.text = Life.ToString();
        PlayerPrefs.SetInt(string.Format("Heart{0}",quantity), 1);
        HeightDecreaseStep = 97.3f / (Life / 10);
        LiquidLife.transform.localPosition = new Vector3(LiquidLife.transform.localPosition.x, -2.7f);
    }

    public bool IsSuperSpeed()
    {
        return gameController.isSuperSpeed;
    }

    public void SetRagedAnim(bool isRaged)
    {
        Swirl.SetActive(!isRaged && group.IsMovingSide());
        if (isRaged)
        {
            WindParticles.Stop();
            SmokeParticles.Play();
            gameController.BigShake(true);
            gameObject.transform.rotation = Quaternion.identity;
        }
        else
        {
            WindParticles.Play();
            SmokeParticles.Stop();
            gameController.BigShake(false);
        }
        stopRotate = isRaged;
        Animator.SetBool("IsRaged", isRaged);
    }

    public void SetBarrier(bool activate)
    {
        Barrier.SetActive(activate);
    }
}
