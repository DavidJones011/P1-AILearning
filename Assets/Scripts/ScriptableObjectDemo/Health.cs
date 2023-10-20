using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    float MAX_HEALTH = 100.0f;

    Camera _cam = null;
    Image _image = null;

    [SerializeField]
    bool _useHealthOnComponent = false;

    float _health = 100.0f;

    [SerializeField]
    SODemo_CharacterData _data = null;

    // Start is called before the first frame update
    void Start()
    {
        _image = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        _cam = Camera.main;

        _health = _data ? _data._maxhealth : MAX_HEALTH;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.GetChild(0).rotation = Quaternion.LookRotation(_cam.transform.position - transform.position);
    }

    public void TakeDamage(float damage)
    {
        float ratio = 0.0f;
        if(_data && !_useHealthOnComponent)
        {
            _data._health -= damage;
            _data._health = Mathf.Max(0, _data._health);
            ratio = _data._health / _data._maxhealth;
        }
        else
        {
            _health -= damage;
            _health = Mathf.Max(0, _health);
            ratio = _health / (_data ? _data._maxhealth : MAX_HEALTH);
        }

        _image.fillAmount = ratio;
    }
}
