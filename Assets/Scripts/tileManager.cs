using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tileManager : MonoBehaviour {

    //private bool _playerInside;
    //private bool _playerThatCanModifyInside;
    //private int _playerThatCanModify;
    //private int _lastPlayerInside;
    //private GameObject _lastPlayerRef;

    //private string _rotateToLeft;
    //private string _rotateToRight;
    //private string _placeTile;

    //private bool _rotated;

    ////private InputsManager _inputsManager;

    //private List<List<Vector2>> _pipeHolePositions;

    //private int _indexHoles;
    //[SerializeField]
    //private PipeData.PipeType type;

    //public int xPos, yPos;
    //// Use this for initialization
    //void Awake () {
    //    _playerThatCanModify = -1;
    //    _playerInside = false;
    //    _playerThatCanModifyInside = false;
    //    _rotated = false;
    //    _lastPlayerInside = -1;
    //    //_inputsManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputsManager>();
    //    _indexHoles = 0;
    //}
	
    //// Update is called once per frame
    //void Update () {
    //    if (_playerThatCanModifyInside)
    //    {
    //        float l, r;
    //        l = Input.GetAxis(_rotateToLeft);
    //        r = Input.GetAxis(_rotateToRight);
    //        if (r== 1 && !_rotated)
    //            rotateToRight();
    //        if (l== 1&&!_rotated)
    //            rotateToLeft();
    //        if (l != 1&&r!=1)
    //            _rotated = false;
    //    }
    //        if (_playerInside)
    //        {
    //        if (Input.GetAxis(_placeTile)==1)
    //        {
    //           // PlayerPipeManager _pipeManager= _lastPlayerRef.GetComponent<PlayerPipeManager>();
               
               
    //            foreach (Vector2 pos in _pipeHolePositions[_indexHoles])
    //            {
    //                bool found = false;
    //                //Cycle over the current sourcepositions to detect if one of the pipeholeposition is the same of one of the source position (if the pipe that you are trying to place match with the other pipes)
    //                foreach (Vector2 availablePos in _pipeManager.lastSourcePositions)
    //                    if (pos == availablePos)
    //                    {
    //                        //If you find a match, place the pipe, remove from the source position and add to the source positions every valid free position
    //                        GameObject.FindGameObjectWithTag("GameController").GetComponent<PipeManager>().placePipeOfTypeAt(_lastPlayerInside, type, xPos, yPos, transform.rotation);
    //                        _pipeManager.removeFreePosition(new Vector2(xPos, yPos));
    //                        foreach (Vector2 p in _pipeHolePositions[_indexHoles])
    //                            if (p != pos&&p.x>=0&&p.x<GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>().r&&p.y>=0&&p.y<GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>().c)
    //                                _pipeManager.freePositions.Add(p);
    //                        found = true;
    //                        break;
    //                    }
    //                if (found)
    //                {
    //                    //If you find a match, destroy the placeholder and add the current position in the last source positions
    //                    _pipeManager.lastSourcePositions.Add(new Vector2(xPos, yPos));
    //                    Destroy(gameObject);
    //                }
    //            }
                
    //        }
    //        }
        

    //}
    ////Function that generates the different free positions based on the type of the pipe and on the rotation
    //public void updatePipeHolePositions()
    //{
    //    _pipeHolePositions = new List<List<Vector2>>();
    //    List<Vector2> l = new List<Vector2>();
    //    List<Vector2> l1 = new List<Vector2>();
    //    List<Vector2> l2 = new List<Vector2>();
    //    List<Vector2> l3 = new List<Vector2>();
    //    switch (type)
    //    {
    //        case PipeData.PipeType.Corner:
    //            l.Add(new Vector2(xPos - 1, yPos));
    //            l.Add(new Vector2(xPos, yPos - 1));
    //            _pipeHolePositions.Add(l);
    //            l1.Add(new Vector2(xPos, yPos - 1));
    //            l1.Add(new Vector2(xPos + 1, yPos));
    //            _pipeHolePositions.Add(l1);
    //            l2.Add(new Vector2(xPos + 1, yPos));
    //            l2.Add(new Vector2(xPos, yPos + 1));
    //            _pipeHolePositions.Add(l2);
    //            l3.Add(new Vector2(xPos, yPos + 1));
    //            l3.Add(new Vector2(xPos-1, yPos));
    //            _pipeHolePositions.Add(l3);
    //            break;
    //        case PipeData.PipeType.Cross:
    //            l.Add(new Vector2(xPos + 1, yPos));
    //            l.Add(new Vector2(xPos - 1, yPos));
    //            l.Add(new Vector2(xPos, yPos + 1));
    //            l.Add(new Vector2(xPos, yPos - 1));
    //            _pipeHolePositions.Add(l);
    //            break;
    //        case PipeData.PipeType.T:
    //            l.Add(new Vector2(xPos + 1, yPos));
    //            l.Add(new Vector2(xPos, yPos+1));
    //            l.Add(new Vector2(xPos, yPos - 1));
    //            _pipeHolePositions.Add(l);
    //            l1.Add(new Vector2(xPos + 1, yPos));
    //            l1.Add(new Vector2(xPos-1, yPos));
    //            l1.Add(new Vector2(xPos, yPos - 1));
    //            _pipeHolePositions.Add(l1);
    //            l2.Add(new Vector2(xPos, yPos+1));
    //            l2.Add(new Vector2(xPos, yPos - 1));
    //            l2.Add(new Vector2(xPos-1, yPos));
    //            _pipeHolePositions.Add(l2);
    //            l3.Add(new Vector2(xPos + 1, yPos));
    //            l3.Add(new Vector2(xPos - 1, yPos));
    //            l3.Add(new Vector2(xPos, yPos - 1));
    //            _pipeHolePositions.Add(l3);
    //            break;
    //        case PipeData.PipeType.Straight:
    //            l.Add(new Vector2(xPos + 1, yPos));
    //            l.Add(new Vector2(xPos - 1, yPos));
    //            _pipeHolePositions.Add(l);
    //            l1.Add(new Vector2(xPos, yPos+1));
    //            l1.Add(new Vector2(xPos, yPos-1));
    //            _pipeHolePositions.Add(l1);
    //            break;
    //    }

    //}

    //public void rotateToRight()
    //{
    //    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
    //    _rotated = true;
    //    Debug.Log(gameObject.name);
    //    Debug.Log("BEFORE");
    //    foreach (Vector2 v in _pipeHolePositions[_indexHoles])
    //        Debug.Log(v);
    //    if (_indexHoles + 1 < _pipeHolePositions.Count)
    //        _indexHoles ++;
    //    else
    //        _indexHoles=0;
    //    Debug.Log("AFTER");
    //    foreach (Vector2 v in _pipeHolePositions[_indexHoles])
    //        Debug.Log(v);
    //}

    //public void rotateToLeft()
    //{
    //    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 90, 0);
    //    _rotated = true;
    //    if (_indexHoles - 1 < 0)
    //        _indexHoles = _pipeHolePositions.Count-1;
    //    else
    //        _indexHoles--;
    //}

    //void OnTriggerEnter(Collider col)
    //{
    //    if(col.gameObject.tag=="Player")
    //    {
    //        _playerInside = true;
    //        _lastPlayerRef = col.gameObject;
    //        _lastPlayerInside = col.gameObject.GetComponent<PlayerManager>().index;
    //        _placeTile = _inputsManager.playerInputs[_lastPlayerInside].placePipe;
    //        if (_playerThatCanModify == -1) {
    //            _playerThatCanModify = col.gameObject.GetComponent<PlayerManager>().index;
    //            _rotateToLeft = _inputsManager.playerInputs[_playerThatCanModify].rotatePipeLeft;
    //            _rotateToRight = _inputsManager.playerInputs[_playerThatCanModify].rotatePipeRight;
    //            _playerThatCanModifyInside = true;
    //        }
    //        if (col.gameObject.GetComponent<PlayerManager>().index == _playerThatCanModify)
    //            _playerThatCanModifyInside = true;
    //    }
    //}

    //void OnTriggerExit(Collider col)
    //{
    //    if (col.gameObject.tag == "Player")
    //    {
    //        _playerInside = false;
    //        _playerThatCanModifyInside = false;
    //    }
    //}
}
