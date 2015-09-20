using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPrototype))]
public abstract class AbstractPlayerBehaviour : MonoBehaviour
{
	public float accelerometerUpdateInterval = 1f / 60f;
	public float lowPassKernelWidthInSeconds = 0.2f;

	protected float _lowPassFilterFactor;
	protected float _lowPassValue = 0;

	protected PlayerPrototype _playerPrototype;

	void Start ()
	{
		_playerPrototype = GetComponent<PlayerPrototype> ();
		_lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
	}

	public abstract void GroundedBehaviour ();
	public abstract void InAirBehaviour ();
	public abstract void FallingBehaivour ();

}
