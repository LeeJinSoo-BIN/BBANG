using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCircle : MonoBehaviour
{

	private static DamageCircle instance;

	[SerializeField] private Transform targetCircleTransform;

	private Transform circleTransform;

	private float circleShrinkSpeed;
	private float shrinkTimer;

	private Vector3 circleSize;
	private Vector3 circlePosition;

	private Vector3 targetCircleSize;
	private Vector3 targetCirclePosition;

	private void Awake()
	{
		instance = this;

		circleShrinkSpeed = 1f;

		circleTransform = transform.Find("circle");

		SetCircleSize(new Vector3(-313, 70, 24), new Vector3(500, 500, 500));

		SetTargetCircle(new Vector3(Random.Range(-438, -187), 70, Random.Range(-101, 150)), new Vector3(250, 250, 250), 5f);
	}

	private void Update()
	{
		shrinkTimer -= Time.deltaTime;

		if (shrinkTimer < 0)
		{
			Vector3 sizeChangeVector = (targetCircleSize - circleSize).normalized;
			Vector3 newCircleSize = circleSize + sizeChangeVector * Time.deltaTime * circleShrinkSpeed;

			Vector3 circleMoveDir = (targetCirclePosition - circlePosition).normalized;
			Vector3 newCirclePosition = circlePosition + circleMoveDir * Time.deltaTime * circleShrinkSpeed;

			SetCircleSize(newCirclePosition, newCircleSize);

			float distanceTestAmount = .1f;
			if (Vector3.Distance(newCircleSize, targetCircleSize) < distanceTestAmount && Vector3.Distance(newCirclePosition, targetCirclePosition) < distanceTestAmount)
			{
				GenerateTargetCircle();
			}
			else
				return;
		}
	}

	private void GenerateTargetCircle()
	{
		float shrinkSizeAmount = Random.Range(20f, 80f);
		Vector3 generatedTargetCircleSize;

		if (circleSize.magnitude <= new Vector3(shrinkSizeAmount, shrinkSizeAmount, shrinkSizeAmount).magnitude)
		{
			generatedTargetCircleSize = new Vector3(0, 0, 0);
		}
		else
		{
			generatedTargetCircleSize = circleSize - new Vector3(shrinkSizeAmount, shrinkSizeAmount, shrinkSizeAmount);
		}

		Vector3 generatedTargetCirclePosition = circlePosition +
			new Vector3(Random.Range(-shrinkSizeAmount, shrinkSizeAmount), 0, Random.Range(-shrinkSizeAmount, shrinkSizeAmount));

		float shrinkTime = Random.Range(1f, 4f);

		SetTargetCircle(generatedTargetCirclePosition, generatedTargetCircleSize, shrinkTime);
	}

	private void SetCircleSize(Vector3 position, Vector3 size)
	{
		circlePosition = position;
		circleSize = size;

		transform.position = position;

		circleTransform.localScale = size;
	}

	private void SetTargetCircle(Vector3 position, Vector3 size, float shrinkTimer)
	{
		this.shrinkTimer = shrinkTimer;

		targetCircleTransform.position = position;
		targetCircleTransform.localScale = size;

		targetCircleSize = size;
		targetCirclePosition = position;
	}
	private bool IsOutsideCircle(Vector3 position)
	{
		return Vector3.Distance(position, circlePosition) > circleSize.x * .5f;
	}

	public static bool IsOutsideCircle_Static(Vector3 position)
	{
		return instance.IsOutsideCircle(position);
	}
};

