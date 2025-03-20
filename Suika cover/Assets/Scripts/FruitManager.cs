using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    public FruitSets fruitSets; // Reference to the Fruit List

    public void HandleFruitCollision(Fruit fruit1, Fruit fruit2, Vector2 collisionPosition)
    {
        if (fruit1.value == fruit2.value && fruit1.value < 11)
        {
            int nextValue = fruit1.value + 1;

            // Get the prefab for the next level fruit
            GameObject nextLevelFruitPrefab = fruitSets.GetFruitPrefabByValue(nextValue);

            if (nextLevelFruitPrefab != null)
            {
                // Spawn the new merged fruit at the collision position
                GameObject newFruit = Instantiate(nextLevelFruitPrefab, collisionPosition, Quaternion.identity);

                // Ensure the new fruit is ready to merge again
                Fruit newFruitScript = newFruit.GetComponent<Fruit>();
                if (newFruitScript != null)
                {
                    newFruitScript.ResetMergingState();
                }
            }

            // Delay destroy to ensure smooth merging physics updates
            StartCoroutine(DestroyFruitsWithDelay(fruit1.gameObject, fruit2.gameObject));
        }
    }

    private IEnumerator DestroyFruitsWithDelay(GameObject fruit1, GameObject fruit2)
    {
        yield return new WaitForSeconds(0.1f); // Small delay to allow proper physics updates
        Destroy(fruit1);
        Destroy(fruit2);
    }
}
