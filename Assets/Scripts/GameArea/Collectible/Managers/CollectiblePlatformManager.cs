using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class CollectiblePlatformManager : AbstractCollectibleManager<PlatformTypeChance.PlatformType>
{
    protected PlatformFactory _platformFactory;
    protected float _defaultDuration = 30;
          
    public CollectiblePlatformManager(PlatformFactory platformFactory)
    {
        _platformFactory = platformFactory;
    }

    protected void AddDefaultCollectible(PlatformTypeChance.PlatformType type, float toPositionY)
    {
        if (list == null)
            list = new List<CollectibleHolder<PlatformTypeChance.PlatformType>>();

        bool found = false;

        foreach (CollectibleHolder<PlatformTypeChance.PlatformType> ptc in list)
        {
            if (ptc.item == type)
            {

                if (ptc.toPositionY < toPositionY)
                    ptc.toPositionY = toPositionY;

                found = true;
                break;
            }
        }

        if (found)
            return;

        CollectibleHolder<PlatformTypeChance.PlatformType> newHolder = new CollectibleHolder<PlatformTypeChance.PlatformType>();

        for (int i = 0; i < _platformFactory.components.Length; i++)
        {
            if (_platformFactory.components[i].type == type)
            {
                _platformFactory.components[i].chance = 100;
                _platformFactory.components[i].minimumSameComponentGap = 0;
                break;
            }
        }

        newHolder.toPositionY = toPositionY;
        newHolder.item = type;
        list.Add(newHolder);
    }

    protected void AddResizeableCollectible(PlatformTypeChance.PlatformType type, float toPositionY)
    {
        if (list == null)
            list = new List<CollectibleHolder<PlatformTypeChance.PlatformType>>();

        bool found = false;

        foreach (CollectibleHolder<PlatformTypeChance.PlatformType> ptc in list)
        {
            if (ptc.item == type)
            {

                if (ptc.toPositionY < Camera.main.transform.position.y + _defaultDuration)
                    ptc.toPositionY = Camera.main.transform.position.y + _defaultDuration;

                if(_platformFactory.maxPlatformWidth != toPositionY || _platformFactory.minPlatformWidth != toPositionY)
                {
                    _platformFactory.maxPlatformWidth = toPositionY;
                    _platformFactory.minPlatformWidth = toPositionY;
                }

                found = true;
                break;
            }
        }

        if (found)
            return;

        CollectibleHolder<PlatformTypeChance.PlatformType> newHolder = new CollectibleHolder<PlatformTypeChance.PlatformType>();
        newHolder.item = type;

        _platformFactory.maxPlatformWidth = toPositionY;
        _platformFactory.minPlatformWidth = toPositionY;

        newHolder.toPositionY = Camera.main.transform.position.y + _defaultDuration;
        list.Add(newHolder);
    }

    public override void AddCollectible(PlatformTypeChance.PlatformType type, float toPositionY)
    {    
         switch(type)
        {
            default:
                Debug.Log(type + " to position " + toPositionY);
                AddDefaultCollectible(type, toPositionY);
                break;

            case PlatformTypeChance.PlatformType.Resizeable:
                AddResizeableCollectible(type, toPositionY);
                break;
        }
        
    }      

    public override void CheckCollectibles(float positionY)
    {
        if(list != null && list.Count > 0)
        {
            foreach (CollectibleHolder<PlatformTypeChance.PlatformType> holder in list)
            {
                Debug.Log("CEKUJU - "+holder.item + "stopY: "+holder.toPositionY);
                if (holder.toPositionY < positionY)
                {
                    Debug.Log("JDU ODJEBAT. - "+holder.item);
                    ResetItem(holder.item);
                }
            }
            list.RemoveAll(x => x.toPositionY < positionY);   
        }
    }

    public override void Clear()
    {
        if(list != null)
        { 
            foreach (CollectibleHolder<PlatformTypeChance.PlatformType> holder in list)
            {
                ResetItem(holder.item);
            }
        list.Clear();
        }
    }

    void ResetItem(PlatformTypeChance.PlatformType type)
    {
        if(type == PlatformTypeChance.PlatformType.Resizeable)
        {                               
            ResetResizableItem();
        }
        else
        {
            ResetDefaultItem(type);
        }
        
    }

    void ResetResizableItem()
    {
        Debug.Log("ODJEBAVAM KURVA!");
        _platformFactory.minPlatformWidth = _platformFactory.defaultMinPlatformWidth;
        _platformFactory.maxPlatformWidth = _platformFactory.defaultMaxPlatformWidth;
    }

    void ResetDefaultItem(PlatformTypeChance.PlatformType type)
    {
        for (int i = 0; i < _platformFactory.components.Length; i++)
        {
            if (_platformFactory.components[i].type == type)
            {
                _platformFactory.components[i].chance = _platformFactory.components[i].defualtChance;
                _platformFactory.components[i].minimumSameComponentGap = _platformFactory.components[i].defaultMinimumSameComponentGap;
                break;
            }
        }
    }
}