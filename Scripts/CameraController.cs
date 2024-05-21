using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float animSpeed = 0.25f;

    private Animator _animator;
    private float timer;
    private bool _isPlaying;
    private bool startGame;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isPlaying && startGame)
        {
            timer -= Time.deltaTime * animSpeed;
            if (timer < 0f)
            {
                timer = 0f;
                _isPlaying = false;
            }
            _animator.SetLayerWeight(1, timer);
        }
        else if (_isPlaying && !startGame)
        {
            timer += Time.deltaTime * animSpeed;
            if (timer > 1f)
            {
                timer = 1f;
                _isPlaying = false;
            }
            _animator.SetLayerWeight(1, timer);
        }
    }

    public void StartGame()
    {
        if (!_isPlaying)
        {
            startGame = !startGame;
            _animator.SetBool("startGame", startGame);
            timer = startGame ? 1f : 0f;
            _isPlaying = true;

            if (!startGame)
            {
                _animator.Play("SlowMotionCamera", 1, 0f); // Restart Anim SlowMotionCamera
            }
        }
    }
}
