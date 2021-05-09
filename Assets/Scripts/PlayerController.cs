using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 350;
    public event EventHandler<MushroomPickupEventArgs> OnPlayerMushroomPickup;
    public List<AudioClip> walkSounds;
    public List<AudioClip> runSounds;
    private AudioSource audioSource;

    public void SetAudioSource(AudioSource audioSource)
    {
        this.audioSource = audioSource;
        StartCoroutine(DoWalkSound());
        StartCoroutine(DoRunSound());
    }

    public Player Player
    {
        get
        {
            if(_player == null)
            {
                _player = GetComponent<Player>();
            }
            return _player;
        }
    }
    private Player _player;

    public Animator Animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }
    private Animator _animator;

    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
    }

    private void FixedUpdate()
    {

        float verticalInput = Input.GetAxis("Vertical");
        float speed = Player.GetSpeed();

        // Animation Stuff
        if (verticalInput > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            isWalking = false;
            Animator.SetBool("isWalking", true);
            Animator.SetBool("isRunning", true);
            speed *= 3f;
        }
        else if (verticalInput > 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            isWalking = true;
            isRunning = false;
            Animator.SetBool("isRunning", false);
            Animator.SetBool("isWalking", true);
        }
        else
        {
            isRunning = false;
            isWalking = false;
            Animator.SetBool("isRunning", false);
            Animator.SetBool("isWalking", false);
        }

        if(verticalInput == 0 && Input.GetKey(KeyCode.Space))
        {
            Animator.SetBool("isStanding", true);
        }
        if (verticalInput == 0 && !Input.GetKey(KeyCode.Space))
        {
            Animator.SetBool("isStanding", false);
        }

        Vector3 forwardAmount = Vector3.forward * Time.deltaTime * speed * verticalInput;
        transform.Translate(forwardAmount);

        RotateToGround();
    }

    public void RotateToFace(Vector3 position)
    {
        transform.forward = position;
        RotateToGround();
    }

    private void RotateToGround()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, .05f, -(transform.up), out hit, 100f))
        {
            //Quaternion q = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal)), .1f);
            //transform.rotation = q;

            transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal));
        }
    }

    private bool isWalking = false;
    private bool isRunning = false;

    private IEnumerator DoWalkSound()
    {
        int walkSoundIndex = 0;

        while (true)
        {
            if (isWalking)
            {
                walkSoundIndex = (walkSoundIndex + 1) % walkSounds.Count();
                audioSource.PlayOneShot(walkSounds[walkSoundIndex], .3f * audioSource.volume);
                yield return new WaitForSeconds(walkSounds[walkSoundIndex].length);
            }
            else
            {
                yield return new WaitForSeconds(.3f);
            }
        }
    }

    private IEnumerator DoRunSound()
    {
        int runSoundIndex = 0;

        while (true)
        {
            if (isRunning)
            {
                runSoundIndex = (runSoundIndex + 1) % runSounds.Count();
                audioSource.PlayOneShot(runSounds[runSoundIndex], .37f * audioSource.volume);
                yield return new WaitForSeconds(runSounds[runSoundIndex].length / .595f);
                audioSource.Stop();
            }
            else
            {
                yield return new WaitForSeconds(.2f);
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Mushroom mushroom))
        {
            OnPlayerMushroomPickup?.Invoke(this, new MushroomPickupEventArgs()
            {
                PickedUpMushroom = mushroom,
                Player = Player
            });
        }
    }
}
