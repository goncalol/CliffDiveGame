using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitUI : MonoBehaviour, IPointerDownHandler
{
    private GameObject group;

    void Start()
    {
        group = gameObject.transform.parent.gameObject;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        group.SetActive(false);
    }

    protected virtual void PointerExtend()
    {

    }
}
