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
    private float progressIncrement;

    void Start()
    {
        currentScore = 0;
        UpdateScoreUI();
        
        InitializeQuestions();
        DisplayQuestion(currentQuestionIndex);
        nextButton.onClick.AddListener(OnNextButtonClicked);
        
        timerIsRunning = true;
        UpdateTimerDisplay();
        
        progressIncrement = 1f / questions.Count;
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
                answers = new string[]
                {
                    "Wheels",
                    "Wings only",
                    "Propellers",
                    "Jet engine"
                },
                correctAnswerIndex = 2
            },
            new Question
            {
                questionText = "What is the main controller of a drone called?",
                answers = new string[]
                {
                    "Remote control",
                    "Dashboard",
                    "Gamepad",
                    "Antenna"
                },
                correctAnswerIndex = 0
            }
        };
    }

    void DisplayQuestion(int index)
    {
        if (index < 0 || index >= questions.Count) return;
        
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
        }
        else
        {
            MoveToNextQuestion();
        }
    }

    void ShowAnswers()
    {
        answerButtons[questions[currentQuestionIndex].correctAnswerIndex].image.color = Color.green;
        
        if (selectedAnswerIndex != -1 && selectedAnswerIndex != questions[currentQuestionIndex].correctAnswerIndex)
        {
            answerButtons[selectedAnswerIndex].image.color = Color.red;
        }
        
        foreach (Button button in answerButtons)
        {
            button.interactable = false;
        }
        
        if (selectedAnswerIndex != -1)
        {
            if (selectedAnswerIndex == questions[currentQuestionIndex].correctAnswerIndex)
            {
                currentScore += 10;
                if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
            }
            else
            {
                currentScore = Mathf.Max(0, currentScore - 10);
                if (audioSource && wrongSound) audioSource.PlayOneShot(wrongSound);
            }
            UpdateScoreUI();
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
            float targetProgress = currentQuestionIndex * progressIncrement;
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
        
        if (selectedAnswerIndex == -1 && audioSource && wrongSound)
        {
            audioSource.PlayOneShot(wrongSound);
        }
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
        progressBar.fillAmount = 1f; // Remplir complètement à la fin
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