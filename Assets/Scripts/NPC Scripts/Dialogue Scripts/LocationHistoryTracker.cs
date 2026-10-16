using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationHistoryTracker : MonoBehaviour
{
   
    private readonly HashSet<LocationSO> locationVisited = new HashSet<LocationSO>();

   

    public void RecordLocation(LocationSO locationSO){
       if(locationVisited.Add(locationSO)){
        Debug.Log("Just spoke to " + locationSO.displayName);
       }
    }

    public bool HasVisited(LocationSO locationSO){
        return locationVisited.Contains(locationSO);
    }
}
