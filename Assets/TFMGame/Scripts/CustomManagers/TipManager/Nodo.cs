using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

public class Nodo
{
    public string variableId;
    private bool isActive;
    private bool variableStatus;
    private string fraseTip;
    private bool isOptional;
    [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Serialize)]
    private List<Nodo> predecessors;
    [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Serialize)]
    private List<Nodo> successors;

    public Nodo(string variableId = "", bool isActive = false, string fraseTip = "",
        List<Nodo> predecessors = null, List<Nodo> successors = null, bool variableStatus = false, bool isOptional =false)
    {
        this.variableId = variableId;
        this.isActive = isActive;
        this.fraseTip = fraseTip;
        this.isOptional = isOptional;
        this.predecessors = new List<Nodo>();
        if (predecessors != null)
            this.predecessors.AddRange(predecessors);
        this.successors = new List<Nodo>();
        if (successors != null)
            this.successors.AddRange(successors);
        this.variableStatus = (variableStatus);
    }
    public Nodo() { }

    public global::System.Boolean IsOptional { get => isOptional; set => isOptional = value; }
    public global::System.Boolean IsActive { get => isActive; set => isActive = value; }
    public global::System.String FraseTip { get => fraseTip; set => fraseTip = value; }
    public List<Nodo> Predecessors { get => predecessors; set => predecessors = value; }
    public List<Nodo> Successors { get => successors; set => successors = value; }
    public bool VariableStatus { get => variableStatus; set => variableStatus = value; }

    public void addPredecessor(Nodo newNodo)
    {
        this.predecessors.Add(newNodo);
    }

    public void removePredecessor(Nodo oldNodo)
    {
        this.predecessors.Remove(oldNodo);
    }

    public void addSuccessor(Nodo newNodo)
    {
        this.successors.Add(newNodo);
    }

    public void removeSuccessor(Nodo oldNodo)
    {
        this.successors.Remove(oldNodo);
    }

    public override bool Equals(Object obj)
    {
        if (obj == null)
        {
            return false;
        }
        // If the passed object is not Customer Type, return False
        if (!(obj is Nodo))
        {
            return false;
        }

        return this.variableId.Equals(((Nodo)obj).variableId);
    }

    public static bool operator ==(Nodo obj1, Nodo obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(Nodo obj1, Nodo obj2)
    {
        return !(obj1 == obj2);
    }
}
