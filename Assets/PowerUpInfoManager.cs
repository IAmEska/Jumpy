using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpInfoManager : MonoBehaviour {

    
    public PowerUpInfoHolder holder;
    public float redrawTreshold = 0.5f;

    public float holderStartY = 15f;
    public float holderHeight = 30f;

    public float panelStartY = 0;
    public float panelHeight = 0f;


    protected List<PowerUpInfoHolder> _list;

    protected bool _redraw = false;

    protected RectTransform _panel;

    protected float _lastPositionY;

    public void AddCollectible(Collectible collectible, float toPositionY)
    {                                   
        bool addCollectible = true;
        foreach(PowerUpInfoHolder h in _list)
        {
            if(h.type == collectible.GetType())
            {
                if (toPositionY > h.toPositionY)
                {
                    h.toPositionY = toPositionY;
                    _redraw = true;
                }

                addCollectible = false;
                break;
            }
        }
        
        if(addCollectible)
        {
            PowerUpInfoHolder h = Instantiate(holder);
            h.toPositionY = toPositionY;
            h.SetImage(collectible.GetSprite());
            h.type = collectible.GetType();
            h.transform.SetParent(transform, false);
            _list.Add(h);
            _redraw = true;
        }
    }

	// Use this for initialization
	void Start ()
    {
        _list = new List<PowerUpInfoHolder>();
        _panel = GetComponent<RectTransform>();
        holderHeight = holder.GetComponent<RectTransform>().sizeDelta.y;
    }

    public void Clear()
    {
        _redraw = false;
        panelStartY = 0;
        panelHeight = 0;
        _list.Clear();
        foreach (Transform child in _panel.transform)
        {                                                    
            Destroy(child.gameObject);
        }
        _panel.sizeDelta = new Vector2(_panel.sizeDelta.x, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(_redraw)
        {
            float posY = holderStartY;

            float panelHeight = _list.Count * holderHeight;

            

            _panel.anchoredPosition = new Vector2(_panel.anchoredPosition.x, panelHeight / 2);
            _panel.sizeDelta = new Vector2(_panel.sizeDelta.x, panelHeight);  

            foreach (PowerUpInfoHolder h in _list)
            {
                h.SetPositionY(posY);
                h.SetText((h.toPositionY - Camera.main.transform.position.y).ToString("F0"));
                posY += holderHeight;
            }
            _redraw = false;
        }

        if(Mathf.Abs(_lastPositionY - Camera.main.transform.position.y) >= redrawTreshold)
        {
            _redraw = true;
        }

        // check PowerUps;

        var itemsToRemove = _list.FindAll(x => x.toPositionY - Camera.main.transform.position.y <= 0);
        foreach(PowerUpInfoHolder h in itemsToRemove)
        {   
            _list.Remove(h);                                   
            Destroy(h.gameObject);
        }
        if (itemsToRemove.Count > 0)
            _redraw = true;

    }
}
