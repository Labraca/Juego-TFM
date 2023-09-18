using AC;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class CampanasPuzzle : MonoBehaviour
{
    #region Fields
    public static CampanasPuzzle campanasPuzzle;
    [SerializeField]
    private GameObject[] _teclas;
    [SerializeField]
    public string mSolucion;
    [SerializeField]
    private ActionList finalCinematic;
    [SerializeField]
    private ActionList invFinalCinematic;
    [SerializeField]
    private GameObject _silbidoUI;

    public FixedSizedQueue<int> mCola = new FixedSizedQueue<int>();
    public bool gameFinished = false;
    public bool reversedActive = false;
    #endregion
    
    
    void Awake()
    {
        
        if (campanasPuzzle == null)
        {
            campanasPuzzle = GetComponent<CampanasPuzzle>();
        }
    }
    private void Start()
    {
        /* Desactivo los elementos de la Interfaz que no se vayan a utilizar y desactivo el plugin de AC
         MenuElement _element = PlayerMenus.GetElementWithName("InventoryControl", "InventoryIcon");
         _element.IsVisible = false;
         AC.Menu _Inventory = PlayerMenus.GetMenuWithName("InventoryControl");
         _Inventory.TurnOff();
         _Inventory.ResetVisibleElements();
         _Inventory.Recalculate();
         AC.Menu _Options = AC.PlayerMenus.GetMenuWithName("MapaControl");
         AC.Menu _InGame = AC.PlayerMenus.GetMenuWithName("InGame");
         AC.Menu _TipMenu = AC.PlayerMenus.GetMenuWithName("TipMenu");
         //_Inventory.TurnOff();
         _Options.TurnOff();
         _InGame.TurnOff();

         AC.KickStarter.TurnOffAC();
         */


        gameFinished = false;
        mSolucion = Reverse(mSolucion);

        reversedActive = AC.GlobalVariables.GetVariable("MultitudFuriosaEvent").BooleanValue;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameFinished)
        {
            //Si el juego ha sido resuelto, el script dejara de realizar operaciones
            return;
        }
        if (reversedActive && isPuzzleInvSolved())
        {
            gameFinished = true;
            FinalizePuzzleCutscene(invFinalCinematic);
        }
        if (isPuzzleSolved() && !reversedActive)
        {
            //cinematica de juego terminado
            gameFinished = true;
            FinalizePuzzleCutscene(finalCinematic);
            
        }
       // else
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {

            Ray ray = new Ray(Vector3.zero, Vector3.zero);

#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#elif (UNITY_IOS || UNITY_ANDROID)
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 1);

            if (hit.collider != null)
            {
                int teclaTocada = Int32.Parse(hit.collider.gameObject.name);
                mCola.Enqueue(teclaTocada);

            }
        }
        }

    private void FinalizePuzzleCutscene(ActionList cutscene)
    {
        DisableColliders(_teclas);
        _silbidoUI.SetActive(false);
        //AC.KickStarter.TurnOnAC();

        cutscene.Interact();

        DisableColliders(_teclas);
        _silbidoUI.SetActive(true);
        //AC.KickStarter.TurnOffAC();
        /*
        MenuElement _element = PlayerMenus.GetElementWithName("InventoryControl", "InventoryIcon");
        _element.IsVisible = true;
        AC.Menu _Inventory = PlayerMenus.GetMenuWithName("InventoryControl");
        _Inventory.TurnOn();
        _Inventory.ResetVisibleElements();
        _Inventory.Recalculate();

        AC.Menu _Options = AC.PlayerMenus.GetMenuWithName("MapaControl");
        AC.Menu _InGame = AC.PlayerMenus.GetMenuWithName("InGame");

        _Options.TurnOn();
        _InGame.TurnOn();
        */


    }

    private void DisableColliders(GameObject[] teclas)
    {
        foreach(GameObject tecla in teclas)
        {
            tecla.GetComponent<Collider2D>().enabled = false;
        }
    }

    //Comprobar que el puzle ha sido resuelto
    private bool isPuzzleSolved()
    {
        
        if(mCola.Count < 4)
            return false;

        string estado = "";
        Array mColaArr = mCola.ToArray();

        for (int i = mCola.Count-1; i>=0; i--)
            estado = estado + mColaArr.GetValue(i);

        Debug.Log("Estado de la Cola:" + estado);

        return estado.Equals(mSolucion);
    }
    private bool isPuzzleInvSolved()
    {

        if (mCola.Count < 4)
            return false;

        string estado = "";
        Array mColaArr = mCola.ToArray();

        for (int i = 0; i < mCola.Count; i++)
            estado = estado + mColaArr.GetValue(i);

        Debug.Log("Estado de la Cola Inv:" + estado);

        return estado.Equals(mSolucion);
    }
    //La cola tiene un limite de 4 objetos
    public class FixedSizedQueue<T>:Queue<T>
    {
        const int MAX_SIZE_QUEUE = 4;
        //Override del metodo de poner en cola
        public new void Enqueue(T obj)
        {
            if (this.Count == MAX_SIZE_QUEUE)
                this.Dequeue();
            base.Enqueue(obj);
        }
    }

    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
