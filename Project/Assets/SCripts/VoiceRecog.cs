using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Text;
using UnityEngine.UI;
using TMPro;

public class VoiceRecognition : MonoBehaviour
{
    private DictationRecognizer dictationRecognizer;
    private StringBuilder recognizedText;
    private float startTime;
    private int wordCount = 0;
    public TMP_Text wpmText;
    private bool isRecording = false; 

    void Start()
    {
        recognizedText = new StringBuilder();
        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationResult += (text, confidence) =>
        {
            recognizedText.Append(text + " ");
            wordCount += CountWords(text);
        };

        dictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogError("Eroare la recunoastere: " + error);
        };
    }

    public void StartDictation()
    {
        if (!isRecording)
        {
            dictationRecognizer.Start();
            startTime = Time.time;
            isRecording = true;
            Debug.Log("Recunoasterea vocala a pornit.");
        }
    }

    void Update()
    {
        if (!isRecording) return;

        float timePassed = Time.time - startTime;
        float wpm = (wordCount / timePassed) * 60;

        wpmText.text = "WPM: " + Mathf.RoundToInt(wpm).ToString();

        if (wpm < 125)
        {
            wpmText.text += "\nVorbesti prea incet!";
        }
        else if (wpm > 160)
        {
            wpmText.text += "\nVorbesti prea repede!";
        }
    }

    int CountWords(string sentence)
    {
        string[] words = sentence.Split(' ');
        return words.Length;
    }

    void OnDestroy()
    {
        if (dictationRecognizer != null && dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            dictationRecognizer.Stop();
            dictationRecognizer.Dispose();
        }
    }
}
