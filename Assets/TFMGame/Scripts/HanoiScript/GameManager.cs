using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class GameManager : MonoBehaviour
{
    #region Fields
    public static GameManager gameManager;

    [SerializeField]
    private GameObject[] _discs;
    private Transform[] _moveTargets = new Transform[3];
    [SerializeField]
    private float _moveSpeed;

    private bool _isMoving = false;

    private bool _canSaveMoveFromKey = true;
    private string _moveFromKey;
    private bool _canSaveMoveToKey = true;
    private string _moveToKey;

    private bool CanPlay = true;
    // Disk components
    private GameObject _gameObject;
    private Rigidbody2D _rigidbody;
    private Transform _transform;

    private int[] _moveTargetsIndex = new int[2];
    private int _index = 0;
    // Components of a selected disk.
    private Transform _selectionTransform;
    private Transform _selectionPosition;
    private Vector3 _deltaPosition = new Vector3(0f, 0.1f,0f);
    [SerializeField]
    private float _risingSpeed = 1f;
    [SerializeField]
    private float _motionSpeed = 5f;

    private bool _hasReachedSelectionPos = false;
    private Dictionary<string, Stack> _towers = new Dictionary<string, Stack>
        {
            {"A",new Stack() },
            {"B",new Stack() },
            {"C",new Stack() }
        };

    [SerializeField]
    private Torre A;
    [SerializeField]
    private Torre B;
    [SerializeField]
    private Torre C;
    #endregion

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GetComponent<GameManager>();
        }


        for (int i = 0; i < _discs.Length; i++)
        {
            _towers["A"].Push(_discs[i]);
        }
        _moveTargets[0] = A.GetComponent<Torre>().SelectPoint;
        _moveTargets[1] = B.GetComponent<Torre>().SelectPoint;
        _moveTargets[2] = C.GetComponent<Torre>().SelectPoint;

        
    }

    private void Update()
    {
        
        if (!_isMoving && CanPlay)
        {
            
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                // Construct a ray from the current touch coordinates
                Ray ray = new Ray(Vector3.zero,Vector3.zero);

#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#elif (UNITY_IOS || UNITY_ANDROID)
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction,Mathf.Infinity,1);

                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    if (_canSaveMoveFromKey)
                    {
                        if (IsTowerSelectedValid(hit, true, _moveFromKey))
                        {
                            _moveFromKey = hit.collider.gameObject.name;

                            GameObject disk = PeekTower(_moveFromKey);
                            _selectionTransform = disk.GetComponent<Transform>();
                            _rigidbody = disk.GetComponent<Rigidbody2D>();

                            _rigidbody.isKinematic = true;
                            _rigidbody.simulated = false;

                            _selectionPosition = hit.collider.gameObject.GetComponent<Torre>().SelectPoint;
                            _canSaveMoveFromKey = false;
                        }
                    }
                    else if (_canSaveMoveToKey)
                    {
                        if (IsTowerSelectedValid(hit, false, _moveFromKey))
                        {
                            _moveToKey = hit.collider.gameObject.name;

                            _canSaveMoveToKey = false;

                            if (IsMoveSelectedValid(_moveFromKey, _moveToKey))
                            {
                                
                                // Allow the disc to move, and don't get user input
                                // while it is still moving.
                                _isMoving = true;

                                _hasReachedSelectionPos = false;

                                // Set up the moving disc components
                                _gameObject = MoveTopDisk(_moveFromKey, _moveToKey);
                                _transform = _gameObject.GetComponent<Transform>();
                                _rigidbody = _gameObject.GetComponent<Rigidbody2D>();

                                _rigidbody.simulated = false;
                                _rigidbody.isKinematic = true;


                                SetMoveTargetsIndex();

                                if (HasLevelFinished())
                                {
                                    CanPlay = false;


                                    //KickStarter.dialog.StartDialog(KickStarter.player, "Vale, con esto podré alcanzar la pipa [hold]", preventSkipping:true);
                                    
                                    //Cambio a true la variable global del minijuego
                                    AC.GlobalVariables.SetBooleanValue(45,true);

                                    StartCoroutine(WaitForInput());
                                }
                            }
                            else
                            {
                                //Poner que es un movimiento invalido
                                Debug.Log("Movimiento Invalido");
                                
                                KickStarter.dialog.StartDialog(KickStarter.player, "No puedo poner una caja mas grande encima de una mas pequeña");
                                _canSaveMoveFromKey = true;
                                _canSaveMoveToKey = true;

                                _rigidbody.isKinematic = false;
                                _rigidbody.simulated = true;

                                _hasReachedSelectionPos = false;
                            }
                        }
                        else
                        {
                            //movimiento invalido
                            
                            _canSaveMoveFromKey = true;
                            _canSaveMoveToKey = true;

                            _rigidbody.isKinematic = false;
                            _rigidbody.simulated = true;

                            _hasReachedSelectionPos = false;
                        }
                    }
                }
                else
                {
                    _canSaveMoveFromKey = true;
                    _canSaveMoveToKey = true;

                    if (_rigidbody != null)
                    {
                        _rigidbody.isKinematic = false;
                        _rigidbody.simulated = true;
                    }

                    _hasReachedSelectionPos = false;
                }

            }
            else
            {
                // If a disk has been selected but no tower destination
                // has been specified, move it around.
                if (_canSaveMoveFromKey == false && _canSaveMoveToKey == true)
                {
                    if (!_hasReachedSelectionPos)
                    {
                        _selectionTransform.position = Vector2.MoveTowards(_selectionTransform.position, _selectionPosition.position, Time.deltaTime * _risingSpeed);

                        if (Vector2.Distance(_selectionTransform.position, _selectionPosition.position) <= 0.01)
                            _hasReachedSelectionPos = true;
                    }
                    else
                    {
                        _selectionTransform.position = Vector3.Lerp(_selectionPosition.position, _selectionPosition.position + _deltaPosition, Mathf.PingPong(Time.timeSinceLevelLoad * _motionSpeed, 1f));
                    }
                }
            }
        }
        else
        {
            if (_isMoving)
            {
                Debug.Log("Moviendo disco");
                MoveDisc();
            }
        }
    }

    public GameObject PeekTower(string towerKey)
    {
        return (GameObject)_towers[towerKey].Peek();
    }

    public int GetCollectionCount(string towerKey)
    {
        return _towers[towerKey].Count;
    }
    private bool IsTowerSelectedValid(RaycastHit2D hit, bool isMoveFrom, string moveFromKey)
    {
        string towerKey = hit.collider.gameObject.name;

        // If it is the tower the disc is going to move from,
        // then check if the tower selected has discs.
        if (isMoveFrom)
        {
            if (GetCollectionCount(towerKey) > 0)
                return true;
            else
            {
                Debug.Log("Invalid selection. Tower has no discs.");

                KickStarter.dialog.StartDialog(KickStarter.player, "Aqui deberia poner una caja");
                
                return false;
            }
        }
        else
        {
            // If it is the tower the disc is goint to move to,
            // then check if the tower selected is not the same
            // has the tower it is moving from.
            if (towerKey == moveFromKey)
                return false;
            else
                return true;
        }
    }
    private bool IsMoveSelectedValid(string moveFromKey, string moveToKey)
    {
        // If MoveTo tower have discs compare top disc ranks, and evaluate
        // If it is possible based on MoveFrom Top Disc can not have greater
        // rank than MoveTo Top Disc.
        if (_towers[moveToKey].Count > 0)
        {
            GameObject moveFromTopDisc = (GameObject)_towers[moveFromKey].Peek();
            int moveFromRank = moveFromTopDisc.GetComponent<Disco>().size;

            GameObject moveToTopDisc = (GameObject)_towers[moveToKey].Peek();
            int moveToRank = moveToTopDisc.GetComponent<Disco>().size;

            if (moveFromRank < moveToRank)
                return true;
            else
                return false;
        }

        return true;
    }

    private GameObject MoveTopDisk(string moveFromKey, string moveToKey)
    {
        // Move the disc GameObject from one stack to another
        GameObject disk = (GameObject)_towers[moveFromKey].Pop();
        _towers[moveToKey].Push(disk);

        return disk;
    }

    private void SetMoveTargetsIndex()
    {
        if (_moveFromKey == "A")
            _moveTargetsIndex[0] = 0;
        else if (_moveFromKey == "B")
            _moveTargetsIndex[0] = 1;
        else
            _moveTargetsIndex[0] = 2;

        if (_moveToKey == "A")
            _moveTargetsIndex[1] = 0;
        else if (_moveToKey == "B")
            _moveTargetsIndex[1] = 1;
        else
            _moveTargetsIndex[1] = 2;
    }

    private void MoveDisc()
    {
        _transform.position = Vector2.MoveTowards(
            _transform.position,
            _moveTargets[_moveTargetsIndex[_index]].position,
            Time.deltaTime * _moveSpeed);

        if (Vector2.Distance(_moveTargets[_moveTargetsIndex[_index]].position, _transform.position) <= 0)
        {
            if (_index == 1)
            {
                _index = 0;
                _isMoving = false;
                _rigidbody.isKinematic = false;
                _rigidbody.simulated = true;

                _canSaveMoveFromKey = true;
                _canSaveMoveToKey = true;
            }
            else
            {
                _index++;
            }
        }
    }

    public bool HasLevelFinished()
    {
        if (_towers["A"].Count == 0 && _towers["B"].Count == 0)
            return true;

        return false;
    }

    private IEnumerator WaitForInput()
    {
        bool done = false;
        yield return new WaitForSeconds(1.0F);

        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            Debug.Log("State of keys: " + Input.GetKey(KeyCode.Mouse0) + "Number of touchCounts: " + Input.touchCount);
            if (Input.GetKey(KeyCode.Mouse0) || Input.touchCount > 0)
            {
                Debug.Log("State of keys: " + Input.GetKey(KeyCode.Mouse0) + "Number of touchCounts: " + Input.touchCount);
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
        FinishLevel();
    }
    private void FinishLevel()
    {
            // Devolver icono de inventario
            MenuElement _element = PlayerMenus.GetElementWithName("InventoryControl", "InventoryIcon");
            _element.IsVisible = true;
            AC.Menu _menu = PlayerMenus.GetMenuWithName("InventoryControl");
            _menu.TurnOn();
            _menu.ResetVisibleElements();
            _menu.Recalculate();

            //Cambio de escena
            SceneChanger sc = AC.KickStarter.sceneChanger;
            int prevScn = sc.GetPreviousSceneIndex();

            sc.ChangeScene(prevScn, true);

    }
}
