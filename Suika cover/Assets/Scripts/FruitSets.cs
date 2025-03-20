using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSets : MonoBehaviour

{
    public GameObject[] fruitPrefabs;

    public GameObject GetFruitPrefabByValue(int value)
    {
        if (value >= 1 && value <= fruitPrefabs.Length)
        {
            return fruitPrefabs[value - 1]; // Array index starts at 0, value starts at 1
        }
        return null;
    }

}