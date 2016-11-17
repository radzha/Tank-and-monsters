using UnityEngine;
using Progress;

/// <summary>
/// Класс, отвечающий за взрыв и урон от снаряда.
/// </summary>
public class ShellExplosion : MonoBehaviour {
	public LayerMask m_TankMask;
	public ParticleSystem m_ExplosionParticles;
	public AudioSource m_ExplosionAudio;
	public float m_MaxDamage = 100f;
	public float m_ExplosionForce = 1000f;
	public float m_MaxLifeTime = 2f;
	public float m_ExplosionRadius = 5f;

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

		targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

		var unit = targetRigidbody.GetComponent<Progress.Unit>();

		float damage = OwnerUnit.Settings.Attack;
		unit.TakeDamage(OwnerUnit, damage);

		m_ExplosionParticles.transform.parent = null;
		m_ExplosionParticles.Play();
		m_ExplosionAudio.Play();

		Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);
		Destroy(gameObject);
	}
}
