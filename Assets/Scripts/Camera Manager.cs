using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Image flashImage;

    [SerializeField] float shakeMagnitude;
    [SerializeField] float shakeDuration;
    [SerializeField] float flashDuration;


    public void CameraShake()
    {
        StartCoroutine(ShakeCoroutine());
        StartCoroutine(FlashCoroutine());
    }




    public IEnumerator FlashCoroutine()
    {
        flashImage.enabled = true;
        flashImage.color = new Color(1, 1, 1, 1);

        float timePassed = 0f;

        while (timePassed < flashDuration)
        {
            flashImage.color = new Color(1, 1, 1, 1 - (timePassed / flashDuration));

            timePassed += Time.deltaTime;
            yield return null; // Poczekaj do nastêpnej klatki
        }

        flashImage.enabled = false;
    }




    public IEnumerator ShakeCoroutine()
    {

        Vector3 originalPosition = transform.localPosition;

        float timePassed = 0f;
        float tempMagnitude = shakeMagnitude;

        while (timePassed < shakeDuration)
        {
            float shakeX = Random.Range(-1f, 1f) * tempMagnitude;
            float shakeY = Random.Range(-1f, 1f) * tempMagnitude;

            transform.localPosition = new Vector3(originalPosition.x + shakeX, originalPosition.y + shakeY, originalPosition.z);


            tempMagnitude = Mathf.Lerp(shakeMagnitude, 0, timePassed / shakeDuration); 
            timePassed += Time.deltaTime;
            yield return null; // Poczekaj do nastêpnej klatki
        }

        transform.localPosition = originalPosition;
    }
}
