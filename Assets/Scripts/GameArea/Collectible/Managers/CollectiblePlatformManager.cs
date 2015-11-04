using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class CollectiblePlatformManager : AbstractCollectibleManager<PlatformTypeChance.PlatformType>
{
    protected PlatformFactory platformFactory;
          
    public CollectiblePlatformManager(PlatformFactory platformFactory)
    {
        this.platformFactory = platformFactory;
    }

    public override void AddCollectible(PlatformTypeChance.PlatformType type, int toPositionY)
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
                Debug.Log("was found madafaka");
                break;
            }
        }

        if (found)
            return;

        CollectibleHolder<PlatformTypeChance.PlatformType> newHolder = new CollectibleHolder<PlatformTypeChance.PlatformType>();
        
        for(int i=0; i< platformFactory.components.Length; i++)
        {
            if (platformFactory.components[i].type == type)
            {
                platformFactory.components[i].chance = 100;
                platformFactory.components[i].minimumSameComponentGap = 0;
                break;
            }
        }

        Debug.Log("add type - " + type);                        
        newHolder.toPositionY = toPositionY;  
        list.Add(newHolder);
    }      

    public override void CheckCollectibles(float positionY)
    {
        if(list != null && list.Count > 0)
        {
            foreach (CollectibleHolder<PlatformTypeChance.PlatformType> holder in list)
            {
                if(holder.toPositionY < positionY)
                {
                    Debug.Log("remove madafaka");
                    ResetItem(holder.item);
                }
            }
            list.RemoveAll(x => x.fromPositionY < positionY);
            Debug.Log("Remove all - " + list.Count);
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
        for (int i = 0; i < platformFactory.components.Length; i++)
        {
            if (platformFactory.components[i].type == type)
            {
                platformFactory.components[i].chance = platformFactory.components[i].defualtChance;
                platformFactory.components[i].minimumSameComponentGap = platformFactory.components[i].defaultMinimumSameComponentGap;
                break;
            }
        }
    }
}