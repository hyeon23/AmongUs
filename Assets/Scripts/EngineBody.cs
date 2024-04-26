using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineBody : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> steams = new List<GameObject>();

    private void Start()
    {
        foreach(var steam in steams)
        {
            StartCoroutine(RandomSteam(steam));
        }
    }

    private IEnumerator RandomSteam(GameObject steam)
    {
        while (true)
        {
            float timer = Random.Range(0.5f, 1.5f);
            while(timer >= 0)
            {
                yield return null;
                timer -= Time.deltaTime;
            }
            steam.SetActive(true);
            timer = 0;
            while(timer <= 0.6f)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            steam.SetActive(false);
        }
    }
}
