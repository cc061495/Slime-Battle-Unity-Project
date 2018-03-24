/* Copyright (c) cc061495 */
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public Sound[] sounds;

	public static AudioManager instance;
	private string prevTheme;

	// Use this for initialization
	void Awake () {

		if(instance == null)
			instance = this;
		else{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		foreach(Sound s in sounds){
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.playOnAwake = s.playOnAwake;
			s.source.loop = s.loop;
		}
	}

	void Start(){
		PlayTheme("Home");
	}

	public void ChangeTheme(string currMusic){
		if(prevTheme != currMusic){
			Stop(prevTheme);
			PlayTheme(currMusic);
		}
	}
	
	private void PlayTheme (string name){
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null){
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.Play();
		prevTheme = name;
	}

	public void Play (string name){
		
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null){
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.Play();
	}

	public void Stop (string name){
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null){
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.Stop();
	}
}
