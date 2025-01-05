using UnityEngine;
using System.Collections;

public class ContinuousMicrophone : MonoBehaviour
{
    private AudioClip microphoneClip; 
    private bool isRecording = false; 
    private const int sampleRate = 44100; 

    void Start()
    {
        StartMicrophone();
    }

    void StartMicrophone()
    {
        if (Microphone.devices.Length > 0)
        {
            string micName = Microphone.devices[0]; 
            microphoneClip = Microphone.Start(micName, true, 10, sampleRate); 
            isRecording = true;
            Debug.Log("Microphone started: " + micName);
        }
        else
        {
            Debug.LogError("No microphone detected!");
        }
    }

    void StopMicrophone()
    {
        if (isRecording)
        {
            Microphone.End(null);
            isRecording = false;
            Debug.Log("Microphone stopped.");
        }
    }

    void OnApplicationQuit()
    {
        StopMicrophone();
    }

    public float[] GetMicrophoneData()
    {
        if (microphoneClip == null)
            return null;

        float[] samples = new float[microphoneClip.samples * microphoneClip.channels];
        microphoneClip.GetData(samples, 0); 
        return samples;
    }
}
