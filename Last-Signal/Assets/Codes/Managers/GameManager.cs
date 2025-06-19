using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField, Tooltip("SOS-timer duur (seconden)")]
    private float sosTimerDuration = 300f;
    private float sosTimer;
    private bool gameOver = false;
    private bool timerActivated = false; // Nieuwe vlag voor activering
    private GoldenUnit goldenUnit;
    private float lastLogTime; // Voor log-interval
    private float logInterval = 1f; // Log elke seconde
    [SerializeField, Tooltip("Kosten om timer te activeren (materialen)")]
    private int activationCost = 50; // Standaard kosten

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        goldenUnit = FindObjectOfType<GoldenUnit>();
        if (goldenUnit == null)
        {
            Debug.LogError("Geen GoldenUnit gevonden in de scène!");
        }
        sosTimer = 0f; // Start uitgeschakeld
        Debug.Log("SOS-timer is uitgeschakeld, activeer met " + activationCost + " materialen.");
        lastLogTime = Time.time;
    }

    private void Update()
    {
        if (gameOver) return;

        if (timerActivated && sosTimer > 0)
        {
            sosTimer -= Time.deltaTime;
            if (Time.time - lastLogTime >= logInterval)
            {
                Debug.Log("SOS-timer resterend: " + Mathf.Ceil(sosTimer) + " seconden.");
                lastLogTime = Time.time;
            }
            if (sosTimer <= 0)
            {
                OnGameWon();
            }
        }

        if (goldenUnit != null && goldenUnit.CurrentHP <= 0 && !gameOver)
        {
            OnGameLost();
        }

        // Tijdelijke activering met toets 'T'
        if (Input.GetKeyDown(KeyCode.T) && !timerActivated)
        {
            ActivateTimer();
        }
    }

    private void ActivateTimer()
    {
        if (MaterialManager.Instance != null && MaterialManager.Instance.SpendMaterials(activationCost))
        {
            timerActivated = true;
            sosTimer = sosTimerDuration;
            Debug.Log("SOS-timer geactiveerd met " + activationCost + " materialen.");
        }
        else
        {
            Debug.LogWarning("Niet genoeg materialen om timer te activeren!");
        }
    }

    public void OnGameLost()
    {
        gameOver = true;
        Time.timeScale = 0; // Pauzeer de game
        Debug.Log("Spel verloren! GoldenUnit vernietigd.");
    }

    public void OnGameWon()
    {
        gameOver = true;
        Time.timeScale = 0; // Pauzeer de game
        Debug.Log("Spel gewonnen! SOS-timer verlopen zonder verlies.");
    }
}