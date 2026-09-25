using UnityEngine;

public class scr_Sunlight : MonoBehaviour
{
	public Vector3 SunPosition { get; private set; }
	public Vector3 AimPosition { get; private set; }
	private scr_InputManager inputManager;
	private const int MaxTransparentHits = 32;
	private const float RayOffset = 0.001f;
	public Vector3 RayDirection { get; private set; }

	void Start()
	{
		inputManager = GameManager.s_Instance.GetComponent<scr_InputManager>();
		RayDirection = transform.forward;
	}

	void Update()
	{
		SunPosition = transform.position;
		AimPosition = new Vector3(-SunPosition.x, 5, -SunPosition.z);
		Vector3 calculatedDirection = AimPosition - SunPosition;
		if (calculatedDirection.sqrMagnitude > 0.000001f)
		{
			RayDirection = calculatedDirection.normalized;
		}

		if (RayDirection.sqrMagnitude <= 0.000001f)
		{
			RayDirection = Vector3.down;
		}

		transform.rotation = Quaternion.LookRotation(RayDirection);
		Sunlight();
	}

	private void Sunlight()
	{
		Vector3 rayDirection = RayDirection;
		Vector3 rayOrigin = SunPosition;

		for (int hitCount = 0; hitCount < MaxTransparentHits; hitCount++)
		{
			if (!Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo))
			{
				break;
			}

			if (inputManager.IsHitSolid(hitInfo))
			{
				Vector3 hitDirection = hitInfo.point - SunPosition;
				if (hitDirection.sqrMagnitude > 0.000001f)
				{
					transform.LookAt(hitInfo.point);
					RayDirection = transform.forward;
				}
				break;
			}
			rayOrigin = hitInfo.point + rayDirection * RayOffset;
		}

		RayDirection = transform.forward;
		Debug.DrawRay(SunPosition, rayDirection * 10000f, Color.yellow);
	}
}
