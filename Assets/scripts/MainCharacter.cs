using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    private Vector3 _moveVector;
    private float _gravityForce;
    private int hashOfForward;

    private CharacterController _characterController;
    private Animator _animator;
    private bool _isWalking;

    public float SpeedMove;
    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        hashOfForward = Animator.StringToHash("moving_forward");
    }

    void Update()
    {
        CharacterMove();
        GamingGravity();
    }

    private void CharacterMove()
    {
        _moveVector = Vector3.zero;
        
        if(_characterController.isGrounded)
        {
            _moveVector.x = Input.GetAxis("Horizontal") * SpeedMove;
            _moveVector.z = Input.GetAxis("Vertical") * SpeedMove;

            if ((_moveVector.x != 0 
                || _moveVector.z != 0)
                && !_isWalking)
            {
                _animator.SetBool(hashOfForward, true);
                _isWalking = true;
            }
            else if(_isWalking)
            {
                _animator.SetBool(hashOfForward, false);
                _isWalking = false;
            }

            if(Vector3.Angle(Vector3.forward, _moveVector) > 1f 
                || Vector3.Angle(Vector3.forward, _moveVector) == 0)
            {
                Vector3 direct = Vector3.RotateTowards(transform.forward, _moveVector, SpeedMove, 0.0f);
                transform.rotation = Quaternion.LookRotation(direct);
            }
        }

        _moveVector.y = _gravityForce;
        _characterController.Move(_moveVector * Time.deltaTime);
    }

    private void GamingGravity()
    {
        if(!_characterController.isGrounded)
            _gravityForce -= 20f * Time.deltaTime;
        else
            _gravityForce = -1f;
    }
}
