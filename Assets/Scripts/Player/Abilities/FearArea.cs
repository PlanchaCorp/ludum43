using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearArea : MonoBehaviour
{
    private float FEARREACH = 3.0f;
    private float FEARDURATION = 2.0f;
    private float AREADURATION = 3.0f;
    private List<GameObject> ennemies;
    private GameObject fireAura;
    private SpriteRenderer auraRenderer;
    private bool animationOpacityIncreasing = false;
    
    void Start ()
    {
        ennemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ennemy"));
        fireAura = Instantiate<GameObject>(new GameObject("FireAura"), transform.position, Quaternion.identity);
        auraRenderer = fireAura.AddComponent<SpriteRenderer>();
        auraRenderer.sprite = Resources.Load<Sprite>("Sprites/Fear");
        auraRenderer.color = new Color(1, 0.6f, 0, 0.5f);
        auraRenderer.sortingOrder = -1;
        auraRenderer.transform.localScale = new Vector2(FEARREACH * 5.5f, FEARREACH * 5.5f);
        StartCoroutine(Dissip(AREADURATION));
    }
	
	void Update () {
        fireAura.transform.position = gameObject.transform.position;
        // Animate opacity
        if (auraRenderer.color.a > 0.75 || auraRenderer.color.a < 0.25)
        {
            animationOpacityIncreasing = !animationOpacityIncreasing;
        }
        auraRenderer.color = new Color(auraRenderer.color.r, auraRenderer.color.g, auraRenderer.color.b, animationOpacityIncreasing ? auraRenderer.color.a + Time.deltaTime : auraRenderer.color.a - Time.deltaTime);
        // Fear each ennemies within reach
        foreach (GameObject ennemy in ennemies)
        {
            if (Vector2.Distance(transform.position, ennemy.transform.position) <= FEARREACH)
            {
                StartCoroutine(ennemy.GetComponentInChildren<Sight>().Scare(FEARDURATION));
            }
        }
    }

    private IEnumerator Dissip(float timeBeforeDissipation)
    {
        yield return new WaitForSeconds(AREADURATION);
        Destroy(fireAura);
        Destroy(this);
    }
}
