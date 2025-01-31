using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class OfficeSitOnChair : MonoBehaviour
{
    public Transform mainCamera;
    public Transform mainCamera2;
    public TrackedPoseDriver trackedPoseDriver;
    public Transform chairPosition;
    public Transform returnPosition;
    public float sittingHeightOffset = 0.5f;
    private float sitDelay = 3f;
    private bool sitting = false;
    private AudioSource audioSource;
    private Vector3 startingPosition;

    public float smoothTime = 0.5f;    // Timpul pentru mișcarea smooth

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.spatialBlend = 1.0f;
        audioSource.minDistance = 0.4f;
        audioSource.maxDistance = 0.6f;
    }

    public void TriggerSitDown()
    {
        StartCoroutine(SitDownWithDelay());
    }

    private IEnumerator SitDownWithDelay()
    {
        Debug.Log($"Aștept {sitDelay} secunde înainte de așezare...");
        yield return new WaitForSeconds(sitDelay);

        Debug.Log("Player începe să se așeze...");
        yield return StartCoroutine(SmoothDampTransition(mainCamera, chairPosition.position));
        yield return StartCoroutine(SmoothDampTransition(mainCamera2, chairPosition.position));

        trackedPoseDriver.trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
        sitting = true;

        Debug.Log("Player s-a așezat automat pe scaun.");
    }

    private IEnumerator SmoothDampTransition(Transform target, Vector3 destination)
    {
        Vector3 velocity = Vector3.zero; // Viteza curentă a mișcăriie
        float elapsedTime = 0f;
        float duration = 2f; // Durata maximă a tranziției
        Vector3 currentPosition = target.position;

        while (elapsedTime < duration)
        {
            target.position = Vector3.SmoothDamp(currentPosition, destination, ref velocity, smoothTime);
            currentPosition = target.position;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asigură-te că poziția finală este exact cea dorită
        target.position = destination;
    }

    public void StandUp()
    {
        trackedPoseDriver.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;

        sitting = false;
        Debug.Log("Player s-a ridicat de pe scaun.");
    }
}
