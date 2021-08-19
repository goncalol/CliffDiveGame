using System;
using UnityEngine;

public class ShakeBehaviour : MonoBehaviour
{
    // Transform of the GameObject you want to shake
    private Transform _transform;

    // Desired duration of the shake effect
    private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.1f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 0.5f;

    // The initial position of the GameObject
    Vector3 initialPosition;

    bool IsBigShake;

    bool stopped;

    void Awake()
    {
        if (_transform == null)
        {
            _transform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        initialPosition = _transform.localPosition;
    }

    void Update()
    {
        if (!IsBigShake)
        {
            if (shakeDuration > 0)
            {
                _transform.localPosition = initialPosition + UnityEngine.Random.insideUnitSphere * shakeMagnitude;

                shakeDuration -= Time.deltaTime * dampingSpeed;
            }
            else
            {
                shakeDuration = 0f;
                _transform.localPosition = initialPosition;
            }
        }
        else
        {
            _transform.localPosition = initialPosition + UnityEngine.Random.insideUnitSphere * shakeMagnitude;
        }
    }

    public void TriggerShake()
    {
        if(!stopped && !IsBigShake) shakeDuration = 0.1f;
    }

    public void TriggerBigShake()
    {
        IsBigShake = true;
    }

    public void StopBigShake()
    {
        IsBigShake = false;
        _transform.localPosition = initialPosition;
    }

    internal void Stop()
    {
        stopped = true;
        shakeDuration = 0;
    }

    public void Restart()
    {
        stopped = false;
    }
}
