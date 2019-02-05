using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScript : MonoBehaviour
{
    Image img;
	public Color blackColor;
	public Color clearColor;
	public bool IsTransitioning 
	{
		get
		{
			return isTransitioning;
		}
		set
		{
			isTransitioning = value;
		}
	}
	private bool isTransitioning;

	void Awake()
	{
		img = GetComponent<Image>();
	}

    public IEnumerator FadeImg(float duration = 1f, bool fadeIn = true)
	{
		isTransitioning = true;
		float lerp = 0f;
		if(fadeIn)
		{		
			img.color = blackColor;
			while(lerp <= 1f)
			{
				img.color = Color.Lerp(blackColor, clearColor, lerp);
				lerp += Time.deltaTime / duration;
				yield return null;
			}
		img.color = clearColor;
		gameObject.SetActive(false);
		}
		else
		{
			gameObject.SetActive(true);
			img.color = clearColor;
			while(lerp <= 1f)
			{
				img.color = Color.Lerp(clearColor, blackColor, lerp);
				lerp += Time.deltaTime / duration;
				yield return null;
			}
		img.color = blackColor;
		}
		isTransitioning = false;
	}
}
