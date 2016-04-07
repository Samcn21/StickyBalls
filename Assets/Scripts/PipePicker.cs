using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipePicker : MonoBehaviour {
    private List<GameObject> _pipeSelected;
    private PlayerManager _playerManagerRef;
    void Awake()
    {
        _pipeSelected = new List<GameObject>();
        _playerManagerRef = GetComponent<PlayerManager>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(_playerManagerRef.isCarryingEmptyPipe()||_pipeSelected.Count>0)
        if (col.gameObject.tag == "Pipe")
        {
            switch (col.gameObject.GetComponent<ConveyorPipe>().PipeType)
            {
                case PipeType.Corner:
                    _playerManagerRef.carryCornerPipe();
                    _pipeSelected.Add(col.gameObject);
                    break;
                case PipeType.Cross:
                    _playerManagerRef.carryCrossPipe();
                    _pipeSelected.Add(col.gameObject);
                    break;
                case PipeType.T:
                    _playerManagerRef.carryTPipe();
                    _pipeSelected.Add(col.gameObject);
                    break;
                case PipeType.Straight:
                    _playerManagerRef.carryStraightPipe();
                    _pipeSelected.Add(col.gameObject);
                    break;
            }
        }

    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Pipe")
        {
            if (_pipeSelected.Count == 1)
            {
                _pipeSelected[0].GetComponent<ConveyorPipe>().PickPipe();
                _pipeSelected.RemoveAt(0);
            }
            else
                _pipeSelected.Remove(col.gameObject);
        }
    }
}
