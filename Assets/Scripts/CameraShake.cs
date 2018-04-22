using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;

        float elapsedTime = 0.0f;

        while(elapsedTime < duration)
        {
            float x = RandomShake(magnitude);
            float y = RandomShake(magnitude);

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = Vector3.zero;
    }

    private float RandomShake(float magnitude)
    {
        return Random.Range(-1f, 1f) * magnitude;
    }
}
