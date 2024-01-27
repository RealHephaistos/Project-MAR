using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameplayLoopMainScene : MonoBehaviour
{
    [SerializeField] private UIDocument startUI;
    private Label countDownLabel;
    private bool isCountDown = true;
    [SerializeField] private float countDownTime = 5.0f;
    private float countDownCurrentTime;

    [SerializeField] private UIDocument mainUI;
    private Label timerLabel;
    private Label scoreLabel;

    [SerializeField] private UIDocument gameOverUI;
    private Label cleanedGraffitiLabel;
    private Label missedGraffitiLabel;

    [SerializeField] private float gameTime = 60f;
    private float currentTime;

    private int maxNbGrafitti;
    private int currentNbGrafitti;

    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        mainUI.rootVisualElement.style.display = DisplayStyle.None;
        gameOverUI.rootVisualElement.style.display = DisplayStyle.None;
        countDownLabel = startUI.rootVisualElement.Q<Label>("countDownLabel");
        countDownCurrentTime = countDownTime;

        currentTime = gameTime;
        timerLabel = mainUI.rootVisualElement.Q<Label>("timerLabel");
        scoreLabel = mainUI.rootVisualElement.Q<Label>("scoreLabel");

        maxNbGrafitti = CountGraffiti();
        currentNbGrafitti = maxNbGrafitti;
        timerLabel.text = currentTime.ToString(currentTime + "s/" + gameTime + "s");
        scoreLabel.text = currentNbGrafitti + "/" + maxNbGrafitti + " Max Graffiti";

        // Desactivate input
        player.GetComponent<PlayerInput>().DeactivateInput();
        // Desactivate water gun
        player.GetComponentInChildren<WaterGunController>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isCountDown)
        {
            CountDown();
        }
        else
        {
            GameLoop();
        }
    }

    private void CountDown()
    {
        // Update timer
        countDownCurrentTime -= Time.deltaTime;
        countDownLabel.text = countDownCurrentTime.ToString("0.0") + "s";
        if (countDownCurrentTime <= 0.0f)
        {
            countDownCurrentTime = 0.0f;
            countDownLabel.text = countDownCurrentTime.ToString("0.0") + "s";
            isCountDown = false;

            // Display "GO" on screen for 1 second
            countDownLabel.text = "GO";
            Invoke("HideCountDown", 1.0f);

            // Activate main screen
            mainUI.rootVisualElement.style.display = DisplayStyle.Flex;

            // Activate input
            player.GetComponent<PlayerInput>().ActivateInput();
            // Activate water gun
            player.GetComponentInChildren<WaterGunController>().enabled = true;
        }
    }

    private void GameLoop()
    {
        // Update timer
        currentTime -= Time.deltaTime;
        timerLabel.text = currentTime.ToString("0.0") + "s/" + gameTime + "s";
        if (currentTime <= 0.0f)
        {
            currentTime = 0.0f;
            timerLabel.text = currentTime.ToString("0.0") + "s/" + gameTime + "s";
            GameOverScreen();
        }
    }

    private void GameOverScreen()
    {
        // Desactivate input
        player.GetComponent<PlayerInput>().DeactivateInput();
        // Desactivate water gun
        player.GetComponentInChildren<WaterGunController>().enabled = false;

        // Display game over screen
        mainUI.rootVisualElement.style.display = DisplayStyle.None;
        gameOverUI.rootVisualElement.style.display = DisplayStyle.Flex;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        // Update score
        cleanedGraffitiLabel = gameOverUI.rootVisualElement.Q<Label>("cleanedGraffitiLabel");
        missedGraffitiLabel = gameOverUI.rootVisualElement.Q<Label>("missedGraffitiLabel");
        cleanedGraffitiLabel.text = "You cleaned " + (maxNbGrafitti - currentNbGrafitti) + " graffiti";
        missedGraffitiLabel.text = "You missed " + currentNbGrafitti + " graffiti";

        // Main menu button
        gameOverUI.rootVisualElement.Q<Button>("mainMenuButton").RegisterCallback<ClickEvent>(ev =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        });
    }

    private void HideCountDown()
    {
        countDownLabel.text = "";
        startUI.rootVisualElement.style.display = DisplayStyle.None;
    }

    private int CountGraffiti()
    {
        return GameObject.FindGameObjectsWithTag("Decal").Length;
    }
}
