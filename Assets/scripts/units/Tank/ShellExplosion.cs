using UnityEngine;
using Progress;

public class ShellExplosion : MonoBehaviour {
	public LayerMask m_TankMask;
	// Used to filter what the explosion affects, this should be set to "Players".
	public ParticleSystem m_ExplosionParticles;
	// Reference to the particles that will play on explosion.
	public AudioSource m_ExplosionAudio;
	// Reference to the audio that will play on explosion.
	public float m_MaxDamage = 100f;
	// The amount of damage done if the explosion is centred on a tank.
	public float m_ExplosionForce = 1000f;
	// The amount of force added to a tank at the centre of the explosion.
	public float m_MaxLifeTime = 2f;
	// The time in seconds before the shell is removed.
	public float m_ExplosionRadius = 5f;
	// The maximum distance away from the explosion tanks can be and are still affected.

	public Unit OwnerUnit{ get; set; }

	private void Start() {
		Destroy(gameObject, m_MaxLifeTime);
	}

	private void OnTriggerEnter(Collider other) {
		if (!other.gameObject.CompareTag("Unit")) {
			return;
		}

		Rigidbody targetRigidbody = other.GetComponent<Rigidbody>();
		if (!targetRigidbody) {
			return;
		}

		// Add an explosion force.
		targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

		var unit = targetRigidbody.GetComponent<Progress.Unit>();

		float damage = OwnerUnit.Settings.Attack;
		unit.TakeDamage(OwnerUnit, damage);

		// Unparent the particles from the shell.
		m_ExplosionParticles.transform.parent = null;

		// Play the particle system.
		m_ExplosionParticles.Play();

		// Play the explosion sound effect.
		m_ExplosionAudio.Play();

		// Once the particles have finished, destroy the gameobject they are on.
		Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);

		// Destroy the shell.
		Destroy(gameObject);
	}
}
