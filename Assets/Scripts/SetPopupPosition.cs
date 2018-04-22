using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPopupPosition : MonoBehaviour {

    Vector3 location;

    public void Set(Vector3 position)
    {
        location = position;
    }

    private void Update()
    {
        Vector2 viewportPosition = Camera.main.WorldToScreenPoint(location);
        transform.position = viewportPosition;

        if (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            Destroy(gameObject);
        }
    }
}
