using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
	
	AudioSource ambience1;
	AudioSource ambience2;
	AudioSource soundEffects;
	
	//background sounds
	public AudioClip ambienceClip1; //birds cheeping
	public AudioClip ambienceClip2; //slight breeze
	
	//character sounds
	public AudioClip fart1;
	public AudioClip fart2;
	public AudioClip fart3;
	public AudioClip chefDodge;
	
	//attack sounds
	public AudioClip Clang;
	public AudioClip Clash;
	public AudioClip Ting;
	
	//heal sounds
	public AudioClip Detergent;
	public AudioClip WateringCan;
	
	
	//List of the sounds index 0 = attack, 1 = idle, 2 = dodge, 3 = death, 4 = damage, 5 = special (idle 2 or attack miss)
	public List<AudioClip> chefAudio = new List<AudioClip>();
	public List<AudioClip> frierAudio = new List<AudioClip>();
	public List<AudioClip> ladlewightAudio = new List<AudioClip>();
	public List<AudioClip> bowlderAudio = new List<AudioClip>();
	public List<AudioClip> mowerAudio = new List<AudioClip>();
	public List<AudioClip> floristAudio = new List<AudioClip>();
	public List<AudioClip> prunerAudio = new List<AudioClip>();
	public List<AudioClip> potterAudio = new List<AudioClip>();
	
	// Use this for initialization
	void Start () {
		ambience1 = this.gameObject.AddComponent<AudioSource>();
		ambience2 = this.gameObject.AddComponent<AudioSource>();
		ambience1.clip = ambienceClip1; //set first ambience clip
		ambience1.loop = true;
		ambience2.loop = true;
		ambience1.Play(); //play the first ambience clip
		ambience2.clip = ambienceClip2;
		ambience2.Play();
		
//		ambience2 = GameObject.Find("Ambience2").GetComponent<AudioSource>();
//		ambience2.clip = ambienceClip2;
//		ambience2.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public AudioClip returnClang()
	{
		return Clang;
	}
	
	public AudioClip returnClash()
	{
		return Clash;
	}
	
	public AudioClip returnTing()
	{
		return Ting;
	}
	public AudioClip returnDetergent()
	{
		return Detergent;
	}
	
	public AudioClip returnRandomFart()
	{
		int soundToPlay = Random.Range(1,3);
		if(soundToPlay == 1)
		{
			return fart1;
		}
		else if(soundToPlay == 2)
		{
			return fart2;
		}
		else
		{
			return fart3;
		}
	}
	
	public AudioClip returnChefDodge()
	{
		return chefDodge;
	}
}
