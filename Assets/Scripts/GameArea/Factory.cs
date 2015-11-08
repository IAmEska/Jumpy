﻿using UnityEngine;
using System.Collections;

public abstract class Factory : MonoBehaviour  {

    public enum FactoryState
    {
        IDLE,
        GENERATE,
        CLEAR
    }

    protected float _minX, _maxX;
    protected FactoryState _state;

    public virtual void SetBounds(float minX, float maxX)
    {
        _minX = minX;
        _maxX = maxX;
    }

    public abstract System.Type ItemType();

    public abstract void InstantiateItem(float positionY);

    public abstract void DestoryItem(Component item);

    public void SetState(FactoryState state)
    {
        _state = state;
    }

    protected abstract void GenerateBehaviour();

    protected abstract void ResetBehaviour();

    protected virtual void ClearBehaviour()
    {
        Debug.Log(name + " clear");
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var t = transform.GetChild(i);
                if (t.gameObject.layer == gameObject.layer)
                {
                    Component rt = t.GetComponent(ItemType());
                    DestoryItem(rt);
                }
            }
        }
    } 
    
    void FixedUpdate()
    {
        switch(_state)
        {
            case FactoryState.GENERATE:   
                GenerateBehaviour();
                _state = FactoryState.IDLE; 
                break;

            case FactoryState.CLEAR:            
                ClearBehaviour();
                ResetBehaviour();
                _state = FactoryState.IDLE;  
                break;        
        }                   
    }   
}