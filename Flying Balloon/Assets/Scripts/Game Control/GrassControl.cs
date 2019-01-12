using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassControl : MonoBehaviour {

    [System.Serializable]
    public struct Entry { public string groupName; public float randomVariation; };

    public Entry[] groupNames = { };

	// Use this for initialization
	void Awake () {
		foreach (Entry group in groupNames)
        {
            setTime(group.groupName + " 1", group.randomVariation);
            setTime(group.groupName + " 2", group.randomVariation);
        }
	}

    void setTime(string groupName, float randomVariation)
    {
        GameObject[] grassGroup = GameObject.FindGameObjectsWithTag(groupName);
        List<GameObject> sortedGroup = new List<GameObject>(grassGroup);
        sortedGroup.Sort((x, y) => y.transform.position.x.CompareTo(x.transform.position.x));

        int groupSize = sortedGroup.Count;
        float change = 1.0f / groupSize;

        int count = 0;

        foreach (GameObject grass in sortedGroup)
        {
            AnimationStart grassScript = grass.GetComponent<AnimationStart>();
            grassScript.startTime = Mathf.Repeat(change * count + Random.Range(-change * randomVariation, change * randomVariation), 1);

            count++;
        }
    }

}
