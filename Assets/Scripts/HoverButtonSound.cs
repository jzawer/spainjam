using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButtonSound : MonoBehaviour, IPointerEnterHandler
{
    MusicManager musicManager;
    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        musicManager.Play(SoundNames.StartMenu_Hover);
        Debug.Log("Mouse Entered");
    }
}
