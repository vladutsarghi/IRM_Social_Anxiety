using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Text;
using TMPro;

public class OfficeVoiceRecognition : MonoBehaviour
{
    private DictationRecognizer dictationRecognizer;
    private StringBuilder recognizedText;
    private bool isRecording = false;
    public TMP_Text wpmText;
    public NpcController npcController; // Referință către NPC

    void Start()
    {
        
            recognizedText = new StringBuilder();
            dictationRecognizer = new DictationRecognizer();

            dictationRecognizer.InitialSilenceTimeoutSeconds = 200f; 
            dictationRecognizer.AutoSilenceTimeoutSeconds = 200f;    

            dictationRecognizer.DictationResult += OnDictationResult;
            dictationRecognizer.DictationError += OnDictationError;
    }

    public void StartDictation()
    {
        if (!isRecording)
        {
            dictationRecognizer.Start();
            isRecording = true;
            recognizedText.Clear();
            Debug.Log("Voice recognition started.");
        }
    }

    public void StopDictation()
    {
        if (isRecording && dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            dictationRecognizer.Stop();
            isRecording = false;
            Debug.Log("Voice recognition stopped.");
        }
    }

    private void OnDictationResult(string text, ConfidenceLevel confidence)
    {
        recognizedText.Append(text + " ");
    }

    private void OnDictationError(string error, int hresult)
    {
        Debug.LogError("Voice recognition error: " + error);
    }

    public string GetRecognizedText()
    {
        return recognizedText.ToString().Trim(); // Returnează textul recunoscut
    }

    void OnDestroy()
    {
        if (dictationRecognizer != null)
        {
            dictationRecognizer.DictationResult -= OnDictationResult;
            dictationRecognizer.DictationError -= OnDictationError;

            if (dictationRecognizer.Status == SpeechSystemStatus.Running)
            {
                dictationRecognizer.Stop();
            }
            dictationRecognizer.Dispose();
        }
    }
}
