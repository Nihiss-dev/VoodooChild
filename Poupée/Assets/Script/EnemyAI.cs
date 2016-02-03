using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {


    public GameObject _target;
    public float _range = 1f;
    public LayerMask _TargetMask;
    public float _delay = 0.55f;
    public List<AudioClip> _list;
    public float violence = 300f;

    private AudioSource src;

    private enum IAState
    {
        INACTIVE = 0,
        WALKING,
        ATTACKING,
    }

    private IAState _state;
    private bool _isActivated = false;
    private NavMeshAgent _agent;
    private float _nextAttack = 0;
    private float nextActive = 0;
    private Animator _anim;
    

    // Use this for initialization
	void Start () {
        _agent = GetComponent<NavMeshAgent>();
        src = GetComponent<AudioSource>();
        _state = IAState.INACTIVE;
        _anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate()
    {
        if (_isActivated)
        {
             if (_state == IAState.WALKING)
             {
             	_anim.SetBool("isActive", true);
				 transform.GetChild(2).GetChild(0).GetChild(7).GetChild(0).gameObject.SetActive(true);
                 _agent.destination = _target.transform.position;
                 Collider[] entittiesAround = Physics.OverlapSphere(transform.position, _range, _TargetMask);
                 foreach (Collider entity in entittiesAround)
                 {
                     _state = IAState.ATTACKING;
					_anim.SetBool("Attack", true);					
                 }
             }
            if (_state == IAState.ATTACKING)
            {	
				transform.GetChild(2).GetChild(0).GetChild(7).GetChild(0).gameObject.SetActive(true);
                _nextAttack += Time.deltaTime;
				
                if (_nextAttack > _delay) // Delay of attack Animation
                {
                    Collider[] entittiesAround = Physics.OverlapSphere(transform.position, _range, _TargetMask);
                    foreach (Collider entity in entittiesAround)
                    {
                        _target.GetComponent<PlayerAttack>().isAttacked(transform.position);
                    }
                    _nextAttack = 0;
					_anim.SetBool("Attack", false);
                    _state = IAState.WALKING;
                    _agent.Resume();
                }
                else
                    _agent.Stop();
            }
            if (_state == IAState.INACTIVE && _isActivated)
            {
				transform.GetChild(2).GetChild(0).GetChild(7).GetChild(0).gameObject.SetActive(false);           
            	_nextAttack += Time.deltaTime;
            	if (_nextAttack > 10f)
            	{
            		_state = IAState.WALKING;
					_agent.Resume();
					transform.GetChild(2).GetChild(0).GetChild(7).GetChild(0).gameObject.SetActive(true);
					_nextAttack = 0f;
            	}
            }
        }
        else
        {
            _agent.Stop();
			transform.GetChild(2).GetChild(0).GetChild(7).GetChild(0).gameObject.SetActive(false);           
			
        }
    }

    public void Activate(bool state)
    {
    	
        _isActivated = state;
        if (state)
        {
			_agent.Resume();
            _state = IAState.WALKING;
        }
        else
            _state = IAState.INACTIVE;
    }
    
    public void TakeDamage(Vector3 enemypos)
    {
		float tmpX, tmpZ;
		
		tmpX = transform.position.x - enemypos.x;
		tmpZ = transform.position.z - enemypos.z;
		GetComponent<Rigidbody>().AddForce(new Vector3(tmpX * violence, 0, tmpZ * violence));
		System.Random rnd = new System.Random();
		int angle = rnd.Next(180, 360);
		transform.Rotate(new Vector3(0, angle, 0) * 30 * Time.deltaTime);
		_state = IAState.INACTIVE;
		_anim.SetBool("isActive", false);
    }
}
