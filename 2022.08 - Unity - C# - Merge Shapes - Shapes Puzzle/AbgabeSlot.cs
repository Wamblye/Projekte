using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbgabeSlot : MonoBehaviour, IDropHandler
{
    public GameObject targetSlot;

    private GameManager manager;
    private GameObject gameObjDrag;
    private List<FormInfos> formVorgabe;
    private List<FormInfos> formVorgabeGesamt;
    private List<FormInfos> formAbgabe;
    private List<GameObject> listVorgabeGameObjekts;

    private int keinGroupSlot = 10;
    private bool abgabeKorrekt = false;

    private int dragSlot = 0;

    void Awake()
    {
        abgabeKorrekt = false;
        manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        formVorgabeGesamt = new List<FormInfos>();
        listVorgabeGameObjekts = manager.GetVorgabeGameObjekts();

		// GameObject Vorgaben in eine Liste Speichern
        for (int i = 0; i < listVorgabeGameObjekts.Count; i++)
        {
            for (int z = 0; z < 6; z++) // 6 Slots
            {
                SpriteZahlVorgabe(i, listVorgabeGameObjekts[i].gameObject.transform.GetChild(z).GetComponent<Image>().sprite.name, keinGroupSlot);
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Image imSlot = null;
        gameObjDrag = eventData.pointerDrag;

		// Nur GameObjekte mit Unter Objekten abfragen
        if (gameObject.transform.childCount > 0)
        {
            dragSlot = int.Parse(gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            imSlot = gameObject.transform.GetChild(1).GetComponent<Image>();
        }

        if(eventData.pointerDrag.transform.childCount > 1) 
        {
            string spriteNameDrop = eventData.pointerDrag.GetComponent<Image>().sprite.name;
            int keinDragMehr = int.Parse(eventData.pointerDrag.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text);

            // Korrekt abgegebene Form darf nicht nochmal geprüft werden
            if (keinDragMehr != 1)
            {
                formAbgabe = new List<FormInfos>();
                if (SpriteZahl(spriteNameDrop) == 1000)
                {
                    for (int z = 0; z < 6; z++) // 6 Slots
                    {
                        SpriteZahlDrop(dragSlot, eventData.pointerDrag.transform.GetChild(z).GetComponent<Image>().sprite.name, z);
                    }

                    SetAbgabeSlot();
                    CheckAbgabe();

					// Objekt auf Slot einrasten lassen
                    if (abgabeKorrekt == true)
                    {
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = targetSlot.transform.GetComponent<RectTransform>().anchoredPosition;
                        eventData.pointerDrag.transform.GetChild(6).GetComponent<TextMeshProUGUI>().SetText("1");
                        imSlot.enabled = true;
                        AudioManager.instance.Play("Korrekt");
                    }
                    else // Abgabe Falsch: Objekt unterhalb der Abgabe absetzen
                    {
                        Vector2 newPos = new Vector2(targetSlot.transform.GetComponent<RectTransform>().anchoredPosition.x, 
                                                targetSlot.transform.GetComponent<RectTransform>().anchoredPosition.y - 200);
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = newPos;
                        AudioManager.instance.Play("Wrong");
                    } 
                }
            }
        }
        else // Nicht zusammengesetzt Formen dürfen nicht abgegeben werden
        {
            Vector2 newPos = new Vector2(targetSlot.transform.GetComponent<RectTransform>().anchoredPosition.x, 
                                            targetSlot.transform.GetComponent<RectTransform>().anchoredPosition.y - 200);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = newPos;
            AudioManager.instance.Play("Wrong");
        }
    }


    private void CheckAbgabe()
    {
        for (int v = 0; v < formVorgabe.Count; v++)
        {
            if (formVorgabe[v].vorgabeSlot == formAbgabe[v].vorgabeSlot)
            {
                if (formVorgabe[v].form == formAbgabe[v].form &&
                formVorgabe[v].farbe == formAbgabe[v].farbe)
                {
                    // Richtig
                    Debug.Log("abgabeKorrekt: Richtig");
                    abgabeKorrekt = true;
                    SetAbgabeKorrekt(formVorgabe[v].vorgabeSlot, abgabeKorrekt);
                }
                else
                {
                    // Falsch
                    Debug.Log("abgabeKorrekt: Falsch");
                    abgabeKorrekt = false;
                    // Anzahl der Abagbeslots abfragen, damit die leeren Slot mit TRUE nicht auf FALSE gesetzt werden.
                    // Nur die mit Vorgabe dürfen auf FALSE gesetzt werden
                    if (formVorgabe[v].vorgabeSlot < GameManager.abgabeSlotsFalse)
                    {
                        Debug.Log("VorgabeSlot < AbgabeSlot: " + formVorgabe[v].vorgabeSlot + " - " + abgabeKorrekt);
                        SetAbgabeKorrekt(formVorgabe[v].vorgabeSlot, abgabeKorrekt);
                    }
                    else if (GameManager.abgabeSlotsFalse == 0)
                    {
                        Debug.Log(formVorgabe[v].vorgabeSlot + " - " + abgabeKorrekt);
                        SetAbgabeKorrekt(formVorgabe[v].vorgabeSlot, abgabeKorrekt);
                    }
                    break;
                }
            }
        }
    }

    
   

}
