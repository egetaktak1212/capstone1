using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{

    public int maxhealth = 100;

    public int health = 100;

    [SerializeField] Image redBar;
    [SerializeField] Image brownBar;

    public static EnemyInfo instance;

    public bool dead = false;

    List<int> damageLevels = new List<int>
    {
    1,
    2,
    5,
    8,
    10,
    15
    };



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerInfo.instance.madeAMistakeEvent += updateBar;
        updateBar();
    }

    // Update is called once per frame
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }

        instance = this;

        maxhealth = 100;
        health = 100;

    }

    // Update is called once per frame
    void Update()
    {
        //two bars
        //when damage, lower the red bar
        //when player mistake, set the brown bar to the red


        
    }

    public void dealDamage() {
        int rank = PlayerInfo.instance.comboRank;

        health -= damageLevels[rank];
        health = math.clamp(health, 0, maxhealth);
        updateRed();
        if (health <= 0)
        {
            Die();
        }
    }

    void Die() {
        dead = true;
        updateBar();
    }

    void updateBar()
    {
        updateBrown();
        updateRed();
    }

    public void resetLife()
    {
        health = maxhealth;
        dead = false;
        updateBar();
    }

    void updateRed() {
        Mathf.Clamp(health, 0, maxhealth);

        float tempHealth = (float)health;
        float tempMaxHealth = (float)maxhealth;

        redBar.fillAmount = tempHealth / tempMaxHealth;
    }

    void updateBrown()
    {
        Mathf.Clamp(health, 0, maxhealth);

        float tempHealth = (float)health;
        float tempMaxHealth = (float)maxhealth;

        brownBar.fillAmount = tempHealth / tempMaxHealth;
    }








}
