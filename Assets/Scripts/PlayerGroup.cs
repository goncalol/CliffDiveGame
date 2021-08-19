using System;
using UnityEngine;

public class PlayerGroup : MonoBehaviour
{
    float speed;
    bool IsMoveLeft;
    bool IsMoveRight;
    Vector2 initialPosition;
    Rigidbody2D rigid;
    bool stopLeftTranslation;
    bool stopRightTranslation;

    private void Start()
    {
        initialPosition = gameObject.transform.position;
    }

    public void MoveRight()
    {
        IsMoveLeft = false;
        IsMoveRight = true;
    }

    public void MoveLeft()
    {
        IsMoveLeft = true;
        IsMoveRight = false;
    }


    public bool IsMovingLeft() => IsMoveLeft;

    public bool IsMovingRight() => IsMoveRight;

    public bool IsMovingSide() => IsMoveLeft || IsMoveRight;

    void Update()
    {        
        if (!stopLeftTranslation && IsMoveLeft)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (!stopRightTranslation && IsMoveRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        //safecheck
        if (transform.localPosition.x < -2f) MoveRight();
        if (transform.localPosition.x > 2.2f) MoveLeft();
    }


    internal void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    internal void StopLeftTranslation()
    {
        stopLeftTranslation = true;
    }

    internal void ContinueLeftTranslation()
    {
        stopLeftTranslation = false;
    }

    internal void StopRightTranslation()
    {
        stopRightTranslation = true;
    }

    internal void ContinueRightTranslation()
    {
        stopRightTranslation = false;
    }

    internal void StopLeftMove()
    {
        StopLeftTranslation();
        IsMoveLeft = false;
    }

    internal void StopRightMove()
    {
        StopRightTranslation();
        IsMoveRight = false;
    }

    internal void HidePosition()
    {
        IsMoveLeft = false;
        IsMoveRight = false;
        gameObject.transform.position = initialPosition;
    }

    internal void ChangeMoveDirection(bool pushPlayerRight)
    {
        if(!IsMoveLeft && !IsMoveRight)
        {
            IsMoveLeft = pushPlayerRight;
            IsMoveRight = !pushPlayerRight;
        }

        IsMoveLeft = !IsMoveLeft;
        IsMoveRight = !IsMoveRight;
    }
}
