using UnityEngine;
using System.Collections;

public class PlayerSoundControl : MonoBehaviour {  //TODO: landing beim mainMenu wegmachen

    private PlayerControl playerControl;
    private Invoker invoker;
    private float footStepAudioTime;
    private bool wasInAir;
    private bool playLanding = true;
    
    void Start () {
        playerControl = GetComponent<PlayerControl>();
        invoker = GameObject.Find("Player").GetComponent<Invoker>();
    }
	
	void Update () {
        footStepAudioTime += Time.deltaTime;
        if (playerControl.Moving && footStepAudioTime > .5f && !playerControl.IsJumping())
        {
            Globals.AudioSources.footStep.Play();
            footStepAudioTime = 0;
        }

        if (playerControl.IsJumping()) wasInAir = true;
        if (playerControl.IsGrounded && wasInAir && playLanding)
        {
            Globals.AudioSources.landing.Play();
            wasInAir = false;
            playLanding = false;
            invoker.Invoke(.5f, () => playLanding = true);
        }
    }
}
