using UnityEngine;
using System.Collections;

public class PlayerSoundControl : MonoBehaviour {

    private PlayerControl playerControl;
    private float footStepAudioTime;
    private bool wasInAir;
    
    void Start () {
        playerControl = GetComponent<PlayerControl>();

    }
	
	void Update () {
        footStepAudioTime += Time.deltaTime;
        if (playerControl.Moving && footStepAudioTime > .5f && !playerControl.IsJumping())
        {
            Globals.AudioSources.footStep.Play();
            footStepAudioTime = 0;
        }

        if (playerControl.IsJumping()) wasInAir = true;
        if (playerControl.IsGrounded && wasInAir)
        {
            Globals.AudioSources.landing.Play();
            wasInAir = false;
        }
    }
}
