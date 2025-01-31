using UnityEngine;
using System.Collections.Generic;
using Unity.XR.CoreUtils;

public class NpcController : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private bool hasTriggered = false;

    public AudioClip welcomeClip;
    public AudioClip badWords;
    public AudioClip lessKeywords;
    public AudioClip timeExpired;
    public AudioClip congratulations;
    public List<AudioClip> questionClips;
    public OfficeVoiceRecognition voiceRecognition;
    public OfficeSitOnChair sitController;
    public GameObject player;
    public Transform respawnPoint;

    private int currentQuestionIndex = 0;
    private bool waitingForResponse = false;
    private bool responseSubmitted = false;

    private Dictionary<int, List<string>> questionKeywords = new Dictionary<int, List<string>>
    {
        {
            0, new List<string>
            {
                "background", "experience", "education", "skills", "career", "journey",
                "qualifications", "expertise", "achievements", "strengths", "weaknesses",
                "goals", "motivation", "passion", "profile", "certifications", "knowledge",
                "training", "growth", "character"
            }
        },
        {
            1, new List<string>
            {
                "problem", "adaptability", "leadership", "detail", "teamwork", "creativity",
                "resilience", "time", "communication", "thinking", "solution", "flexibility",
                "analysis", "coordination", "prioritization", "focus", "planning", "organization",
                "decisiveness", "negotiation"
            }
        },
        {
            2, new List<string>
            {
                "listening", "mediation", "professionalism", "negotiation", "compromise", "de-escalation",
                "patience", "collaboration", "understanding", "empathy", "conflict", "resolution",
                "respect", "tolerance", "calm", "fairness", "approach", "assertiveness", "dialogue",
                "acknowledgment"
            }
        },
        {
            3, new List<string>
            {
                "values", "mission", "role", "growth", "culture", "vision", "industry", "leader",
                "innovation", "opportunity", "development", "team", "strategy", "goals", "purpose",
                "market", "reputation", "inclusion", "ethics", "branding"
            }
        },
        {
            4, new List<string>
            {
                "advancement", "skills", "leadership", "long-term", "growth", "learning", "professional",
                "position", "mentorship", "expertise", "progress", "promotion", "upskilling", "coaching",
                "specialization", "evolution", "goals", "aspiration", "opportunity", "innovation"
            }
        },
    };

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (respawnPoint == null)
        {
            Debug.LogError("No respawn point");
        }
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (voiceRecognition == null)
        {
            Debug.LogError("VoiceRecognition is not assigned to NPC!");
        }

        if (questionClips.Count < 5)
        {
            Debug.LogError("Not enough audio clips assigned for questions!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && !hasTriggered)
        {
            hasTriggered = true;
            animator.SetTrigger("needToShake");

            if (sitController != null)
            {
                sitController.TriggerSitDown();
                Debug.Log("SitController activated for NPC!");
            }
            else
            {
                Debug.LogWarning("SitController is not set on NPC!");
            }

            Invoke(nameof(PlayWelcomeAudio), 3.0f);
        }
    }

    private void PlayWelcomeAudio()
    {
        if (welcomeClip != null)
        {
            audioSource.PlayOneShot(welcomeClip);
            Debug.Log("NPC: Welcome to the interview!");
            Invoke(nameof(AskQuestion), welcomeClip.length + 6.0f);
        }
        else
        {
            Debug.LogWarning("No welcome audio assigned!");
        }
    }

    private void AskQuestion()
    {
        if (currentQuestionIndex < questionClips.Count)
        {
            audioSource.PlayOneShot(questionClips[currentQuestionIndex]);
            Debug.Log("NPC: Asking question...");

            float waitTime = questionClips[currentQuestionIndex].length;
            Invoke(nameof(StartRecording), waitTime);
        }
        else
        {
            audioSource.PlayOneShot(congratulations);
            Invoke(nameof(FinishLevel), congratulations.length);
        }
    }

    private void StartRecording()
    {
        Debug.Log("Start recording");
        voiceRecognition.StartDictation();
        waitingForResponse = true;
        responseSubmitted = false;

        Invoke(nameof(ResponseTimedOut), 120f);
    }

    private void ResponseTimedOut()
    {
        if (!responseSubmitted)
        {
            audioSource.PlayOneShot(timeExpired);
            RestartLevel();
        }
    }

    void Update()
    {
        if (waitingForResponse && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Player pressed Space. Processing response...");
            voiceRecognition.StopDictation();
            ProcessResponse(voiceRecognition.GetRecognizedText());
        }
    }

    public void ProcessResponse(string response)
    {
        if (!waitingForResponse) return;

        waitingForResponse = false;
        responseSubmitted = true;
        CancelInvoke(nameof(ResponseTimedOut));

        Debug.Log("Player response: " + response);

        if (string.IsNullOrEmpty(response))
        {
            Debug.Log("Response is null or empty! Restarting level...");
            audioSource.PlayOneShot(lessKeywords);
            Invoke(nameof(RestartLevel), lessKeywords.length);
            return;
        }

        if (ContainsBadWords(response))
        {
            audioSource.PlayOneShot(badWords);
            Invoke(nameof(RestartLevel), badWords.length);
            return;
        }

        if (!ContainsRequiredKeywords(response))
        {
            audioSource.PlayOneShot(lessKeywords);
            Invoke(nameof(RestartLevel), lessKeywords.length);
            return;
        }

        Debug.Log("NPC: Your response has been recorded.");
        currentQuestionIndex++;

        if (currentQuestionIndex < questionClips.Count)
        {
            Invoke(nameof(AskQuestion), 2.0f);
        }
        else
        {
            audioSource.PlayOneShot(congratulations);
            Invoke(nameof(FinishLevel), congratulations.length);
        }
    }

    private bool ContainsRequiredKeywords(string response)
    {
        Debug.Log("keyword");
        if(response == null)
            return false;
        if (!questionKeywords.ContainsKey(currentQuestionIndex)) return false;

        int matchCount = 0;
        string lowerResponse = response.ToLower();

        foreach (string keyword in questionKeywords[currentQuestionIndex])
        {
            if (lowerResponse.Contains(keyword))
            {
                matchCount++;
                if (matchCount >= 2) return true;
            }
        }
        return false;
    }

    private bool ContainsBadWords(string text)
    {
        Debug.Log("bad words");
        List<string> badWords = new List<string>
        {
            "fuck", "shit", "bitch", "asshole", "bastard", "damn", "crap", "dumbass", "motherfucker",
            "fucker", "dick", "piss", "wanker", "cock", "twat", "bollocks", "prick", "cunt"
        };

        foreach (string word in badWords)
        {
            if (text.ToLower().Contains(word))
            {
                return true;
            }
        }
        return false;
    }

    private void RespawnPlayer()
    {

        if (respawnPoint != null && player != null)
        {
            Debug.Log("Respawn");
            CharacterController characterController = player.GetComponent<CharacterController>();

            if (characterController != null)
            {
                characterController.enabled = false;
            }

            player.transform.position = new Vector3(45f, 2.8f, 10f);
            player.transform.rotation = Quaternion.Euler(0, 180, 0);

            Debug.Log(player.transform.position);
            Debug.Log(respawnPoint.position);

            if (characterController != null)
            {
                characterController.enabled = true;
            }

            Debug.Log("Player respawned at new position.");
        }
        else
            Debug.Log("palyer null");
    }


    private void RestartLevel()
    {
        Debug.Log("Restarting level...");

        hasTriggered = false;
        currentQuestionIndex = 0;
        waitingForResponse = false;
        responseSubmitted = false;

        CancelInvoke(nameof(ResponseTimedOut));

        if (sitController != null)
        {
            sitController.StandUp();
        }
        //MoveXRPlayer(new Vector3(0, 1, 0));
        RespawnPlayer();
    }

    private void FinishLevel()
    {
        Debug.Log("Finishing level... Resetting NPC and player.");

        hasTriggered = false;
        currentQuestionIndex = 0;
        waitingForResponse = false;
        responseSubmitted = false;

        CancelInvoke(nameof(ResponseTimedOut));

        if (sitController != null)
        {
            sitController.StandUp();
        }
        RespawnPlayer();
    }
}
