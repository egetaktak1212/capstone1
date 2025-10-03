using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    List<int> nowpressed = new List<int>();
    public MidiDebug script;

    List<int> cmajor = new List<int>() { 48, 52, 55 };
    List<int> fmajor = new List<int>() {53, 57, 60};
    List<int> eminor = new List<int>() { 52, 55, 59 };

    [SerializeField] GameObject ccheck;
    [SerializeField] GameObject fcheck;
    [SerializeField] GameObject echeck;
    [SerializeField] GameObject cx;
    [SerializeField] GameObject fx;
    [SerializeField] GameObject ex;



    int question = 1;

    void Start()
    {
        nowpressed = script.getPressed();
        nowpressed.Sort();

        

        ccheck.SetActive(false);
        fcheck.SetActive(false);
        echeck.SetActive(false);
        cx.SetActive(false);
        fx.SetActive(false);
        ex.SetActive(false);

        StartCoroutine(chordsequence());

    }

    // Update is called once per frame
    void Update()
    {





    }

    IEnumerator chordsequence()
    {
        cx.SetActive(true);
        while (true) {
            nowpressed = script.getPressed();
            nowpressed.Sort();

            if (nowpressed.SequenceEqual(cmajor))
            {
                break;
            }

            yield return null;
        }
        cx.SetActive(false);
        ccheck.SetActive(true);

        yield return new WaitForSeconds(1f);

        fx.SetActive(true);
        while (true)
        {
            nowpressed = script.getPressed();
            nowpressed.Sort();

            if (nowpressed.SequenceEqual(fmajor))
            {
                break;
            }

            yield return null;
        }
        fx.SetActive(false);
        fcheck.SetActive(true);

        yield return new WaitForSeconds(1f);

        ex.SetActive(true);
        while (true)
        {
            nowpressed = script.getPressed();
            nowpressed.Sort();

            if (nowpressed.SequenceEqual(eminor))
            {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        ex.SetActive(false);
        echeck.SetActive(true);

    }



}
