using System.Collections;
using UnityEngine;
using AC;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

public class VariableManager : MonoBehaviour
{
    private Grafo myGrafo = new Grafo();
    private List<Nodo> listaActivos = new List<Nodo>();

    [SerializeField]
    public TextAsset grafo;
    private void Awake()
    {
        EventManager.OnVariableChange += mOnVariableChange;
        EventManager.OnFinishLoading += mOnFinishLoading;
        EventManager.OnBeforeSaving += mOnBeforeSaving;
        EventManager.OnMenuElementClick += mOnTipMenuClick;
    }
    private void Start()
    {
        
        string graphVar = AC.GlobalVariables.GetVariable("graphVar").GetValue();

        if (graphVar.Length <= 0)
        {
            string grafoText = grafo.text;
            readDefaultGrafoFile(grafoText);
        }
        else
            myGrafo = DeserializeGrafo(graphVar);
    }


    public void readDefaultGrafoFile(string file)
    {

        var jsonArray = JArray.Parse(file);

        var nodos = new ArrayList();
        foreach (var data in jsonArray[0]["Nodes"])
        {
            var name = (string)data["variableId"];
            var isActive = (bool)data["isActive"];
            var fraseTip = (string)data["fraseTip"];
            Nodo newNodo = new Nodo(name, isActive, fraseTip);
            nodos.Add(newNodo);
            if (newNodo.IsActive)
                listaActivos.Add(newNodo);
        }

        foreach (Nodo nodo in nodos)
        {
            //Debug.Log(nodo.variableId);
            foreach (var predecesor in jsonArray[0]["Adjacency"][nodo.variableId]["predecessors"])
            {
                int index = nodos.IndexOf(new Nodo(predecesor.ToString()));
                nodo.addPredecessor((Nodo)nodos[index]);
            }
            if (nodo.Predecessors.Count <= 0)
                myGrafo.NodoRaiz = nodo;
            foreach (var successor in jsonArray[0]["Adjacency"][nodo.variableId]["successors"])
            {
                int index = nodos.IndexOf(new Nodo(variableId: successor.ToString()));
                nodo.addSuccessor((Nodo)nodos[index]);
            }
        }

        string serializedGrafo = SerializeGrafo(myGrafo);

        //Asignar el string a la variable global
        AC.GlobalVariables.GetVariable("graphVar").SetStringValue(serializedGrafo);
    }

    private string SerializeGrafo(Grafo grafo)
    {
        Debug.Log("Serializando el grafo de variables");
        var root = JsonConvert.SerializeObject(grafo, new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            MaxDepth = 128
        });

        return (string)root;
    }
    private Grafo DeserializeGrafo(string grafo)
    {
        Debug.Log("Deserializando el grafo de variables");
        var settings = new JsonSerializerSettings {
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            MaxDepth = 128
        };
        var _jsonSerializer = JsonSerializer.Create(settings);
        var root = JsonConvert.DeserializeObject<JObject>(grafo, settings).ToObject<Grafo>(_jsonSerializer);

        return (Grafo)root;
    }
    private void mOnVariableChange(GVar variable)
    {
        if (variable.type != VariableType.Boolean)
            return;
        if (AC.GlobalVariables.GetVariable("graphVar").GetValue().Length <= 0)
        {
            string grafoText = grafo.text;
            readDefaultGrafoFile(grafoText);
        }

        bool varIdChanged = variable.BooleanValue;

        Debug.Log("Variable " + variable.label + " es " + varIdChanged);
        if (varIdChanged)
        {
            Nodo findNodo = new Nodo(variable.label);

            if (myGrafo.deactivateNode(findNodo, ref listaActivos))
            {
                Debug.Log("Variable" + variable.label + " desactivada");
                GVar trueVars = AC.GlobalVariables.GetVariable(3);
                GVar progreso = AC.GlobalVariables.GetVariable(2);
                GVar varCount = AC.GlobalVariables.GetVariable("varCount");
                trueVars.IntegerValue = trueVars.IntegerValue + 1;

                float progresoPrcnt = ((float)trueVars.IntegerValue/(float)varCount.IntegerValue)*100f;
                progreso.SetStringValue( "Progreso: " + (int)progresoPrcnt + "%");
            }
            else
                Debug.Log("No se pudo desactivar variable: " + variable.label);
        }

    }
    private void mOnFinishLoading()
    {

        string grafoSerialized = AC.GlobalVariables.GetVariable("graphVar").GetValue();

        myGrafo = DeserializeGrafo(grafoSerialized);

    }
    private void mOnBeforeSaving(int saveID)
    {
        string serializedGrafo = SerializeGrafo(myGrafo);

        //Asignar el string a la variable global
        AC.GlobalVariables.GetVariable("graphVar").SetStringValue(serializedGrafo);
    }
    private void mOnTipMenuClick(Menu _menu, MenuElement _element, int _slot, int buttonPressed)
    {
        if (!(_menu.title.Equals("TipMenu")))
            return;
        
        int indexRand = Random.Range(0, listaActivos.Count - 1);
        AC.KickStarter.dialog.StartDialog(KickStarter.player, listaActivos[indexRand].FraseTip,preventSkipping:true);
    }

    public void OptionOne()
    {
        StartCoroutine(OptionOneCoroutine());
    }

    private IEnumerator OptionOneCoroutine()
    {
        int indexRand = new System.Random().Next(0, listaActivos.Count - 1);
        //KickStarter.stateHandler.StartCutscene();

        Speech speech = AC.KickStarter.dialog.StartDialog(KickStarter.player, listaActivos[indexRand].FraseTip, false, -1, false, true);

        while (speech.isAlive)
        {
            yield return null;
        }

        //KickStarter.stateHandler.EndCutscene();
    }
}
