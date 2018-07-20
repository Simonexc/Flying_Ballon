using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAlpha : MonoBehaviour {

	public float alpha = 0f;

	private Renderer objectRenderer;

	void Start () {
		objectRenderer = GetComponent<Renderer> ();
		Color color = objectRenderer.material.color;
		color.a = alpha;
		objectRenderer.material.color = color;
	}

}
