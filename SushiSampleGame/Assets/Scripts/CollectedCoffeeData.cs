using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedCoffeeData : MonoBehaviour
{
    public static CollectedCoffeeData instance;
    public List<Transform> CoffeeList;
    public List<Transform> HandHoldTransform;
    public List<Transform> HandParent;

    public int finishCount = 0;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Update() {
        if (finishCount == 10)
        {
            finishCount = 6;
        }
    }
}
