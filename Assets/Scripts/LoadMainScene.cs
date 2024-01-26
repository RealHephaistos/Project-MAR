using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadMainScene : MonoBehaviour
{
    [SerializeField] private UIDocument mainUI;
    private Button startButton;

    // Start is called before the first frame update
    void Start()
    {
       startButton = mainUI.rootVisualElement.Q<Button>("startButton");
        startButton.clicked += () =>
        {
            SceneManager.LoadScene(1);
        };
    }
}
