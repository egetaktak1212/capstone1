using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{

    public int maxhealth = 100;

    public int health = 100;

    public Image deathBar;

    public static PlayerInfo instance;

    public bool dead = false;

    public int mistakeCounter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    public void decreaseHealth(int damage) {
        health -= damage;
        Mathf.Clamp(health, 0, maxhealth);
        updateBar();
        if (health <= 0) {
            dead = true;
        }
    }

    public void increaseHealth(int heal)
    {
        health += heal;
        Mathf.Clamp(health, 0, maxhealth);
        updateBar();
    }

    void updateBar() {
        Mathf.Clamp(health, 0, maxhealth);

        float tempHealth = (float) health;
        float tempMaxHealth = (float) maxhealth;

        deathBar.fillAmount = tempHealth/tempMaxHealth;
    }

    public void resetLife() {
        health = maxhealth;
        dead = false;
        mistakeCounter = 0;
        updateBar();
    }

    public void madeAMistake() {
        mistakeCounter++;
        ComboManager.instance.noteHappened(false);
    }




}
