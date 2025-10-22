using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using System;
using UnityEngine.Events;
using DemoMPTK;
using MPTKDemoCatchMusic;

public class NoteCreator: MonoBehaviour
{
    public MidiFilePlayer midiFilePlayer;

    public MidiStreamPlayer midiStreamPlayer;

    public GameObject Plane;
    public float minZ, maxZ, minX, maxX;

    public Material MatNewNote;
    public NoteView NoteDisplay;

    int[] countZ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Default size of a Unity Plan
        float planSize = 10f;

        minZ = Plane.transform.localPosition.z - Plane.transform.localScale.z * planSize / 2f;
        maxZ = Plane.transform.localPosition.z + Plane.transform.localScale.z * planSize / 2f;

        minX = Plane.transform.localPosition.x - Plane.transform.localScale.x * planSize / 2f;
        maxX = Plane.transform.localPosition.x + Plane.transform.localScale.x * planSize / 2f;

        if (midiFilePlayer != null)
        {
            // No listener defined. Set now by script. NotesToPlay will be called for each new notes read from Midi file
            Debug.Log("MusicView: Maestro Event MidiFilePlayer.OnEventNotesMidi set by script (see MusicView.cs). Setting with the inspector is also possible.");
            midiFilePlayer.OnEventNotesMidi.AddListener(NotesToPlay);
        }
        else
            Debug.Log("No MidiFilePlayer prefab detected. Add it to your Hierarchy and defined it in MusicView inspector.");
    }

    public void NotesToPlay(List<MPTKEvent> notes)
    {


        // Count GameObject for each z position in the plan. Useful to stack them.
        countZ = new int[Convert.ToInt32(maxZ - minZ) + 1];

        //Debug.Log(midiFilePlayer.MPTK_PlayTime.ToString() + " count:" + notes.Count);
        foreach (MPTKEvent mptkEvent in notes)
        {
            switch (mptkEvent.Command)
            {
                case MPTKCommand.NoteOn:
                    //Debug.Log($"NoteOn Channel:{note.Channel}  Preset index:{midiStreamPlayer.MPTK_ChannelPresetGetIndex(note.Channel)}  Preset name:{midiStreamPlayer.MPTK_ChannelPresetGetName(note.Channel)}");
                    if (mptkEvent.Value > 40 && mptkEvent.Value < 100)// && note.Channel==1)
                    {
                        // Z position is set depending the note value:mptkEvent.Value
                        float z = Mathf.Lerp(minZ, maxZ, (mptkEvent.Value - 40) / 60f);
                        countZ[Convert.ToInt32(z - minZ)]++;
                        // Y position is set depending the count of object at the z position
                        Vector3 position = new Vector3(maxX, 2 + countZ[Convert.ToInt32(z - minZ)] * 4f, z);
                        // Instantiate a GameObject to represent this midi event in the 3D world
                        NoteView noteview = Instantiate<NoteView>(NoteDisplay, position, Quaternion.identity);
                        noteview.gameObject.SetActive(true);
                        noteview.hideFlags = HideFlags.HideInHierarchy;
                        noteview.midiStreamPlayer = midiStreamPlayer;
                        noteview.noteOn = mptkEvent; // the midi event is attached to the gameobjet, will be played more later
                        noteview.gameObject.GetComponent<Renderer>().material = MatNewNote;
                        // See noteview.cs: update() move the note along the plan until they fall out, then they are played
                        noteview.zOriginal = position.z;

                        if (!NoteView.FirstNotePlayed)
                            PlaySound();
                    }
                    break;
            }
        }
    }

    private void PlaySound()
    {
        // Some sound for waiting the notes, will be disabled at the fist note played ...
        //! [Example PlayNote]
        midiStreamPlayer.MPTK_PlayEvent
        (
            new MPTKEvent()
            {
                Channel = 9,
                Duration = 999999,
                Value = 48,
                Velocity = 100
            }
        );
        //! [Example PlayNote]
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
