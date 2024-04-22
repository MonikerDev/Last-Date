using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [Header("Offset Values")]
    public float xOffSet;
    public float yOffSet;
    public GameObject player;
    private Vector2 cameraOffset;

    [Header("Edge Restrictions")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void Start()
    {
        player = GlobalVariableStorage.playerInstance.gameObject;
    }

    private void Update()
	{
        cameraOffset = new Vector2(Mathf.Clamp(player.transform.position.x +
            xOffSet, minX, maxX), Mathf.Clamp(player.transform.position.y + yOffSet, minY, maxY));

        this.transform.position = cameraOffset;
	}
}
