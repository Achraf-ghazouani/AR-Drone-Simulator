using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex;
    }

    public List<Question> questions;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public Button nextButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public Image progressBar;
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    
    private int currentQuestionIndex = 0;
    private int selectedAnswerIndex = -1;
    private bool answerSelected = false;
    private int currentScore = 0;
    private float timeRemaining = 10f;
    private bool timerIsRunning = false;
    private Coroutine autoContinueCoroutine;

    void Start()
    {
        currentScore = 0;
        UpdateScoreUI();
        
        InitializeQuestions();
        DisplayQuestion(currentQuestionIndex);
        nextButton.onClick.AddListener(OnNextButtonClicked);
        
        timerIsRunning = true;
        UpdateTimerDisplay();
        
        progressBar.fillAmount = 0f;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                TimeExpired();
            }
        }
    }

    void InitializeQuestions()
    {
        questions = new List<Question>
        {
            new Question
            {
                questionText = "What does a drone typically use to fly?",
                answers = new string[] { "Wheels", "Wings only", "Propellers", "Jet engine" },
                correctAnswerIndex = 2
            },
            new Question
            {
                questionText = "What is the main controller of a drone called?",
                answers = new string[] { "Remote control", "Dashboard", "Gamepad", "Antenna" },
                correctAnswerIndex = 0
            },
            new Question
            {
                questionText = "Which of these devices is commonly used to control an AR drone?",
                answers = new string[] { "Television remote", "Smartphone or tablet", "Computer keyboard", "Gaming console controller" },
                correctAnswerIndex = 1
            },
            new Question
            {
                questionText = "What is a common way for an AR drone to communicate with its controller?",
                answers = new string[] { "Bluetooth", "Infrared", "Wi-Fi", "Radio waves" },
                correctAnswerIndex = 2
            },
            new Question
            {
                questionText = "What is a primary power source for an AR drone?",
                answers = new string[] { "Solar panels", "Gasoline engine", "Battery", "Wind turbine" },
                correctAnswerIndex = 2
            },
            new Question
            {
                questionText = "What is a basic function that most AR drone control apps allow?",
                answers = new string[] { "Cooking recipes", "Sending text messages", "Making the drone fly up and down", "Playing music" },
                correctAnswerIndex = 2
            },
            new Question
            {
                questionText = "Which of these is a common sensor found on an AR drone?",
                answers = new string[] { "Taste sensor", "Smell sensor", "Camera", "Touch sensor" },
                correctAnswerIndex = 2
            },
            new Question
            {
                questionText = "What is a typical environment where AR drones are often flown?",
                answers = new string[] { "Underwater", "In outer space", "Indoors or outdoors", "Inside a computer" },
                correctAnswerIndex = 2
            },
            new Question
            {
                questionText = "What might you need to do before flying an AR drone for the first time?",
                answers = new string[] { "Bake a cake", "Register the drone or calibrate it", "Learn a new language", "Plant a tree" },
                correctAnswerIndex = 1
            }
        };
    }

    void DisplayQuestion(int index)
    {
        if (index < 0 || index >= questions.Count) return;
        
        if (autoContinueCoroutine != null)
        {
            StopCoroutine(autoContinueCoroutine);
            autoContinueCoroutine = null;
        }
        
        ResetQuestionState();
        
        Question currentQuestion = questions[index];
        questionText.text = currentQuestion.questionText;
        
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];
                int buttonIndex = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(buttonIndex));
                answerButtons[i].interactable = true;
                answerButtons[i].image.color = Color.white;
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void ResetQuestionState()
    {
        timeRemaining = 10f;
        timerIsRunning = true;
        UpdateTimerDisplay();
        
        nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Skip";
        nextButton.interactable = false;
        selectedAnswerIndex = -1;
        answerSelected = false;
    }

    void OnAnswerSelected(int index)
    {
        if (answerSelected) return;
        
        if (selectedAnswerIndex != -1)
        {
            answerButtons[selectedAnswerIndex].image.color = Color.white;
        }
        
        selectedAnswerIndex = index;
        answerSelected = true;
        answerButtons[index].image.color = new Color(1f, 0.8f, 0f);
        nextButton.interactable = true;
    }

    void OnNextButtonClicked()
    {
        if (timerIsRunning)
        {
            ShowAnswers();
            timerIsRunning = false;
            timeRemaining = 0;
            UpdateTimerDisplay();
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
            
            autoContinueCoroutine = StartCoroutine(AutoContinueAfterDelay(2f));
        }
        else
        {
            MoveToNextQuestion();
        }
    }

    void ShowAnswers()
    {
        answerButtons[questions[currentQuestionIndex].correctAnswerIndex].image.color = Color.green;
        
        if (selectedAnswerIndex != -1)
        {
            if (selectedAnswerIndex != questions[currentQuestionIndex].correctAnswerIndex)
            {
                answerButtons[selectedAnswerIndex].image.color = Color.red;
                if (audioSource && wrongSound) audioSource.PlayOneShot(wrongSound);
            }
            else
            {
                if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
            }
            
            if (selectedAnswerIndex == questions[currentQuestionIndex].correctAnswerIndex)
            {
                currentScore += 10;
            }
            else
            {
                currentScore = Mathf.Max(0, currentScore - 10);
            }
            UpdateScoreUI();
        }
        
        foreach (Button button in answerButtons)
        {
            button.interactable = false;
        }
    }

    void MoveToNextQuestion()
    {
        currentQuestionIndex++;
        UpdateProgressBar();
        
        if (currentQuestionIndex < questions.Count)
        {
            DisplayQuestion(currentQuestionIndex);
        }
        else
        {
            EndQuiz();
        }
    }

    void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            float targetProgress = (float)(currentQuestionIndex + 1) / questions.Count;
            StartCoroutine(AnimateProgressBar(targetProgress));
        }
    }

    IEnumerator AnimateProgressBar(float targetValue)
    {
        float duration = 0.5f;
        float startValue = progressBar.fillAmount;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            progressBar.fillAmount = Mathf.Lerp(startValue, targetValue, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        progressBar.fillAmount = targetValue;
    }

    void TimeExpired()
    {
        timerIsRunning = false;
        timeRemaining = 0;
        UpdateTimerDisplay();
        ShowAnswers();
        nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
        
        autoContinueCoroutine = StartCoroutine(AutoContinueAfterDelay(2f));
    }

    IEnumerator AutoContinueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        MoveToNextQuestion();
    }

    void EndQuiz()
    {
        questionText.text = "Quiz completed! Final score: " + currentScore;
        foreach (Button button in answerButtons)
        {
            button.gameObject.SetActive(false);
        }
        nextButton.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        progressBar.fillAmount = 1f;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
            timerText.color = timeRemaining < 4f ? Color.red : Color.white;
        }
    }
}