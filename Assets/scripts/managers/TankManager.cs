using System;
using UnityEngine;

[Serializable]
public class TankManager {
    public Color m_PlayerColor;                             // This is the color this tank will be tinted.
    public Transform m_SpawnPoint;                          // The position and direction the tank will have when it spawns.
    [HideInInspector] public string m_ColoredPlayerText;    // A string that represents the player with their number colored to match their tank.
    [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.
    [HideInInspector] public int m_Wins;                    // The number of wins this player has so far.

    private TankMovement m_Movement;                        // Reference to tank's movement script, used to disable and enable control.
    private TankShooting m_Shooting;                        // Reference to tank's shooting script, used to disable and enable control.
    private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.

    public void Setup () {
        m_Movement = m_Instance.GetComponent<TankMovement> ();
        m_Shooting = m_Instance.GetComponent<TankShooting> ();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas> ().gameObject;

        var renderers = m_Instance.GetComponentsInChildren<MeshRenderer> ();

        foreach (var t in renderers) {
            t.material.color = m_PlayerColor;
        }
    }

    public void DisableControl () {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;
        m_CanvasGameObject.SetActive (false);
    }

    public void EnableControl () {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;
        m_CanvasGameObject.SetActive (true);
    }

    public void Reset () {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;
        m_Instance.SetActive (false);
        m_Instance.SetActive (true);
    }
}
