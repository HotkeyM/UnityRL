using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Phrase
{
	//public Phrase(){}
	public string text = "Test test test test";
	public Sprite image1 = null;
	public Sprite image2 = null;

	public bool jumping = true;
}

public class DialogScript : MonoBehaviour {



	public Image image1;
	public Image image2;
	public Text text;


	bool skip = false;


	public Phrase[] phrases;
	// Use this for initialization
	void Start () {
	
		//phrases = new List<Phrase> ();
		/*
		foreach (var v in phrases) 
		{
			image1.sprite = v.image1;
			if (image2 && v.image1)
				image2.sprite = v.image2;
			StartCoroutine(PrintText(v.text, v.jumping));

		}
		*/

		StartCoroutine (PrintText (false));
	}



	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonDown (0))
			skip = true;
	}


	IEnumerator PrintText(bool jump1 = false)
	{
		int index;


		foreach (var v in phrases)
		{
			if (image1 && v.image1) {
				image1.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
				image1.sprite = v.image1;
			} else {

				if (image1)
					image1.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
					}
			if (image2 && v.image2) {
				image2.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
				image2.sprite = v.image2;
			} else {
				if (image2)
					image2.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
			}
			//StartCoroutine (PrintText (v.text, v.jumping));
			string text = v.text;
		
			index = 0;
			while (text.Length > index) {
				this.text.text = text.Substring (0, index);
				index = index + 3;
				if (skip) 
				{
					skip = false;
					continue;
				}
				yield return new WaitForSeconds (0.1f);
			}

			while (true) {
				this.text.text = text + "_";
				if (skip) 
				{
					skip = false;
					break;
				}
				yield return new WaitForSeconds (0.1f);
				this.text.text = this.text.text.Substring (0, text.Length);
				if (skip) 
				{
					skip = false;
					break;
				}
				yield return new WaitForSeconds (0.1f);
			}
		}

		Destroy (gameObject);

	}
}
