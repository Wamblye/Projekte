using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormInfos
{
    public int vorgabeSlot;
    public int form;
    public int farbe;
    public int groupSlot;

    public FormInfos(int newVorgabeSlot, int newForm, int newFarbe, int newGroupSlot)
    {
        vorgabeSlot = newVorgabeSlot;
        form = newForm;
        farbe = newFarbe;
        groupSlot = newGroupSlot;
    }
}
