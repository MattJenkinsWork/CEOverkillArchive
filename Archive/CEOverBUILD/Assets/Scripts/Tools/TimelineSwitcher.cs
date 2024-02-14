using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineSwitcher : MonoBehaviour {

    PlayableDirector director;
    public TimelineAsset loopTimeline;
    public TimelineAsset startTimeline;

    private double timeKeeper;

    // Use this for initialization
    void Start () {
        director = GetComponent<PlayableDirector>();
	}
	
	// Update is called once per frame
	void Update () {

        if (director.playableAsset == startTimeline && (timeKeeper+Time.deltaTime) >= director.duration)
        {
            director.playableAsset = loopTimeline;
            director.Play();
            
        }

        if (director.time > 0)
        {
            timeKeeper = director.time;
        }
    }
}
