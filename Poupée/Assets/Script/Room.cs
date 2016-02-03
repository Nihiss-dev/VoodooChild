using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public GameObject _door;
	public List<GameObject> _listIA;
    private bool _inRoom = false;
    private int _roomId = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
			GetComponentInParent<RoomManager> ().changeActualRoom (_roomId);
			if (_door)
				_door.GetComponent<Door>()._state = false;
			for (int i = 0; i < _listIA.Count; ++i)
				_listIA[i].GetComponent<EnemyAI>().Activate(false);
		}
    }

    public void setRoomId(int id)
    {
        _roomId = id;
    }
}
