using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PowerUpInfoHolder : MonoBehaviour {

    public Image image;
    public Text text;
    
    public float toPositionY;
    public Type type;

    protected RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }      
    
    public void SetPositionY(float positionY)
    {
        rectTransform.anchoredPosition = new Vector2(0, positionY);     
    }               
}
