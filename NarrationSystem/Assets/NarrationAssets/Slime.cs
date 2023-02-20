using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Subject
{
    Rigidbody _rb;
    [SerializeField] int _health = 100;
    [SerializeField] int _knockbackForce = 18;
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _acceptableDistToTarget = 2f;
    [SerializeField] Transform _moveArea;
    private Vector3 _targetPos;
    private Renderer _rend;
    private Coroutine _blinkCoroutine;
    [SerializeField] float _blinkDuration = 0.2f;
    [SerializeField] int _blinkCount = 5;
    bool _isAttackable = true;

    public bool IsAttackable { get { return _isAttackable; }}

    void Start()
    {
        _rend = GetComponentInChildren<Renderer>();
        _moveArea = transform;
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Move();
    }

    public void TakeDamage(int damage, Vector3 directionOfAttack)
    {
        if (!_isAttackable) {
            return;
        }

        _isAttackable = false;
        _health -= damage;
        if (_health <= 0)
        {
            Knockback(directionOfAttack);
            Die();
        }
        else
        {
            Knockback(directionOfAttack);
            _blinkCoroutine = StartCoroutine(Blink());
        }
    }

    void Knockback(Vector3 directionOfAttack)
    {
        _rb.AddForce(directionOfAttack * _knockbackForce, ForceMode.Impulse);
    }

    void Die()
    {
        NotifyObservers(PlayerActions.Die);
        _blinkCoroutine = StartCoroutine(Blink(true));
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, _moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _targetPos) < _acceptableDistToTarget)
        {
            _targetPos = GetRandomPos();
        }
    }

    Vector3 GetRandomPos()
    {
        Vector3 randomPos = _moveArea.position + new Vector3(Random.Range(-_moveArea.localScale.x / 2, _moveArea.localScale.x / 2), 0, Random.Range(-_moveArea.localScale.z / 2, _moveArea.localScale.z / 2));
        return randomPos;
    }

    IEnumerator Blink(bool die = false)
    {
        for (int i = 0; i < _blinkCount; i++)
        {
            _rend.enabled = false;
            yield return new WaitForSeconds(_blinkDuration);
            _rend.enabled = true;
            yield return new WaitForSeconds(_blinkDuration);
        }

        if (die) {
            Destroy(gameObject);
        } else {
            _isAttackable = true;
        }
    }

}
