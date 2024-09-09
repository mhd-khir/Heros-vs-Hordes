using System;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;
public class Enemy : MonoBehaviour
{
    public int Health { get; private set; }
    public int Power { get; private set; }

    private float moveSpeed;
    private int initialHealth;
    private float distanceToAttack;
    private float weaponCoolDownTimer;
    private float weaponCoolDownThreshold;

    [SerializeField] Slider HealthBar;
    [SerializeField] Image SliderBG;
    [SerializeField] Bullet bullet;
    SpriteRenderer SP;
    Rigidbody2D rb;
    Transform player;

    public static Action OnEnemyKilled;

    /// <summary>
    /// To Set Enemy 's values, enemies should not have same prperties that is why I gave random values, but for sure for real scenarios we need to use DB for storing and modifiying values
    /// </summary>
    private void InitializeTheEnemy()
    {
        Health = Random.Range(1, 6);
        Power = Random.Range(1, 4);
        initialHealth = Health;
        HealthBar.value = 1;
        SP.color = new Color32((byte)Random.Range(1, 255), (byte)Random.Range(1, 255), (byte)Random.Range(1, 255), 255);
        moveSpeed = Random.Range(0.25f, 1f);
        distanceToAttack = Random.Range(0.5f, 1.5f);
        weaponCoolDownThreshold = Random.Range(0.5f, 1f);
        weaponCoolDownTimer = 0;
        SliderBG.color = Color.green;
    }
    private void Awake()
    {
        SP = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = Player.Instance.transform;
    }
    private void OnEnable()
    {
        InitializeTheEnemy();
    }

    /// <summary>
    /// Eliminating an enemy when it takes enough damages, but I use object pooling for performance optimization, so we can use killed items again rather than instantiating new ones
    /// </summary>
    public void KillEnemy()
    {
        OnEnemyKilled?.Invoke();
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            GetDamage(Player.Instance.Power);
        }
    }

    /// <summary>
    /// Receive a damage from Player
    /// </summary>
    /// <param name="damage"></param>
    public void GetDamage(int damage)
    {
        Health -= damage;
        if(Health < 0)
        {
            KillEnemy();
        }
        else
        {
            float fillAmount = (float)Health / initialHealth;
            HealthBar.value = fillAmount;
            if (fillAmount < 0.25f)
                SliderBG.color = Color.red;
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
        if (Vector2.Distance(transform.position, player.position) <= distanceToAttack)
        {
            if (weaponCoolDownTimer <= 0)
            {
                weaponCoolDownTimer = weaponCoolDownThreshold;
                Shoot();
            }
            else
            {
                weaponCoolDownTimer -= Time.deltaTime;
            }
        }
        else
            weaponCoolDownTimer -= Time.deltaTime;

    }
    /// <summary>
    /// Enemy will shoot the player with a bullet
    /// </summary>
    private void Shoot()
    {
        bullet.gameObject.SetActive(true);
        bullet.Attack(player, "Player");
    }

}
