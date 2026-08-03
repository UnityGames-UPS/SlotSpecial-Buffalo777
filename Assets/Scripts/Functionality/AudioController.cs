using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource bg_adudio;
    [SerializeField] internal AudioSource audioPlayer_wl;
    [SerializeField] internal AudioSource audioPlayer_button;
    [SerializeField] internal AudioSource audioSpin_button;
    [SerializeField] internal AudioSource coins;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip[] Bonusclips;
    [SerializeField] private AudioSource bg_audioBonus;

    private void Start()
    {
        if (bg_adudio) bg_adudio.Play();
        audioPlayer_button.clip = clips[clips.Length-1];
        audioSpin_button.clip = clips[clips.Length-2];
    }

    private bool isForceMuted = false;
    private readonly Dictionary<AudioSource, bool> preFocusMuteState = new Dictionary<AudioSource, bool>();

    internal void CheckFocusFunction(bool focus, bool IsSpinning)
    {
        bool forceMute = !focus;
        if (forceMute == isForceMuted) return;
        isForceMuted = forceMute;

        var sources = new[] { bg_adudio, audioPlayer_wl, audioPlayer_button, audioSpin_button, coins, bg_audioBonus };

        if (forceMute)
        {
            foreach (var source in sources)
            {
                if (source == null) continue;
                preFocusMuteState[source] = source.mute;
                source.mute = true;
            }
        }
        else
        {
            foreach (var source in sources)
            {
                if (source == null) continue;
                source.mute = preFocusMuteState.TryGetValue(source, out bool prevMuted) ? prevMuted : source.mute;
            }
            if (!IsSpinning)
            {
                StopWLAaudio();
            }
        }
    }

    internal void SwitchBGSound(bool isfreeSpin)
    {
        if(isfreeSpin)
        {
            if (bg_audioBonus) bg_audioBonus.enabled = true;
            if (bg_adudio) bg_adudio.enabled = false;
        }
        else
        {
            if (bg_audioBonus) bg_audioBonus.enabled = false;
            if (bg_adudio) bg_adudio.enabled = true;
        }
    }

    internal void PlayCoinSounds(){
        // StopCoinSounds();
        coins.Play();
        Invoke(nameof(StopCoinSounds),1f);

    }

    internal void StopCoinSounds(){
        coins.Stop();
    }
    internal void PlayWLAudio(string type)
    {
        audioPlayer_wl.loop = false;
        int index = 0;
        switch (type)
        {
            case "win":
                index = 2;
                break;
            case "spinStop":
                index = 0;
                break;
            case "megaWin":
                index = 1;
                break;
        }
        StopWLAaudio();
        audioPlayer_wl.clip = clips[index];
        audioPlayer_wl.Play();

    }

    internal void PlayBonusAudio(string type)
    {
        audioPlayer_wl.loop = false;
        int index = 0;
        switch (type)
        {
            case "win":
                index = 0;
                break;
            case "lose":
                index = 1;
                break;
            case "cycleSpin":
                index = 2;
                break;
        }
        StopBonusAaudio();


    }

    internal void PlayButtonAudio()
    {
        audioPlayer_button.Play();
    }

    internal void PlaySpinButtonAudio()
    {
        audioSpin_button.Play();
    }

    internal void StopWLAaudio()
    {
        audioPlayer_wl.Stop();
        audioPlayer_wl.loop = false;
    }

    internal void StopBonusAaudio()
    {

    }

    internal void StopBgAudio()
    {
        bg_adudio.Stop();
    }

    internal void ToggleMute(bool toggle, string type="all")
    {
        switch (type)
        {
            case "bg":
                bg_adudio.mute = toggle;
                bg_audioBonus.mute = toggle;
                break;
            case "button":
                audioPlayer_button.mute=toggle;
                audioSpin_button.mute=toggle;
                break;
            case "wl":
                audioPlayer_wl.mute=toggle;
                coins.mute=toggle;
                break;
            case "all":
                audioPlayer_wl.mute = toggle;
                bg_adudio.mute = toggle;
                audioPlayer_button.mute = toggle;
                audioSpin_button.mute = toggle;
                coins.mute=toggle;
                break;
        }
    }

}
