using UnityEngine;
using Progress;

/// <summary>
/// Специфичные для танка вещи - 
/// движение и вращение клавишами курсора, звук мотора.
/// </summary>
public class TankMovement : MonoBehaviour {
	// Скорость танка.
	public float speed;
	// Скорость вращения танка.
	public float turnSpeed = 180f;

	// Аудио настройки.
	public AudioSource m_MovementAudio;
	public AudioClip m_EngineIdling;
	public AudioClip m_EngineDriving;
	public float m_PitchRange = 0.2f;
	private float originalPitch;

	// Ось движения.
	private string movementAxis;
	// Ось поворота.
	private string turnAxis;
	private Rigidbody m_Rigidbody;
	private float movementInputValue;
	private float turnInputValue;

	// Ссылка на юнит.
	private Unit ownerUnit;

	private void Awake() {
		m_Rigidbody = GetComponent<Rigidbody>();
		ownerUnit = GetComponent<Unit>();
		speed = ownerUnit.Settings.Speed;
	}

	private void OnEnable() {
		m_Rigidbody.isKinematic = false;
		movementInputValue = 0f;
		turnInputValue = 0f;
	}

	private void OnDisable() {
		m_Rigidbody.isKinematic = true;
	}

	private void Start() {
		movementAxis = "Vertical";
		turnAxis = "Horizontal";

		originalPitch = m_MovementAudio.pitch;
	}

	private void Update() {
		movementInputValue = Input.GetAxis(movementAxis);
		turnInputValue = Input.GetAxis(turnAxis);

		EngineAudio();
	}

	/// <summary>
	/// Звук мотора в движении и в простое.
	/// </summary>
	private void EngineAudio() {
		if (Mathf.Abs(movementInputValue) < Constants.Epsilon && Mathf.Abs(turnInputValue) < Constants.Epsilon) {
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

	private void FixedUpdate() {
		Move();
		Turn();
	}

	/// <summary>
	/// Движение танка.
	/// </summary>
	private void Move() {
		var movement = transform.forward * movementInputValue * speed * Time.deltaTime;
		m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
	}

	/// <summary>
	/// Вращение танка.
	/// </summary>
	private void Turn() {
		var turn = turnInputValue * turnSpeed * Time.deltaTime;
		var turnRotation = Quaternion.Euler(0f, turn, 0f);
		m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
	}
}
