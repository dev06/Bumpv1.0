using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class G_UserInterface : MonoBehaviour {

	[HideInInspector]
	public List<Canvas> ChildCanvases = new List<Canvas>();

	void Awake () {
		Init();
	}

	void Init()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).gameObject.tag.Contains("Canvas"))
			{
				ChildCanvases.Add(transform.GetChild(i).GetComponent<Canvas>());
			}
		}

		EnableCanvases(); 
	}

	void EnableCanvases()
	{
		foreach(Canvas c in ChildCanvases)
		{
			c.gameObject.SetActive(true); 
			c.enabled = true; 
		}
	}
}
