using System.Collections;
using System.Collections.Generic;
using MidiPlayerTK;
using UnityEngine;

public class NotePreview : MonoBehaviour
{

    List<int> firstNotes = new List<int>();

    Dictionary<string, GameObject> children = new Dictionary<string, GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            children[child.name] = child.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    int normalizeNoteValue(int value)
    {
        return (value % 24) + 48;
    }

    public IEnumerator startNotePreview(string songName)
    {
        MidiLoad midiLoader = new MidiLoad();
        midiLoader.MPTK_Load(songName);


        foreach (var note in midiLoader.MPTK_MidiEvents)
        {
            if (note.Track == 1 && note.Command == MPTKCommand.NoteOn)
            {
                int value = normalizeNoteValue(note.Value);
                if (!firstNotes.Contains(value))
                {
                    firstNotes.Add(value);
                }
                if (firstNotes.Count == 3)
                    break;
            }
        }

        yield return previewNotesCoroutine();
    }

    IEnumerator previewNotesCoroutine() {
        highlightKeys();
        yield return new WaitForSeconds(5f);
        hideKeys();
    }



    void highlightKeys()
    {
        foreach (int value in firstNotes)
        {
            if (!FullOrHalf.instance.IsItHalf() || (FullOrHalf.instance.IsItHalf() && value < 60))
            {
                children[value.ToString()].SetActive(true);
            }
            
        }
    }

    void hideKeys()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        firstNotes.Clear();
    }



}
