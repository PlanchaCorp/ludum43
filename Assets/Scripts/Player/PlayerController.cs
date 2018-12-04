using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float baseSpeed = 50;
    private float speed;
    [SerializeField]
    private float blood = 100;

    [SerializeField]
    private Animator MouvementAnimator;

    private GameObject bloodBar;
    private Rigidbody2D rigidBody;
    private bool isInvisible = false;

	// Use this for initialization
	void Start ()
    {
        speed = baseSpeed;
        bloodBar = GameObject.FindGameObjectWithTag("BloodBar");
        rigidBody = GetComponent<Rigidbody2D>();
        gameObject.AddComponent<PlayerCastBehaviour>();
    }
	
	// Update is called once per frame
	void Update () {
        // Update UI
        UpdateBloodBar();
        // Check end game condition
        if (blood <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    void UpdateBloodBar()
    {
        if (bloodBar != null)
        {
            bloodBar.GetComponent<Image>().fillAmount = blood / 100;
        }
    }

    void FixedUpdate()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        MouvementAnimator.SetFloat("x", inputHorizontal);
        MouvementAnimator.SetFloat("y", inputVertical);

        float diagonalFullLength = Mathf.Sqrt(Mathf.Pow(inputHorizontal, 2) + Mathf.Pow(inputVertical, 2));
        if (diagonalFullLength > 0)
        {
            MouvementAnimator.SetBool("IsWalking",true );
            float moveHorizontal = (1 * inputHorizontal) / diagonalFullLength;
            float moveVertical = (1 * inputVertical) / diagonalFullLength;

            Vector2 movement = new Vector2(moveHorizontal, moveVertical);

            rigidBody.AddForce(movement * speed);
        }
        else
        {
            MouvementAnimator.SetBool("IsWalking", false);
        }
    }

    public void Accelerate(float amount, float timeToWait)
    {
        speed = baseSpeed * amount;
        StartCoroutine(RestoreSpeed(timeToWait));
    }

    IEnumerator RestoreSpeed(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        speed = baseSpeed;
    }

    public bool CanCast(float bloodAmount)
    {
        return blood > bloodAmount;
    }

    public void ReduceBlood(float amount)
    {
        blood -= amount;
    }

    public bool IsInvisible()
    {
        return isInvisible;
    }

    public void Disappear(float timeToWait)
    {
        StartCoroutine(SetInvisible(timeToWait));
    }

    IEnumerator SetInvisible(float timeToWait)
    {
        isInvisible = true;
        yield return new WaitForSeconds(timeToWait);
        GetComponent<SpriteRenderer>().color = Color.white;
        isInvisible = false;
    }
}
