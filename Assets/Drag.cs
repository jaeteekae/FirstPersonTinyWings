using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class Drag : MonoBehaviour {
	public AudioSource clip;

	//********MYO**********
	// Myo game object to connect with.
	// This object must have a ThalmicMyo script attached.
	public GameObject myo = null;
	
	// Materials to change to when poses are made.
	public Material waveInMaterial;
	public Material waveOutMaterial;
	public Material doubleTapMaterial;
	
	// The pose from the last update. This is used to determine if the pose has changed
	// so that actions are only performed upon making them rather than every frame during
	// which they are active.
	private Pose _lastPose = Pose.Unknown;
	
	// Default drag level for the player when they are flying normally
	private int defaultDrag = 10;

	//******JULIA'S NONSENSE*********
	public Rigidbody rb;
	public ConstantForce cf;
	public Vector3 lastPos = new Vector3(0, 0);
	public Vector3 llastPos = new Vector3(0, 0);
	public bool goingUp;
	public bool spacedown;
	public bool flying;
	public bool checkGrounded;
	protected CharacterController Controller = null;


	void Awake() {
		Controller = gameObject.GetComponent<CharacterController> ();
	}
	
	// Use this for initialization
	void Start () {
		flying = false;
		goingUp = false;
		spacedown = false;
		checkGrounded = false;
	}
	
	// Update is called once per frame
	void Update () {
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		rb = GetComponent<Rigidbody> ();
		cf = GetComponent<ConstantForce> ();
		Vector3 newv = cf.force;
		float newaf = rb.angularDrag;
		
		if (lastPos.y < transform.position.y) {
			goingUp = true;
		}

		if (goingUp) {
			newaf += 0.3f;
			rb.angularDrag = newaf;
		} else {
			if (rb.angularDrag > 1) {
				newaf -= 0.5f;
				rb.angularDrag = newaf;
			}
		}
		
		
		if (goingUp && (lastPos.y > transform.position.y)) {
			//fly();
			goingUp = false;
		} else {
			llastPos = lastPos;
			lastPos = transform.position;
		}
		//if (Input.GetKey (KeyCode.Space)) {
		if ((thalmicMyo.accelerometer.x < -0.4) || Input.GetKey (KeyCode.Space)) {
			//clip.Play ();
			//Debug.Log("drag decrease");
			if (rb.drag > 0.3f) {
				rb.drag -= 0.5f;
			}
			if (cf.force.z > -25) {
				newv.z -= 1f;
				//newv.y -= 1f;
				cf.force = newv;
			}
			spacedown = true;
			//ExtendUnlockAndNotifyUserAction (thalmicMyo);*/
			//this is wings expanded
			//should slow down and jump
			clip.Stop ();
		} else {
			//Debug.Log("resting");
			if (spacedown == true) {
				fly ();
				spacedown = false;
			}
			spacedown = false; 
			if (rb.drag < 10f) {
				rb.drag += 0.5f;
			}
			if (cf.force.z < -15f) {
				newv.z += 0.5f;
				//newv.y += 1f;
				cf.force = newv;
			}
		}
			//ExtendUnlockAndNotifyUserAction (thalmicMyo);
		 
		
		/*if (Input.GetKey (KeyCode.Space)) {

			if (rb.drag > 0.6f) {
				rb.drag -= 0.4f;
			}
			if (cf.force.z > -70) {
				newv.z -= 2f;
				//newv.y -= 1f;
				cf.force = newv;
			}
			spacedown = true;
		} else {
			if (spacedown == true) {
					fly();
					spacedown = false;
			}
			spacedown = false; 
			if (rb.drag < 10f) {
				rb.drag += 0.3f;
			}
			if (cf.force.z < -15f) {
				newv.z += 0.1f;
				//newv.y += 1f;
				cf.force = newv;
			}
		}*/
		
	}

	void fly() {
		cf = GetComponent<ConstantForce> ();
		OVRPlayerController sc = GetComponent<OVRPlayerController> ();
		sc.Jump ();
		
		//float theta = Mathf.Asin((lastPos.y - llastPos.y) / Mathf.Sqrt(Mathf.Pow(lastPos.z - llastPos.z, 2) + Mathf.Pow (lastPos.y - llastPos.y, 2)));
		/*Vector3 newv = cf.force;
		newv.y -= 500;
		//newv.z = 0;
		
		//newv.z = Mathf.Cos (theta * cf.force.z);
		//newv.y = cf.force.z + 10f;//Mathf.Sin (Mathf.PI / 4 * cf.force.z);
		
		cf.force = newv;*/
	}

	void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo)
	{
		ThalmicHub hub = ThalmicHub.instance;
		
		if (hub.lockingPolicy == LockingPolicy.Standard) {
			myo.Unlock (UnlockType.Timed);
		}
		
		myo.NotifyUserAction ();
	}
}
