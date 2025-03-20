using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    private float screenWidth, screenHeight;
    private bool hasEnteredScreen = false;

    public void Initialize(float width, float height)
    {
        screenWidth = width;
        screenHeight = height;
    }

    void Update()
    {
        Vector3 position = transform.position;

        // Check if bullet is within screen bounds
        bool insideScreen = position.x > -screenWidth && position.x < screenWidth &&
                            position.y > -screenHeight && position.y < screenHeight;

        if (insideScreen)
        {
            hasEnteredScreen = true; // Bullet is inside the screen at least once
        }
        else if (hasEnteredScreen)
        {
            Destroy(gameObject); // Only destroy if it has been inside screen once
        }
    }
}
