
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;


public class NoteCreator : MonoBehaviour
{
    public MidiFilePlayer midiFilePlayer;

    public MidiStreamPlayer midiStreamPlayer;

    public NoteView2 NoteDisplay;

    public NoteEmpties noteEmpties;


    public static float Speed = 7f;

    int[] countZ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (midiFilePlayer != null)
        {
            midiFilePlayer.OnEventNotesMidi.AddListener(NotesToPlay);
        }
        else
            Debug.Log("No MidiFilePlayer prefab detected. Add it to your Hierarchy and defined it in MusicView inspector.");


    }

    public void NotesToPlay(List<MPTKEvent> notes)
    {


        // Count GameObject for each z position in the plan. Useful to stack them.

        //Debug.Log(midiFilePlayer.MPTK_PlayTime.ToString() + " count:" + notes.Count);
        foreach (MPTKEvent mptkEvent in notes)
        {
            switch (mptkEvent.Command)
            {
                case MPTKCommand.NoteOn:
                    //Debug.Log($"NoteOn Channel:{note.Channel}  Preset index:{midiStreamPlayer.MPTK_ChannelPresetGetIndex(note.Channel)}  Preset name:{midiStreamPlayer.MPTK_ChannelPresetGetName(note.Channel)}");

                    // Z position is set depending the note value:mptkEvent.Value
                    //float z = Mathf.Lerp(minZ, maxZ, (mptkEvent.Value - 40) / 60f);
                    //countZ[Convert.ToInt32(z - minZ)]++;



                    // Y position is set depending the count of object at the z position
                    //Vector3 position = new Vector3(maxX, 2 + countZ[Convert.ToInt32(z - minZ)] * 4f, z);
                    // Instantiate a GameObject to represent this midi event in the 3D world
                    if (mptkEvent.Track == 1)
                    {
                        //MidiInputs.instance.upcomingNotes.Add(normalizeNoteValue(mptkEvent.Value));
                        Vector3 position = getNoteStartPosition(mptkEvent.Value);

                        NoteView2 noteview = Instantiate<NoteView2>(NoteDisplay, position, Quaternion.identity);

                        noteview.noteOn = mptkEvent;
                        noteview.gameObject.SetActive(true);
                        noteview.hideFlags = HideFlags.HideInHierarchy;
                        noteview.midiStreamPlayer = midiStreamPlayer;
                        noteview.zOriginal = position.z;
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

    int normalizeNoteValue(int value)
    {
        return (value % 24) + 48;
    }

    Vector3 getNoteStartPosition(int value)
    {
        return noteEmpties.GetNotePosition(normalizeNoteValue(value));
    }



    // Update is called once per frame
    void Update()
    {
    }
}
