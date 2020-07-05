using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VmtTransmitter : MonoBehaviour
{
	private uOSC.uOscClient _client;
	[SerializeField] private int clientIndex = 0;
	[SerializeField] public int isEnable = 1;
	void Start()
	{
		_client = GetComponent<uOSC.uOscClient>();
	}

	void Update()
	{
		var trans = transform;
		var position = trans.position;
		var rotation = trans.rotation;

		_client.Send("/VMT/Room/Unity", (int)clientIndex, (int)isEnable, (float)0f,
			(float)position.x,
			(float)position.y,
			(float)position.z,
			(float)rotation.x,
			(float)rotation.y,
			(float)rotation.z,
			(float)rotation.w
		);
	}
}