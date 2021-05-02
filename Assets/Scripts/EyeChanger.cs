using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeChanger : MonoBehaviour
{

    SpriteRenderer sr;

    public List<Sprite> neutralEyes;
    public List<Sprite> madEyes;
    public List<Sprite> sadEyes;
    public List<Sprite> stressedEyes;
    public List<Sprite> thinkingEyes;

    private float counter;

    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ChangeEyes(int typeID)
    {
        if (typeID == 0)
        {
            sr.sprite = neutralEyes[(int)Random.Range(0, neutralEyes.Count - 1)];
        }
        else if (typeID == 1)
        {
            sr.sprite = sadEyes[(int)Random.Range(0, sadEyes.Count - 1)];
        }
        else if (typeID == 2)
        {
            sr.sprite = madEyes[(int)Random.Range(0, madEyes.Count - 1)];
        }
        else if (typeID == 3)
        {
            sr.sprite = stressedEyes[(int)Random.Range(0, stressedEyes.Count - 1)];
        }
        else if (typeID == 4)
        {
            sr.sprite = thinkingEyes[(int)Random.Range(0, thinkingEyes.Count - 1)];
        }
    }

    public void ChangeEyes(string type)
    {
        if (type == "neutral")
        {
            ChangeEyes(0);
        } else if (type == "sad")
        {
            ChangeEyes(1);
        }
        else if (type == "mad")
        {
            ChangeEyes(2);
        }
        else if (type == "stressed")
        {
            ChangeEyes(3);
        }
        else if (type == "thinking")
        {
            ChangeEyes(4);
        }
    }

    public void Update()
    {
        if (counter < 0f)
        {
            counter = Random.Range(2f, 6f);
            ChangeEyes((int)Random.Range(0, 4));
        }

        counter -= Time.deltaTime;

    }
}
