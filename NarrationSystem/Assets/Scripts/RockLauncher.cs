using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLauncher : MonoBehaviour
{
    [SerializeField] GameObject _rockPrefab; 
    [SerializeField] GameObject _target;

    [SerializeField] float _minForce = 18.0f;
    [SerializeField] float _maxForce = 20.0f;
    
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject rock = Instantiate(_rockPrefab, this.transform.position, this.transform.rotation, null);
            Rigidbody rockRb = rock.GetComponent<Rigidbody>();

            float randomValue = Random.Range(_minForce, _maxForce);

            Vector3 forceDirection = (_target.transform.position - rockRb.position) * randomValue;
            rockRb.AddForce(forceDirection, ForceMode.Impulse);
        }
    }
}
