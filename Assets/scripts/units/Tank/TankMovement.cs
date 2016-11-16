using UnityEngine;

public class TankMovement : MonoBehaviour {
    public float speed = 12f;                 // How fast the tank moves forward and back.
    public float turnSpeed = 180f;            // How fast the tank turns in degrees per second.
    public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.

    private string movementAxis;          // The name of the input axis for moving forward and back.
    private string turnAxis;              // The name of the input axis for turning.
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private float originalPitch;              // The pitch of the audio source at the start of the scene.


    private void Awake () {
        m_Rigidbody = GetComponent<Rigidbody> ();
    }


    private void OnEnable () {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable () {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
    }


    private void Start () {
        movementAxis = "Vertical";
        turnAxis = "Horizontal";

        originalPitch = m_MovementAudio.pitch;
    }


    private void Update () {
        m_MovementInputValue = Input.GetAxis (movementAxis);
        m_TurnInputValue = Input.GetAxis (turnAxis);

        EngineAudio ();
    }


    private void EngineAudio () {
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f) {
            if (m_MovementAudio.clip == m_EngineDriving) {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(originalPitch - m_PitchRange, originalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        } else if (m_MovementAudio.clip == m_EngineIdling) {
            m_MovementAudio.clip = m_EngineDriving;
            m_MovementAudio.pitch = Random.Range(originalPitch - m_PitchRange, originalPitch + m_PitchRange);
            m_MovementAudio.Play();
        }
    }

    private void FixedUpdate () {
        Move ();
        Turn ();
    }

    private void Move () {
        var movement = transform.forward * m_MovementInputValue * speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void Turn () {
        var turn = m_TurnInputValue * turnSpeed * Time.deltaTime;
        // Make this into a rotation in the y axis.
        var turnRotation = Quaternion.Euler (0f, turn, 0f);
        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
    }
}
