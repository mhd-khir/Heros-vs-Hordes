using System;
using UnityEngine;
using UnityEngine.UI;

public class FinishGamePopup : MonoBehaviour
{
    [SerializeField] Button PlayAgain;
    [SerializeField] Button Exit;

    public static Action OnRestartGame;

    private void Awake()
    {
        PlayAgain.onClick.AddListener(PlayAgainOnClick);
        Exit.onClick.AddListener(ExitOnClick);
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    /// <summary>
    /// Allows Player To Play again
    /// </summary>
    private void PlayAgainOnClick()
    {
        OnRestartGame?.Invoke();
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Exit Game
    /// </summary>
    private void ExitOnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
