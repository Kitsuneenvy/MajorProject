using UnityEngine;
using System.Collections;

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
