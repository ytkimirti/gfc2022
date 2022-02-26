using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.Assertions;

public class Fader : MonoBehaviour
{

	public bool fadeAtStart = true;
	public float fadeSpeed;
	public Transform faderLeft;
	public Transform faderRight;

	public static Fader main;

	private void Awake()
	{
		main = this;

		print("hello world");

	}

	void Start()
	{
		if (fadeAtStart)
			FadeOut();
	}

	//In means appear
	public void FadeIn()
	{
		faderRight.gameObject.SetActive(true);
		faderRight.DOKill();
		faderRight.localScale = new Vector3(0, 1, 1);
		faderRight.DOScale(Vector3.one, fadeSpeed).SetUpdate(true);

		faderLeft.gameObject.SetActive(true);
		faderLeft.DOKill();
		faderLeft.localScale = new Vector3(0, 1, 1);
		faderLeft.DOScale(Vector3.one, fadeSpeed).SetUpdate(true);
	}


	//Out means disappear
	public void FadeOut()
	{
		faderRight.gameObject.SetActive(true);
		faderRight.DOKill();
		faderRight.localScale = new Vector3(1, 1, 1);
		faderRight.DOScale(new Vector3(0, 1, 1), fadeSpeed).SetUpdate(true);

		faderLeft.gameObject.SetActive(true);
		faderLeft.DOKill();
		faderLeft.localScale = new Vector3(1, 1, 1);
		faderLeft.DOScale(new Vector3(0, 1, 1), fadeSpeed).SetUpdate(true);
	}

	void Disable()
	{
		faderRight.gameObject.SetActive(false);
		faderLeft.gameObject.SetActive(false);
	}
}
