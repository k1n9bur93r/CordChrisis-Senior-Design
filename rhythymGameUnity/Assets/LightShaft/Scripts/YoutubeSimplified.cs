using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class YoutubeSimplified : MonoBehaviour
{
	public YoutubePlayer player;
	public Track meta;

	[Header("Used by SiteHandler - LEAVE THIS BLANK")]
	public string url;
	public bool autoPlay = true;
	public bool fullscreen = true;
	private VideoPlayer videoPlayer;

	private void Awake()
	{
		videoPlayer = GetComponentInChildren<VideoPlayer>();
		player = GetComponentInChildren<YoutubePlayer>();
		player.videoPlayer = videoPlayer;
	}

	private void Start()
	{
		url = meta.json.video;
	}

	public void Play()
	{
		if (fullscreen)
		{
			videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
		}
		
		player.autoPlayOnStart = autoPlay;
		player.videoQuality = YoutubePlayer.YoutubeVideoQuality.STANDARD;

		player.Play(url);
		
		/*
		if (autoPlay)
		{
			player.Play(url);
		}
		*/
	}
}
