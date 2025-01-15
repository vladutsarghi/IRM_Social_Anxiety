using UnityEngine;
using UnityEngine.InputSystem.XR;

public class OfficeSitOnChair : MonoBehaviour
{
    public Transform mainCamera;             // Public Camera reference
    public Transform mainCamera2;
    public TrackedPoseDriver trackedPoseDriver;
    public Transform chairPosition;          // Poziția de așezare pe scaun
    public Transform returnPosition;         // Poziția de revenire
    public float sittingHeightOffset = 0.5f; // Offset-ul înălțimii pentru așezare
                                             //public AudioClip sitSound;               // Sunet la așezare
                                             //public AudioClip standSound;             // Sunet la ridicare
    public Transform playerTarget;

    private bool sitting = false;
    private AudioSource audioSource;
    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = returnPosition.position;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurare audio 3D
        audioSource.spatialBlend = 1.0f; // 3D audio
        audioSource.minDistance = 0.4f;
        audioSource.maxDistance = 0.6f;    // Ajustează după necesități
    }

    void Update()
    {
        // Verifică dacă butonul de așezare este apăsat
        if (Input.GetKeyDown(KeyCode.Space)) // Poți schimba la un input specific XR
        {
                SitDown();
        }
    }

    private void SitDown()
    {
        // Mută XR Origin la poziția scaunului
        mainCamera.position = chairPosition.position;
        mainCamera2.position = chairPosition.position;

        trackedPoseDriver.trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
        sitting = true;

        Debug.Log("Player s-a așezat pe scaun.");

        //// Redă sunetul de așezare
        //if (sitSound != null)
        //{
        //    audioSource.PlayOneShot(sitSound);
        //}
    }

    //private void StandUp()
    //{
    //    // Revine la poziția inițială
    //    xrOrigin.position = returnPosition.position;

    //    // Resetează poziția camerei
    //    cameraTransform.localPosition = Vector3.zero;

    //    sitting = false;
    //    Debug.Log("Player s-a ridicat de pe scaun.");

    //    //// Redă sunetul de ridicare
    //    //if (standSound != null)
    //    //{
    //    //    audioSource.PlayOneShot(standSound);
    //    //}
    //}
}
