using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text enemyTxt;
    [SerializeField] GameObject LosePopup;

    int killedEnemies;

    private void Start()
    {
        killedEnemies = 0;
        SetEnemyText();
    }

    private void OnEnable()
    {
        Player.OnPlayerKilled += ShowFinishGame;
        Enemy.OnEnemyKilled += IncreaseKilledEnemies;
        FinishGamePopup.OnRestartGame += ResetKilledEnemiesCount;
    }
    private void OnDestroy()
    {
        Player.OnPlayerKilled -= ShowFinishGame;
        Enemy.OnEnemyKilled -= IncreaseKilledEnemies;
        FinishGamePopup.OnRestartGame -= ResetKilledEnemiesCount;
    }

    /// <summary>
    /// Update Killed Enemies Text
    /// </summary>
    private void SetEnemyText() => enemyTxt.text = $"Enemy: {killedEnemies}";
   
    /// <summary>
    /// Set Killed Enemies Text
    /// </summary>
    private void IncreaseKilledEnemies()
    {
        killedEnemies++;
        SetEnemyText();
    }

    /// <summary>
    /// Shows Finish Game Popup
    /// </summary>
    private void ShowFinishGame()
    {
        LosePopup.SetActive(true);
    }

    /// <summary>
    /// Reset Killed Enemies Count with updating the text
    /// </summary>
    private void ResetKilledEnemiesCount()
    {
        killedEnemies = 0;
        SetEnemyText();
    }
}
