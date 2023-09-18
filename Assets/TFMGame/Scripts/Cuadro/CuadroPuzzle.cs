using AC;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuadroPuzzle : MonoBehaviour
{
    #region Fields
    public static CuadroPuzzle _cuadroPuzzle;
    [SerializeField]
    private GameObject[] _Diffs;
    [SerializeField]
    private ActionList _finalCinematic;

    public bool gameFinished = false;
    public bool gameStarted = false;

    #endregion


    void Awake()
    {

        if (_cuadroPuzzle == null)
        {
            _cuadroPuzzle = GetComponent<CuadroPuzzle>();
        }
    }
    public void StartGame()
    {
        gameStarted = true;

        for(int i = 0; i< _Diffs.Length-1; i++)
        {
            _Diffs[i].gameObject.GetComponent<DiffObject>().isActive = false;
        }
    }
    public void PauseGame()
    {
        gameStarted = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
            return; 
        
        if (gameFinished)
            return;

        if (isPuzzleSolved())
        {
            //cinematica de juego terminado
            gameFinished = true;
            FinalizePuzzleCutscene(_finalCinematic);

        }
    }

    private void FinalizePuzzleCutscene(ActionList cutscene)
    {
        DisableColliders(_Diffs);
        cutscene.Interact();
    }

    private void DisableColliders(GameObject[] teclas)
    {
        foreach (GameObject tecla in teclas)
        {
            tecla.GetComponent<Collider2D>().enabled = false;
        }
    }

    //Comprobar que el puzle ha sido resuelto
    private bool isPuzzleSolved()
    {

        for (int i = _Diffs.Length - 1; i >= 0; i--)
        {
            if (!_Diffs[i].GetComponent<DiffObject>().isActive)
                return false;
        }

        return true;
    }
    
}
