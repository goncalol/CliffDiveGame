using System.Collections;
using UnityEngine;

public class Warning : MonoBehaviour
{
    SpriteRenderer _renderer;
    bool _enabled;
   // AudioSource warningAudio;
    

    void Start()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
       // warningAudio = GameObject.Find("WarningAudio").GetComponent<AudioSource>();
        //warningAudio.Play();
        //StartCoroutine(PlayAudio(4));
        InvokeRepeating("Blink", 0, 0.1f);
    }

    void Blink()
    {
        _enabled = !_enabled;
        _renderer.enabled = _enabled;
    }

    //IEnumerator PlayAudio(int times)
    //{
    //    for (int i = 0; i < times; i++)
    //    {
    //        warningAudio.Play();
    //        yield return new WaitForSeconds(warningAudio.clip.length);
    //    }
    //}

}
