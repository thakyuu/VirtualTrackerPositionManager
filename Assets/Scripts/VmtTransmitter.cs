using UnityEngine;

public class VmtTransmitter : MonoBehaviour
{
	private uOSC.uOscClient _client;
	[SerializeField] private int clientIndex;
	[SerializeField] public int isEnable = 1;

	private void Start()
	{
		_client = GetComponent<uOSC.uOscClient>();
	}

	private void Update()
	{
		var trans = transform;
		var position = trans.position;
		var rotation = trans.rotation;

		_client.Send("/VMT/Room/Unity", clientIndex, isEnable, 0f,
			position.x,
			position.y,
			position.z,
			rotation.x,
			rotation.y,
			rotation.z,
			rotation.w
		);
	}
}