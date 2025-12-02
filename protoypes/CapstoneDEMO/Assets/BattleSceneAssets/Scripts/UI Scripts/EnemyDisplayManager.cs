using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDisplayManager : MonoBehaviour
{
    
    public List<Sprite> speakingSprites = new List<Sprite>();

    public List<Sprite> defaultSprites = new List<Sprite>();

    public List<Sprite> playerWinSprites = new List<Sprite>();

    public List<Sprite> playerLoseSprites = new List<Sprite>();

    public Image enemyDisplayImage;

    void Start()
    {
        defaultDisplay();
    }

    
    void Update()
    {
        
    }

    public void speakingDisplay() { 
        setDisplayTo(randomlyPickFromArray(speakingSprites));
    }

    public void defaultDisplay()
    {
        setDisplayTo(randomlyPickFromArray(defaultSprites));
    }

    public void playerWinDisplay()
    {
        setDisplayTo(randomlyPickFromArray(playerWinSprites));
    }

    public void playerLoseDisplay()
    {
        setDisplayTo(randomlyPickFromArray(playerLoseSprites));
    }

    Sprite randomlyPickFromArray(List<Sprite> array) {
        return array[Random.Range(0, array.Count)];
    }

    void setDisplayTo(Sprite sprite) { 
        enemyDisplayImage.sprite = sprite;
    }

}
