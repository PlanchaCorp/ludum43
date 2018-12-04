using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnnemyScript : MonoBehaviour {


    private float baseSpeed;
    private float baseViewDistance;

    [FormerlySerializedAs("Jalons")]
    [SerializeField]
    private List<GameObject> _jalons = new List<GameObject>();


    [FormerlySerializedAs("Speed")]
    [SerializeField]
    private float _speed = 10;


    [FormerlySerializedAs("View distance")]
    [SerializeField]
    private float _viewDistance = 5f;


    [FormerlySerializedAs("View range")]
    [SerializeField]
    private int _viewAngle = 45;

    public bool canPatrol
    {
        get { return _jalons.Count > 1; }
    }



    private void Start()
    {
        baseSpeed = _speed;
        baseViewDistance = _viewDistance;
    }


    public float speed
    {
        get { return _speed; }
    }

    public float viewDistance {
        get { return _viewDistance; }
    }
    public float viewAngle
    {
        get { return _viewAngle; }
    }


    public List<GameObject> jalons
    {
        get { return _jalons; }
    }

    public void Blind(float duration)
    {
        StartCoroutine(DisableVue(duration));
    }

    public void Root(float duration) {
        StartCoroutine(DisableMovement(duration));
    }
    public void Stun(float duration)
    {
        StartCoroutine(DisableVue(duration));
        StartCoroutine(DisableMovement(duration));
    }
    IEnumerator DisableVue(float duration)
    {
        _viewDistance = 0;
        yield return new WaitForSeconds(duration);
        _viewDistance = baseViewDistance;
    }
    IEnumerator DisableMovement(float duration)
    {
        _speed = 0;
        yield return new WaitForSeconds(duration);
        _speed = baseSpeed;
    }

}
