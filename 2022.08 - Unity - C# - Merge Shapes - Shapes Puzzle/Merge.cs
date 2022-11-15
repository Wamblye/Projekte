using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Merge : MonoBehaviour, IDropHandler
{

    private GameManager manager;
    private SpawnPrefab spawnPrefab;
    private ParticleSystem mergeWolke = null;
    private GameObject explosion;
    private GameObject gameObjDrag;

    float sec;
    private Vector2 targetPos1;
    private Vector2 targetPos2;
    private Vector2 _velocity = Vector2.zero;
    private Vector2 _velocity2 = Vector2.zero;
    private float speed = 7;
    private bool merged;
    private int Versatz = 80;

    private int blobbForm = 0;
    private int maxBlobbForm = 6;
    private int schereForm = 300;
    private int zusammenGesetztForm = 1000;
    private int minStandardForm = 1;
    private int maxStandardForm = 12;
    private int minGeschnitteneForm = 13;
    private int maxGeschnitteneForm = 48;
    private int minNewGeschnitteneForm = 37;
    private int maxNewGeschnitteneForm = 48;
    private int minNewForm = 200;
    private int maxNewForm = 205;
    private int minWeissForm = 400;
    private int maxWeissForm = 405;
    private int minSchwarzForm = 500;
    private int maxSchwarzForm = 505;

    private int keinVorgabeSlot = 10;
    private int keinGroupSlot = 10;
    
    private List<FormInfos> form;

    // Form, NewForm
    int[,] arrayMerge = new int[,] { { 1, 9 }, { 2, 7 }, { 3, 10 }, { 4, 11 }, { 5, 8 }, { 6, 12 },
                                     { 7, 200 }, { 8, 201 }, { 9, 202 }, { 10, 204 }, { 11, 203 }, { 12, 205 } };
    // Form, NewForm1, NewForm2
    int[,] arrayMergeSchere = new int[,] { { 1, 13, 14 }, { 2, 15, 16 }, { 3, 17, 18 }, { 4, 19, 20 }, { 5, 21, 22 }, { 6, 23, 24 },
                                           { 7, 25, 26 }, { 8, 27, 28 }, { 9, 29, 30 }, { 10, 31, 32 }, { 11, 33, 34 }, { 12, 35, 36 } };
    int[,] arrayMergeSchereNewFormen = new int[,] { { 200, 38, 37 }, { 201, 39, 40 }, { 202, 42, 41 }, { 203, 43, 44 }, { 204, 46, 45 }, { 205, 48, 47 } };
    // Position in der Group, Form
    int[,] arrayZusammengesetzt = new int[,] { { 1, 1, 2 }, 
                                                { 12, 35, 36 } };
    // Form, NewForm
    int[,] arrayMergeNewFormen = new int[,] { { 7, 200 }, { 8, 201 }, { 9, 202 }, { 10, 204 }, { 11, 203 }, { 12, 205 } };
    // Weiss
    int[,] arrayMergeWeiss = new int[,] { { 400, 1 }, { 401, 2 }, { 403, 3 }, { 404, 4 }, { 405, 5 }, { 406, 6 } };
    // Schwarz
    int[,] arrayMergeSchwarz = new int[,] { { 500, 1 }, { 501, 2 }, { 503, 3 }, { 504, 4 }, { 505, 5 }, { 506, 6 } };

    void Awake()
    {
        manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        spawnPrefab = GameObject.FindWithTag("GameManager").GetComponent<SpawnPrefab>();
        explosion = GameObject.FindWithTag("Explosion");
        mergeWolke = explosion.GetComponent<ParticleSystem>();

        form = new List<FormInfos>();
        merged = false;
    }

    void Update()
    {
        sec -= Time.smoothDeltaTime;
        if (sec >= 0)
        {
            if (merged == true)
            {
                if (gameObject != null)
                {
                    gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.SmoothDamp(gameObject.GetComponent<RectTransform>().anchoredPosition, 
                                        targetPos1, ref _velocity, Time.deltaTime * speed);
                }
                if (gameObjDrag != null)
                {
                    gameObjDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.SmoothDamp(gameObjDrag.GetComponent<RectTransform>().anchoredPosition, 
                                            targetPos2, ref _velocity2, Time.deltaTime * speed);
                }
            }
        }
        else
        {
            merged = false;
        }
    }


    // CanvasCroup muss dem Objekt gegeben werden, dass per Drag and Drop
    // auf DIESES Objekt gezogen wird
    public void OnDrop(PointerEventData eventData)
    {
        int keinDragMehr = 99;
        int keinDragMehrDRAG = 99;

        if(gameObject.transform.childCount > 1) 
        {
            keinDragMehr = int.Parse(gameObject.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text);
        }
        if(eventData.pointerDrag.transform.childCount > 1) 
        {
            keinDragMehrDRAG = int.Parse(eventData.pointerDrag.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text);
        }

		// Nur Formen zusammenfügen, die noch nicht abgegeben wurden
        if (keinDragMehr != 1 && keinDragMehrDRAG != 1)
        {
            gameObjDrag = eventData.pointerDrag;
            form = new List<FormInfos>();
            string spriteNameDrop = eventData.pointerDrag.GetComponent<Image>().sprite.name;
            string spriteNameSlot = GetComponent<Image>().sprite.name;

            SpriteZahlDrop(spriteNameDrop, keinGroupSlot);
            SpriteZahlDrop(spriteNameSlot, keinGroupSlot);

            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            NewMergedForm();
        }
        else if (keinDragMehr == 1)
        {
            Vector2 newPos = new Vector2(eventData.pointerDrag.transform.GetComponent<RectTransform>().anchoredPosition.x, 
                                            eventData.pointerDrag.transform.GetComponent<RectTransform>().anchoredPosition.y - 200);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = newPos;
            Debug.Log("Merge: KeinDragMehr = 1");
        }
    }

    private void SpriteZahlDrop(string gameObjSpriteName, int groupSlot)
    {
        // Erste zeichen im String
        string spriteNameGesamt = gameObjSpriteName;
        int found = spriteNameGesamt.IndexOf("-");
        string textReplace = "-" + spriteNameGesamt.Substring(found + 1);

        string formArt = spriteNameGesamt.Replace(textReplace, "").ToString();
        string farbeM = spriteNameGesamt.Substring(found + 1);

        form.Add(new FormInfos(keinVorgabeSlot, int.Parse(formArt), int.Parse(farbeM), groupSlot));
    }

    private void NewMergedForm()
    {
        if (form[0].form == form[1].form && 
             form[0].form >= minStandardForm && form[0].form <= maxStandardForm &&
             form[1].form >= minStandardForm && form[1].form <= maxStandardForm) // Gleichen Formen Arten (ohne die Geschnittenen Formen 13+)
        {
            Debug.Log("SameForm 1-6, 7-12");
            SameForm();
        }
        else if (form[0].form == schereForm || form[1].form == schereForm) // Schere
        {
            Debug.Log("SchereUndForm");
            SchereUndForm();
        }
        else if (form[0].form >= minGeschnitteneForm && form[0].form <= maxGeschnitteneForm && form[0].form != zusammenGesetztForm && 
                form[1].form >= minGeschnitteneForm && form[1].form <= maxGeschnitteneForm && form[1].form != zusammenGesetztForm) // MergeSameGeschnittenen
        {
            Debug.Log("MergeGeschnittene");
            MergeGeschnittene();
        }
        else if (form[0].form == blobbForm || form[1].form == blobbForm ) // Blobb
        {
            Debug.Log("Blobb");
            Blobb();
        }
        else if (form[0].form >= minWeissForm && form[0].form <= maxWeissForm || 
                form[1].form >= minWeissForm && form[1].form <= maxWeissForm ) // Form Weiss
        {
            Debug.Log("Form Weiss");
            FormWeiss();
        }
        else if (form[0].form >= minSchwarzForm && form[0].form <= maxSchwarzForm || 
                form[1].form >= minSchwarzForm && form[1].form <= maxSchwarzForm ) // Form Schwarz
        {
            Debug.Log("Form Schwarz");
            FormSchwarz();
        }
        else
        {
            Debug.Log("MergeZusammensetzen");
            MergeZusammensetzen();
        }
    }

    private int NewMergeFarbe(int farbe1, int farbe2)
    {
        int newFarbe = 0;

        //Debug.Log("Farbe1: " + farbe1 + " - Farbe2: " + farbe2);

        // 1:Rot, 2:Gelb, 3:Blau, 4:Orange, 5:Grün, 6:Violett
        if (farbe1 == 1 && farbe2 == 2 || farbe1 == 2 && farbe2 == 1)
        {
            newFarbe = 4;
        }
        else if (farbe1 == 1 && farbe2 == 3 || farbe1 == 3 && farbe2 == 1)
        {
            newFarbe = 6;
        }
        else if (farbe1 == 2 && farbe2 == 3 || farbe1 == 3 && farbe2 == 2)
        {
            newFarbe = 5;
        }

        //Debug.Log("Neue Farbe: " + newFarbe);
        return newFarbe;
    }

    private void SameForm()
    {
        if (form[0].form == form[1].form) // Gleichen Formen Arten
        {
            if (form[0].farbe == form[1].farbe) // Selbe Farbe
            {
                Debug.Log("Selbe Farbe");
                if (form[0].form <= 6 && form[1].form <= 6) // Nur die Standard Formen
                {
                    int newForm = arrayMerge[form[0].form -1 , 1];
                    form.Add(new FormInfos(keinVorgabeSlot, newForm, form[0].farbe, keinGroupSlot));

                    spawnPrefab.SpawnPosition(gameObject.GetComponent<RectTransform>().anchoredPosition,
                                manager.FormSprites(form[2].form, form[2].farbe), 0);
                    MergeExplosion();
                    Destroy(gameObjDrag);
                    Destroy(gameObject);
                }
                else if (GameManager.level >= 61) // 7-12 new Forms 200-205
                {
                    int newForm = arrayMergeNewFormen[NewFormIndex(form[0].form), 1];
                    form.Add(new FormInfos(keinVorgabeSlot, newForm, form[0].farbe, keinGroupSlot));

                    spawnPrefab.SpawnPosition(gameObject.GetComponent<RectTransform>().anchoredPosition,
                                manager.FormSprites(form[2].form, form[2].farbe), 0);
                    MergeExplosion();
                    Destroy(gameObjDrag);
                    Destroy(gameObject);
                }
                else
                {
                    KeinMerge();
                }
            }
            else // Neue Misch Farbe - Rot + Gelb = Orange
            {
                Debug.Log("Gemischt");
                int newFarbe = NewMergeFarbe(form[0].farbe, form[1].farbe);
                
                if (newFarbe != 0 && form[0].form <= 6 && form[1].form <= 6)
                {
                    int newForm = arrayMerge[form[0].form -1 , 1];
                    form.Add(new FormInfos(keinVorgabeSlot, newForm, newFarbe, keinGroupSlot));

                    MergeExplosion();
                    spawnPrefab.SpawnPosition(gameObject.GetComponent<RectTransform>().anchoredPosition, 
                                manager.FormSprites(form[2].form, form[2].farbe), 0);
                    Destroy(gameObjDrag);
                    Destroy(gameObject);
                }
                else if (newFarbe != 0 && GameManager.level >= 61) // 7-12 new Forms 200-205
                {
                    Debug.Log("Gemischt New Form");
                    int newForm = arrayMergeNewFormen[NewFormIndex(form[0].form), 1];
                    form.Add(new FormInfos(keinVorgabeSlot, newForm, newFarbe, keinGroupSlot));

                    spawnPrefab.SpawnPosition(gameObject.GetComponent<RectTransform>().anchoredPosition,
                                manager.FormSprites(form[2].form, form[2].farbe), 0);
                    MergeExplosion();
                    Destroy(gameObjDrag);
                    Destroy(gameObject);
                }
                else
                {
                    KeinMerge();
                }
            }
        }
    }

    private void SchereUndForm()
    {
        if (form[0].form == schereForm) // Schere
        {
            if (form[1].form >= minStandardForm && form[1].form <= maxStandardForm ||
                form[1].form >= minNewForm && form[1].form <= maxNewForm) // Keine geschnittenen Formen
            {
                AbwicklungMergeSchere(1);
            }
            else if (form[1].form == zusammenGesetztForm)
            {
                AbwicklungMergeSchereUndZusammengesetzteForm(gameObject);  
            }
            else
            {
                KeinMerge();
            }
        }
        else if (form[1].form == schereForm) // Schere
        {
            if (form[0].form >= minStandardForm && form[0].form <= maxStandardForm ||
                form[0].form >= minNewForm && form[0].form <= maxNewForm) // Keine geschnittenen Formen
            {
                AbwicklungMergeSchere(0);
            }
            else if (form[0].form == zusammenGesetztForm)
            {
                AbwicklungMergeSchereUndZusammengesetzteForm(gameObjDrag);  
            }
            else
            {
                KeinMerge();
            }
        }
    }

    private void AbwicklungMergeSchere(int index)
    {
        float x = gameObject.GetComponent<RectTransform>().anchoredPosition.x;
        float y = gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        float versatzRand = 160;

        Vector2 newPos1 = new Vector2(x + Versatz, y - Versatz);
        Vector2 newPos2 = new Vector2(x - Versatz, y - Versatz);
        Vector2 newPosSchere = new Vector2(x, y + Versatz);

        if (form[index].form >= minNewForm && form[index].form <= maxNewForm) // New Formen 200-205
        {
            Debug.Log("form: " + form[index].form);
            
            string name = form[index].form.ToString();
            string formArt = name.Replace("20", "").ToString();

            int new200Form1 = arrayMergeSchereNewFormen[int.Parse(formArt) , 1];
            int new200Form2 = arrayMergeSchereNewFormen[int.Parse(formArt) , 2];

            form.Add(new FormInfos(keinVorgabeSlot, new200Form1, form[index].farbe, keinGroupSlot));
            form.Add(new FormInfos(keinVorgabeSlot, new200Form2, form[index].farbe, keinGroupSlot));
        }
        else // Formen 1-6
        {
            int newForm1 = arrayMergeSchere[form[index].form -1 , 1];
            int newForm2 = arrayMergeSchere[form[index].form -1 , 2];

            form.Add(new FormInfos(keinVorgabeSlot, newForm1, form[index].farbe, keinGroupSlot));
            form.Add(new FormInfos(keinVorgabeSlot, newForm2, form[index].farbe, keinGroupSlot));
        }

        // Wenn Formen am Rand sind, sollen die neu erzeugten Formen nach oben spawnen, damit
        // sie nicht außerhalb des Bildschirms landen
        if ( x >= 350 && x <= 600 || x >= -600 && x <= -350)
        {
            Debug.Log("im IF: BildschirmRand " + gameObject.GetComponent<RectTransform>().anchoredPosition.x);
            newPos1 = new Vector2(x, y + versatzRand);
            newPos2 = new Vector2(x, y + versatzRand + Versatz);
        }  

        spawnPrefab.SpawnPosition(newPos1, manager.FormSprites(form[2].form, form[2].farbe), 0);
        spawnPrefab.SpawnPosition(newPos2, manager.FormSprites(form[3].form, form[3].farbe), 0);

        if (index == 0)
        {
            MergeExplosion();
            gameObject.GetComponent<RectTransform>().anchoredPosition = newPosSchere;
            Destroy(gameObjDrag);
        } 
        else if (index == 1)
        {
            MergeExplosion();
            gameObjDrag.GetComponent<RectTransform>().anchoredPosition = newPosSchere;
            Destroy(gameObject);
        }
        
        //Debug.Log(newForm1 + " - " + newForm2);
    }

    
    private void KeinMerge()
    {
        Debug.Log("Kein Merge");
        
        Vector2 newPos1 = new Vector2(gameObject.GetComponent<RectTransform>().anchoredPosition.x + Versatz, 
                                        gameObject.GetComponent<RectTransform>().anchoredPosition.y);
        Vector2 newPos2 = new Vector2(gameObject.GetComponent<RectTransform>().anchoredPosition.x - Versatz, 
                                        gameObject.GetComponent<RectTransform>().anchoredPosition.y);

        targetPos1 = newPos1;
        targetPos2 = newPos2;
        merged = true;
        sec = 0.5f;
    }

    private int NewFormIndex(int form)
    {
        int index = 0;

        if (form == 7)
        {
            index = 0;
        }
        else if (form == 8)
        {
            index = 1;
        }
        else if (form == 9)
        {
            index = 2;
        }
        else if (form == 10)
        {
            index = 3;
        }
        else if (form == 11)
        {
            index = 4;
        }
        else if (form == 12)
        {
            index = 5;
        }

        return index;
    }



}
