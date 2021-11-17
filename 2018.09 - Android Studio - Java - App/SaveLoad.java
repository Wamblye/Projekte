package com.alchemie;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.files.FileHandle;
import com.badlogic.gdx.utils.Json;
import com.badlogic.gdx.utils.XmlReader;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.Iterator;

/**
 * Created by black_000 on 05.04.2017.
 */

public class SaveLoad {

    int level;
    int gold;
    boolean kaufDeko;


    public SaveLoad() {

    }





    public static Spielstand loadSpielstand() {
        String save = readFile("game.sav");
        if (!save.isEmpty()) {
            ArrayList a = new ArrayList();
            Json json = new Json();
            Spielstand spielStand = json.fromJson(Spielstand.class, save);
            return spielStand;
        }
        return null;
    }

    public static String readFile(String fileName) {
        FileHandle file = Gdx.files.local(fileName);
        if (file != null && file.exists()) {
            String s = file.readString();
            if (!s.isEmpty()) {
                return com.badlogic.gdx.utils.Base64Coder.decodeString(s);
            }
        }
        return "";
    }



    public static void saveSpielstand(int level, int gold) {
        Spielstand spielStand = new Spielstand();


        Json json = new Json();
        writeFile("game.sav", json.toJson(spielStand));
    }

    public static void saveSpielstand(Spielstand spielstand) {
        Spielstand spielStand2 = spielstand;

        Json json = new Json();
        writeFile("game.sav", json.toJson(spielStand2));
    }



    public static void writeFile(String fileName, String s) {
        // internal (Readonly), local (write & load)
        FileHandle file = Gdx.files.local(fileName);
        file.writeString(com.badlogic.gdx.utils.Base64Coder.encodeString(s), false);
    }

    public void ersterStartApp() {
        Spielstand spielStand = new Spielstand();
        /*LevelSterne levelSterne;

        spielStand.level = 1;
        spielStand.sprache = "deutsch";

        for (int i = 0; i < 80; i++) {
            levelSterne = new LevelSterne();
            levelSterne.level = i + 1;
            levelSterne.sterne = 0;
            listLevelSterne.add(levelSterne);
        }

        spielStand.listLevelSterne = listLevelSterne;*/

        Json json = new Json();
        writeFile("game.sav", json.toJson(spielStand));
    }






    public HashSet<XMLelementErgebnisse> readXMLErgebnisse(String xmlName) {
        HashSet<XMLelementErgebnisse> listXMLErgebnisse = new HashSet<XMLelementErgebnisse>();
        XmlReader xml = new XmlReader();
        try {
            XmlReader.Element xml_element = xml.parse(Gdx.files.internal(xmlName));
            Iterator iterator_level = xml_element.getChildrenByName("ball").iterator();

            while (iterator_level.hasNext()) {
                XmlReader.Element level_element = (XmlReader.Element) iterator_level.next();

                XMLelementErgebnisse a = new XMLelementErgebnisse();

                a.ergebnis = (level_element.getAttribute("Ergebnis"));
                a.element1 = (level_element.getAttribute("element1"));
                a.element2 = (level_element.getAttribute("element2"));
                a.region = (Integer.parseInt(level_element.getAttribute("region")));
                listXMLErgebnisse.add(a);
            }
        } catch (Exception e) {

        }
        return listXMLErgebnisse;
    }



    public HashSet<XMLkollision> readXMLkollision(String xmlName) {
        HashSet<XMLkollision> listXMLkollision = new HashSet<XMLkollision>();
        XmlReader xml = new XmlReader();
        try {
            XmlReader.Element xml_element = xml.parse(Gdx.files.internal(xmlName));
            Iterator iterator_level = xml_element.getChildrenByName("ball").iterator();

            while (iterator_level.hasNext()) {
                XmlReader.Element level_element = (XmlReader.Element) iterator_level.next();

                XMLkollision a = new XMLkollision();

                a.spieler = (level_element.getAttribute("spieler"));
                a.gegner = (level_element.getAttribute("gegner"));
                a.schaden = Boolean.parseBoolean(level_element.getAttribute("schaden"));
                listXMLkollision.add(a);
            }
        } catch (Exception e) {

        }
        return listXMLkollision;
    }



    /*public HashSet<XMLAnimation> readXMLAnimation(String xmlName) {
        XmlReader xml = new XmlReader();
        try {
            XmlReader.Element xml_element = xml.parse(Gdx.files.internal(xmlName));
            Iterator iterator_level = xml_element.getChildrenByName("animation").iterator();

            while (iterator_level.hasNext()) {
                XmlReader.Element level_element = (XmlReader.Element) iterator_level.next();

                XMLAnimation a = new XMLAnimation();

                a.name = (level_element.getAttribute("name"));
                a.texture = (level_element.getAttribute("texture"));
                a.b = (Integer.parseInt(level_element.getAttribute("b")));
                a.h = (Integer.parseInt(level_element.getAttribute("h")));
                a.region = (Integer.parseInt(level_element.getAttribute("region")));
                a.rows = (Integer.parseInt(level_element.getAttribute("rows")));
                a.cols = (Integer.parseInt(level_element.getAttribute("cols")));
                a.speed = Float.parseFloat(level_element.getAttribute("speed"));
                listXMLAnimation.add(a);
            }
        } catch (Exception e) {

        }
        return listXMLAnimation;
    }*/


}
