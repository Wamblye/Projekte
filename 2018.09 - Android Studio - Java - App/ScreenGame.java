package com.alchemie;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Animation;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.math.Vector3;
import com.badlogic.gdx.scenes.scene2d.InputEvent;
import com.badlogic.gdx.scenes.scene2d.InputListener;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.ui.Button;
import com.badlogic.gdx.scenes.scene2d.ui.Table;
import com.badlogic.gdx.scenes.scene2d.utils.ClickListener;
import com.badlogic.gdx.scenes.scene2d.utils.TextureRegionDrawable;
import com.badlogic.gdx.utils.Array;
import com.badlogic.gdx.utils.viewport.Viewport;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.Random;

/**
 * Created by black_000 on 05.04.2017.
 */

public class ScreenGame extends Resolutions implements Screen {


    private final Texture texHintergrund;
    private Texture texLebenspunkteSpieler;

    private Texture texFeuer;
    private Texture texWasser;

    private Texture texPlus;
    private Texture texGleich;
    private Texture texBox;
    private Texture texTransparent;

    private Texture texStab;

    private Sprite spriteElement1;
    private Sprite spriteElement2;
    private Sprite spriteElementErgebnis;

    private boolean element1Clicked = false;
    private boolean element2Clicked = false;

    private Button buttonExit;
    private Button buttonHerz;
    private Button buttonFeuer;
    private Button buttonBlatt;
    private Button buttonStein;
    private Button buttonWasser;
    private Button buttonBlitz;
    private Button buttonWind;
    private Button buttonFinsternis;
    private Button buttonLicht;

    private Texture texHerz;
    private Texture texElemente;
    private Texture texElemente2;
    private Texture texElemente3;
    private Texture texElemente4;
    private Texture texElemente5;
    private TextureRegion[] regionsElemente = new TextureRegion[42];

    private ShapeRenderer shapeRendererSpieler = new ShapeRenderer();
    private ShapeRenderer shapeRendererGegner = new ShapeRenderer();

    private TextureRegion[] regionKuh = new TextureRegion[2];
    private Animation animKuhRechts;
    private Animation animKuhLinks;
    private float stateTime;

    private Stage stage;
    private SpriteBatch batch;
    private final OrthographicCamera camera;
    private Array<Viewport> viewports;

    private HashSet<XMLelementErgebnisse> listErgebnisse = new HashSet<XMLelementErgebnisse>();
    private HashSet<ElementErgebnisse> listMoveErgebnisse = new HashSet<ElementErgebnisse>();
    private HashSet<ElementErgebnisse> listMoveErgebnisseGegner = new HashSet<ElementErgebnisse>();
    private HashSet<XMLkollision> listKollision = new HashSet<XMLkollision>();
    //private String[][] array = new String[44][44]{{"1"},{"b"}};
    //private String[][] array = new String[44][44];

    private ArrayList<Integer> arrayGegnerRegions = new ArrayList<Integer>();
    private Array<String> arrayGegnerName = new Array<String>(44);



    private Array<String> arrayElemente = new Array<String>(2);

    private float time;
    private float timeEffekt;
    private final int VIRTUAL_WIDTH;
    private final int VIRTUAL_HEIGHT;

    private boolean feuer = false;
    private boolean blatt = false;
    private boolean stein = false;
    private boolean wasser = false;
    private boolean blitz = false;
    private boolean wind = false;
    private boolean finsternis = false;
    private boolean licht = false;

    private Table table;
    private Table table2;
    private Table table3;

    private InputListener inputListener;

    private Random rnd = new Random();

    private int RAND_OBEN = 510;
    private int RAND_UNTEN = 120;
    private int RAND_RECHTS = 370;
    private int RAND_LINKS = 40;

    private int anzahlElementVariation;
    private int land;
    private int level;
    private int lebenspunkteSpieler;
    private int lebenspunkteGegner;

    private Spielstand spielstand;
    private SaveLoad saveLoad;
    private ScreenStart screenStart;
    private TheGame game;


    public ScreenGame(int lev, TheGame theGame, Spielstand spielstand, InputListener listener) {
        inputListener = listener;
        this.spielstand = spielstand;
        saveLoad = new SaveLoad();
        game = theGame;
        level = lev;

        VIRTUAL_WIDTH = getVirtualWidth();
        VIRTUAL_HEIGHT = getVirtualHeight();

        camera = new OrthographicCamera(VIRTUAL_WIDTH, VIRTUAL_HEIGHT);
        camera.setToOrtho(false, VIRTUAL_WIDTH, VIRTUAL_HEIGHT);
        stage = new Stage();
        Gdx.input.setInputProcessor(stage);
        batch = new SpriteBatch();

        loadTexElementeRegion();
        levels();

        texHintergrund = new Texture(Gdx.files.internal("hintergrund_game.png"));
        texLebenspunkteSpieler = new Texture(Gdx.files.internal("lebenspunkte_anzeige.png"));


        texPlus = new Texture(Gdx.files.internal("plus.png"));
        texGleich = new Texture(Gdx.files.internal("gleich.png"));
        texBox = new Texture(Gdx.files.internal("box.png"));

        texHerz = new Texture(Gdx.files.internal("icon_herz.png"));

        texTransparent = new Texture(Gdx.files.internal("transparent.png"));

        texStab = new Texture(Gdx.files.internal("stab_drache.png"));

        spriteElement1 = new Sprite(texTransparent);
        spriteElement2 = new Sprite(texTransparent);
        spriteElementErgebnis = new Sprite(texTransparent);

        texFeuer = new Texture(Gdx.files.internal("icon_feuer_rechts.png"));
        texWasser = new Texture(Gdx.files.internal("icon_welle.png"));

        loadXMLErgebnisse();
        loadXMLkollision();

        //loadAnimations();
        land = 1;
        lebenspunkteSpieler = 187;
        lebenspunkteGegner = 187;



        loadElemente();
        addErgebnisListGegner();


        table = new Table();
        table.setFillParent(true);


        /*Button.ButtonStyle buttonStyle10 = new Button.ButtonStyle();
        buttonStyle10.up = new TextureRegionDrawable(new TextureRegion(new Texture(Gdx.files.internal("icon_button_herz_neu.png"))));
        //buttonStyle9.up = new TextureRegionDrawable(new TextureRegion(new Texture(Gdx.files.internal("button_x.png"))));
        buttonHerz = new Button(buttonStyle10);
        buttonHerz.addListener(new ClickListener() {
            @Override
            public void clicked(InputEvent event, float x, float y) {
                if (element1Clicked == false) {
                    spriteElement1 = new Sprite(texHerz);
                    spriteElement2 = new Sprite(texTransparent);
                    spriteElementErgebnis = new Sprite(texTransparent);
                    element1Clicked = true;
                    arrayElemente.add("herz");
                } else {
                    spriteElement2 = new Sprite(texHerz);
                    element2Clicked = true;
                    arrayElemente.add("herz");
                }
            }

            ;
        });*/

        ohneKombination();

        if (level < 3 || level > 20 && level < 23) {
            table.add(buttonFeuer).padTop(400).padLeft(0).size(70, 70);
            table.add(buttonBlatt).padTop(400).padLeft(0).size(70, 70);
            stage.addActor(table);
        } else if (level < 5 || level >= 23 && level < 25) {
            table.add(buttonFeuer).padTop(400).padLeft(0).size(70, 70);
            table.add(buttonBlatt).padTop(400).padLeft(0).size(70, 70);
            table.add(buttonStein).padTop(400).padLeft(0).size(70, 70);
            stage.addActor(table);
        } 

        Table table3 = new Table();
        table3.setFillParent(true);

        Table table4 = new Table();
        table4.setFillParent(true);


        Table tableExit = new Table();
        tableExit.setFillParent(true);

        Button.ButtonStyle buttonStyleExit = new Button.ButtonStyle();
        buttonStyleExit.up = new TextureRegionDrawable(new TextureRegion(new Texture(Gdx.files.internal("button_klein_abbrechen.png"))));
        //buttonStyle9.up = new TextureRegionDrawable(new TextureRegion(new Texture(Gdx.files.internal("button_x.png"))));
        buttonExit = new Button(buttonStyleExit);
        buttonExit.addListener(inputListener);

        tableExit.add(buttonExit).padTop(-435).padLeft(595).size(40, 40);
        stage.addActor(tableExit);

        //table2.add(buttonMoreGames).padTop(300).padLeft(0).size(350, 70);
        //stage.addActor(table2);

        // Damit auf allen Aufloesungen immer gleich aussieht
        viewports = getViewports(stage.getCamera());
        stage.setViewport(viewports.first());

    }


    public Button getButtonExit()
    {
        return buttonExit;
    }
    public int getLevel()
    {
        return level;
    }


    @Override
    public void render(float v) {
        float t = Gdx.graphics.getDeltaTime(); // seconds
        time += t;
        timeEffekt += t;
        stateTime += Gdx.graphics.getDeltaTime();

        stage.act();
        camera.update();

        Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);
        batch.begin();
        batch.setProjectionMatrix(stage.getCamera().combined);

        batch.draw(texHintergrund, 0, 0, VIRTUAL_WIDTH, VIRTUAL_HEIGHT);

        batch.end();

        shapeRendererSpieler.begin(ShapeRenderer.ShapeType.Filled);
        shapeRendererSpieler.setProjectionMatrix(stage.getCamera().combined);
        //shapeRendererSpieler.setColor(Color.GREEN);
        shapeRendererSpieler.setColor(89/255f, 219/255f, 117/255f, 1);
        shapeRendererSpieler.rect(96, 150, lebenspunkteSpieler, 22);
        shapeRendererSpieler.end();


        batch.begin();
        batch.draw(texLebenspunkteSpieler, 90, 145, 200, 30);
        batch.draw(texLebenspunkteSpieler, 350, 145, 200, 30);

        batch.draw(texBox, 130, 70, 70, 70);
        batch.draw(texPlus, 220, 80, 50, 50);

        if (element2Clicked == true) {
            showErgebnis();

        }

        moveErgebnis();
        deleteErgebnis();

        moveErgebnisGegner();

        kolision();

        batch.end();

        stage.draw();

    }



    private void ohneKombination() {
        if (level >= 1 && level <= 20) {

            Button.ButtonStyle buttonStyle9 = new Button.ButtonStyle();
            buttonStyle9.up = new TextureRegionDrawable(new TextureRegion(new Texture(Gdx.files.internal("icon_button_feuer_neu.png"))));
            //buttonStyle9.up = new TextureRegionDrawable(new TextureRegion(new Texture(Gdx.files.internal("button_x.png"))));
            buttonFeuer = new Button(buttonStyle9);
            buttonFeuer.addListener(new ClickListener() {
                @Override
                public void clicked(InputEvent event, float x, float y) {
                    if (element1Clicked == false) {
                        spriteElement1 = new Sprite(regionsElemente[7]);
                        spriteElement2 = new Sprite(texTransparent);
                        spriteElementErgebnis = new Sprite(texTransparent);
                        element1Clicked = true;
                        feuer = true;
                        arrayElemente.add("feuer");
                    } else if (blatt == false
                            && stein == false
                            && wasser == false
                            && blitz == false
                            && wind == false
                            && finsternis == false
                            && licht == false) {
                        spriteElement2 = new Sprite(regionsElemente[7]);
                        element2Clicked = true;
                        arrayElemente.add("feuer");
                    }
                }

                ;
            });


            Button.ButtonStyle buttonStyle2 = new Button.ButtonStyle();
            buttonStyle2.up = new TextureRegionDrawable(new TextureRegion(new Texture(Gdx.files.internal("icon_button_blatt_neu.png"))));
            //buttonStyle9.up = new TextureRegionDrawable(new TextureRegion(new Texture(Gdx.files.internal("button_x.png"))));
            buttonBlatt = new Button(buttonStyle2);
            buttonBlatt.addListener(new ClickListener() {
                @Override
                public void clicked(InputEvent event, float x, float y) {
                    if (element1Clicked == false) {
                        spriteElement1 = new Sprite(regionsElemente[1]);
                        spriteElement2 = new Sprite(texTransparent);
                        spriteElementErgebnis = new Sprite(texTransparent);
                        element1Clicked = true;
                        blatt = true;
                        arrayElemente.add("blatt");
                    } else if (feuer == false
                            && stein == false
                            && wasser == false
                            && blitz == false
                            && wind == false
                            && finsternis == false
                            && licht == false) {
                        spriteElement2 = new Sprite(regionsElemente[1]);
                        element2Clicked = true;
                        arrayElemente.add("blatt");
                    }
                }

                ;
            });
        } else {
            Button.ButtonStyle buttonStyle10 = new Button.ButtonStyle();
            buttonStyle10.up = new TextureRegionDrawable(new TextureRegion(new Texture(Gdx.files.internal("icon_button_herz_neu.png"))));
            //buttonStyle9.up = new TextureRegionDrawable(new TextureRegion(new Texture(Gdx.files.internal("button_x.png"))));
            buttonHerz = new Button(buttonStyle10);
            buttonHerz.addListener(new ClickListener() {
                @Override
                public void clicked(InputEvent event, float x, float y) {
                    if (element1Clicked == false) {
                        spriteElement1 = new Sprite(texHerz);
                        spriteElement2 = new Sprite(texTransparent);
                        spriteElementErgebnis = new Sprite(texTransparent);
                        element1Clicked = true;
                        arrayElemente.add("herz");
                    } else {
                        spriteElement2 = new Sprite(texHerz);
                        element2Clicked = true;
                        arrayElemente.add("herz");
                    }
                }

                ;
            });
        }
    }



    private void showErgebnis() {
        String element1 = "";
        String element2 = "";
        int i = 0;

        for (String s : arrayElemente) {
            if (i == 0) {
                element1 = s;
            } else {
                element2 = s;
            }
            i++;
        }

        for (XMLelementErgebnisse e : listErgebnisse) {
            if (e.element1.equals(element1) && e.element2.equals(element2) || e.element1.equals(element2) && e.element2.equals(element1)) {
                spriteElementErgebnis = new Sprite(regionsElemente[e.region]);
                addErgebnisList(e.region, e.ergebnis);
            }
        }

        arrayElemente.clear();
        element1Clicked = false;
        element2Clicked = false;

        feuer = false;
        blatt = false;
    }



    private void moveErgebnis() {
        for (ElementErgebnisse e : listMoveErgebnisse) {
            if (e.name.contains("feuerBlatt") == false && e.name.contains("blitzStein") == false) {
                e.x += 1;
            }
            batch.draw(regionsElemente[e.regionElement], e.x, e.y, e.b, e.h);

            if (e.x > 640 || e.name.contains("feuerBlatt") == true && timeEffekt > 3 || e.name.contains("blitzStein") == true && timeEffekt > 3) {
                e.delete = true;
            }

        }
    }

    private void moveErgebnisGegner() {
        for (ElementErgebnisse e : listMoveErgebnisseGegner) {
            if (time >= 5 && e.erschienen == false) {
                e.erschienen = true;
                time = 0;
                break;
            }
        }

        for (ElementErgebnisse e : listMoveErgebnisseGegner) {
            if (e.erschienen == true) {
                if (e.name.contains("feuerBlatt") == false && e.name.contains("blitzStein") == false) {
                    e.x -= 1;
                }
                batch.draw(regionsElemente[e.regionElement], e.x + e.b, e.y, -e.b, e.h);

                if (e.x < 0 || e.name.contains("feuerBlatt") == true && timeEffekt > 3 || e.name.contains("blitzStein") == true && timeEffekt > 3) {
                    e.delete = true;
                }
            }
        }
    }


    private void deleteErgebnis() {
        for (ElementErgebnisse e : listMoveErgebnisse) {
            if (e.delete == true) {
                listMoveErgebnisse.remove(e);
                break;
            }
        }
    }



    private void kolision() {
        boolean delete = false;

        for (ElementErgebnisse e : listMoveErgebnisseGegner) {
            if (e.erschienen == true) {
                for (ElementErgebnisse a : listMoveErgebnisse) {
                    if (e.x + 5 >= a.x + a.b && e.x - 5 <= a.x + a.b) {
                        for (XMLkollision k : listKollision) {
                            if (a.name.contains(k.spieler) == true && e.name.contains(k.gegner) == true) {
                                delete = true;
                                lebenspunkteGegner -= 10;
                                if (lebenspunkteGegner < 0) {
                                    lebenspunkteGegner = 0;
                                }
                                break;
                            }
                        }
                        if (delete == true) {
                            listMoveErgebnisse.remove(a);
                            listMoveErgebnisseGegner.remove(e);
                        }
                        //delete = true;
                        break;
                    }
                }

                // 10.10.17
                if (e.x < 80) {
                    listMoveErgebnisseGegner.remove(e);
                    delete = true;

                    lebenspunkteSpieler -= 10;
                    if (lebenspunkteSpieler < 0) {
                        lebenspunkteSpieler = 0;
                        // GAME OVER
                    }
                }

            }
            if (delete == true) {
                break;
            }
        }
    }


    public void touch () {
        if (Gdx.input.isTouched()) {
            // camera.setToOrtho(false, VIRTUAL_WIDTH, VIRTUAL_HEIGHT) setzen !!!
            Vector3 t = new Vector3(Gdx.input.getX(), Gdx.input.getY(), 0);
            camera.unproject(t);

            /*if (t.x > 350 && t.x < 480 && t.y > 390 && t.y < 640) {
                spielstand.gold += 100;
                saveLoad.saveSpielstand(spielstand);
            } else if (t.x > 350 && t.x < 480 && t.y > 320 && t.y < 370) {
                if (spielstand.gold >= 100) {
                    spielstand.gold -= 100;
                    saveLoad.saveSpielstand(spielstand);
                }
            }*/
        }
    }


    private void loadXMLErgebnisse() {
        listErgebnisse = saveLoad.readXMLErgebnisse("elementErgebnisse.xml");
    }

    private void loadXMLkollision() {
        listKollision = saveLoad.readXMLkollision("kollision.xml");
    }


    private void loadAnimations() {
        Texture texture2 = new Texture(Gdx.files.internal("spritesheet_kuh.png"));
        TextureRegion[][] tmp2 = TextureRegion.split(texture2, 400, 300);

        TextureRegion[] regionKuhRechts = new TextureRegion[6];
        TextureRegion[] regionKuhLinks = new TextureRegion[6];

        for (int zeile = 0; zeile < 5; zeile++) {
            for (int spalte = 0; spalte < 6; spalte++) {
                if (zeile == 0 && spalte == 0) {
                    regionKuh[spalte] = tmp2[zeile][spalte];
                } else if (zeile == 0 && spalte == 1) {
                    regionKuh[spalte] = tmp2[zeile][spalte];
                } else if (zeile == 1 && spalte < 6) {
                    regionKuhRechts[spalte] = tmp2[zeile][spalte];
                } else if (zeile == 2 && spalte < 6) {
                    regionKuhLinks[spalte] = tmp2[zeile][spalte];
                }
            }
        }
        animKuhRechts = new Animation(1f/7f, regionKuhRechts);
        animKuhLinks = new Animation(1f/7f, regionKuhLinks);
    }



    @Override
    public void resize(int width, int height) {
        stage.getViewport().update(width, height, true);
    }

    @Override
    public void show() {

    }

    @Override
    public void hide() {

    }

    @Override
    public void pause() {

    }

    @Override
    public void resume() {

    }

    @Override
    public void dispose() {
        texHintergrund.dispose();

        table.remove();

        batch.dispose();
        stage.dispose();
        Gdx.app.log("startscreen","dispose");
    }






}
