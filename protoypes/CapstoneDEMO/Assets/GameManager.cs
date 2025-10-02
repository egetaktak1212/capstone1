using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    List<int> nowpressed = new List<int>();
    public MidiDebug script;

    List<int> cmajor = new List<int>();


    void Start()
    {
        nowpressed = script.getPressed();
        nowpressed.Sort();

        cmajor = new List<int>() {48, 52, 55};

    }

    // Update is called once per frame
    void Update()
    {
        nowpressed = script.getPressed();
        nowpressed.Sort();

        Debug.Log(nowpressed.SequenceEqual(cmajor));




    }
}
