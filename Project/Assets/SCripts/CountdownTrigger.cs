using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownTrigger : MonoBehaviour
{
    public TMP_Text countdownText;
    public GameObject speechPanel;
    private bool hasTriggered = false;
    public VoiceRecognition voiceRecognition; 

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("MainCamera"))
        {
            hasTriggered = true;
            StartCoroutine(StartCountdown());
        }
    }

    private IEnumerator StartCountdown()
    {
        speechPanel.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "Start Speech!";
        yield return new WaitForSeconds(1f);

        speechPanel.SetActive(false);

        if (voiceRecognition != null)
        {
            voiceRecognition.StartDictation(); 
        }
        else
        {
            Debug.LogError("VoiceRecognition script is not assigned!");
        }
    }
}
