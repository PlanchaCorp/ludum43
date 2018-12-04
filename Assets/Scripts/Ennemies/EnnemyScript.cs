using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Pathfinding;

public class EnnemyScript : MonoBehaviour
{


    private float baseSpeed;
    private float baseViewDistance;

    [FormerlySerializedAs("Jalons")]
    [SerializeField]
    private List<GameObject> _jalons = new List<GameObject>();

    private Vector3 _startPosition;

    private float _startRotation;

    public float startRotation
    {
        get { return _startRotation; }
    }

    public Vector3 startPostition
    {
        get { return _startPosition; }
    }

    public GameObject interogationMark;
    public GameObject ExclamationMark;
    public GameObject FearMark;

    public GameObject overHead;


    [FormerlySerializedAs("Speed")]
    [SerializeField]
    private float _speed = 10;

    [FormerlySerializedAs("Damage")]
    [SerializeField]
    private float _damage = 40;


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

    public float damage
    {
        get { return _damage; }
    }



    private void Start()
    {
        _startRotation = transform.Find("Vision").transform.rotation.z;
        _startPosition = transform.position;
        baseSpeed = _speed;
        baseViewDistance = _viewDistance;
    }


    public float speed
    {
        get { return _speed; }
    }

    public float viewDistance
    {
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

    public void Root(float duration)
    {
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
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        _speed = 0;
        yield return new WaitForSeconds(duration);
        rigidbody.constraints = RigidbodyConstraints2D.None;
        _speed = baseSpeed;
    }

    public IEnumerator TeleportToSpawn()
    {
        // Now you're dealing with portals !
        Object portal1 = Instantiate(Resources.Load("Prefabs/Portal"), transform.position, Quaternion.identity);
        Object portal2 = Instantiate(Resources.Load("Prefabs/Portal"), _startPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.4f);
        // Teleporting ...
        transform.position = _startPosition;
        transform.Find("Vision").transform.rotation = Quaternion.AngleAxis(_startRotation, Vector2.right);
        yield return new WaitForSeconds(0.4f);
        // Destroying portals
        Destroy(portal1);
        Destroy(portal2);
    }

    public void CanMove(bool canMove)
    {
        gameObject.GetComponent<AIPath>().canMove = canMove;
    }

    public void SetTarget(Transform target)
    {
        gameObject.GetComponent<AIDestinationSetter>().target = target;
    }

    public void Update()
    {
        float angle = transform.Find("Vision").eulerAngles.z;
        transform.Find("Gfs").GetComponent<Animator>().SetFloat("Angle",angle);
    }

    public void Attack()
    {
        transform.GetComponent<AudioSource>().Play();
        transform.Find("Gfs").GetComponent<Animator>().Play("Cultist_Attack");
    }

    public void Exclamation()
    {
        Instantiate(ExclamationMark, overHead.transform.position, overHead.transform.rotation,transform);
    }

    public void Interogation()
    {
        Instantiate(interogationMark, overHead.transform.position + new  Vector3(0, 0.5f,0), overHead.transform.rotation,transform);
    }

    public void Fear()
    {
        Instantiate(FearMark, overHead.transform.position + new Vector3(0, 0.5f, 0), overHead.transform.rotation, transform);
    }
}
