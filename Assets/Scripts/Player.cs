using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public int Health { get; private set; }
    public int Power { get; private set; }

    private int initialHealth;
    private float weaponCoolDown;
    private float weaponCoolDownThreshold;

    public static Action OnPlayerKilled;

    [SerializeField] Slider HealthBar;
    [SerializeField] Image SliderBG;
    [SerializeField] Bullet bullet;

    /// <summary>
    /// To Set Player 's values, for now I give a hard coded values, but we can read them from a database either external or even scriptable objects
    /// </summary>
    void InitializePlayer()
    {
        Health = 100;
        Power = 10;
        initialHealth = Health;
        transform.position = Vector3.zero;
        HealthBar.value = 1;
        SliderBG.color = Color.green;
        weaponCoolDownThreshold = Random.Range(3.5f, 5.5f);
        weaponCoolDown = 0;
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        InitializePlayer();
    }

    private void OnEnable()
    {
        FinishGamePopup.OnRestartGame += InitializePlayer;
    }

    private void OnDisable()
    {
        FinishGamePopup.OnRestartGame -= InitializePlayer;
    }

    private void Update()
    {
        weaponCoolDown -= Time.deltaTime;
        if(weaponCoolDown <= 0)
        {
            weaponCoolDown = weaponCoolDownThreshold;
            AttackAnEnemy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() is Enemy enemy)
        {
            GetDamage(enemy.Power);
        }
    }
    /// <summary>
    /// Receives a damage from an enemy, and the damage 's value will depend on the enemy 's power
    /// </summary>
    /// <param name="damage"></param>
    public void GetDamage(int damage)
    {
        Health -= damage;
        if(Health<=0)
        {
            HealthBar.value = 0;
            OnPlayerKilled?.Invoke();
        }
        else
        {
            float fillAmount = (float)Health / initialHealth;
            HealthBar.value = fillAmount;
            if (fillAmount <= 0.25f)
                SliderBG.color = Color.red;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            Enemy enemy = collision.transform.parent.GetComponent<Enemy>();
            GetDamage(enemy.Power);
        }
    }
    /// <summary>
    /// Choose a random active enemy to attack it
    /// </summary>
    private void AttackAnEnemy()
    {
        Enemy enemy =Spawner.Instance.PickAnEnemy();
        if(enemy != null) 
        {
            bullet.gameObject.SetActive(true);
            bullet.Attack(enemy.transform, "Enemy");
        }
    }
}
