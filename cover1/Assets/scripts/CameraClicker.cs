using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClicker : MonoBehaviour
{
    public Camera myCamera;
    public float spherecastRadius = 1f;

    void LateUpdate()
    {
        Vector2 mousePosition = Input.mousePosition;
        Ray worldRay = myCamera.ScreenPointToRay(mousePosition);

        //if (Physics.SphereCast(worldRay, spherecastRadius, out RaycastHit hitInfo))
        //{
          //  ClickableObject clickable = hitInfo.transform.GetComponent<ClickableObject>();
         //   if (clickable != null)
          //  {
           //     clickable.Hover();
           // }
      //  }

        RaycastHit[] hits = Physics.SphereCastAll(worldRay, spherecastRadius);
        foreach (RaycastHit hit in hits)
        {
            ClickableObject clickable = hit.transform.GetComponent<ClickableObject>();
            if (clickable != null)
            {
                clickable.Hover();
            }
        }

        Debug.DrawRay(worldRay.origin, worldRay.direction);
    }
}
