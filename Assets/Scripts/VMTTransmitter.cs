using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMTTransmitter : MonoBehaviour
{
	uOSC.uOscClient client;
	[SerializeField] private int clientIndex = 0;
	[SerializeField] public int isEnable = 1;
	void Start()
	{
		client = GetComponent<uOSC.uOscClient>();
	}

	void Update()
	{
		client.Send("/VMT/Room/Unity", (int)clientIndex, (int)isEnable, (float)0f,
			(float)transform.position.x,
			(float)transform.position.y,
			(float)transform.position.z,
			(float)transform.rotation.x,
			(float)transform.rotation.y,
			(float)transform.rotation.z,
			(float)transform.rotation.w
		);
	}
}