using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDrag : MonoBehaviour
{
    public void OnMouseDrag()
    {
        Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(vector3.x, vector3.y, 0);
    }
}
