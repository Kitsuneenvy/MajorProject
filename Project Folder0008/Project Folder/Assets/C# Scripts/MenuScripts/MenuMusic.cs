using UnityEngine;
using System.Collections;

public class MenuMusic : MonoBehaviour {
	
	public GameObject checkbox;
	public GameObject slider;
	AudioSource source;
	public AudioClip musicToPlay;
	bool muted = false;
	UICheckbox UICheckboxObject;
	UISlider UISliderObject;
	// Use this for initialization
	void Start () {
		UICheckboxObject = checkbox.GetComponent<UICheckbox>();
		UISliderObject = slider.GetComponent<UISlider>();
		source = this.gameObject.GetComponent<AudioSource>();
		source.clip = musicToPlay;
		source.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!UICheckboxObject.isChecked)
		{
			muted = true;
			source.mute = true;
		}
		else if(UICheckboxObject.isChecked && muted)
		{
			muted = false;
			source.mute = false;
		}
		
		
	}
	
	void OnSliderChange()
	{
		source.volume = UISliderObject.sliderValue;
	}
}
