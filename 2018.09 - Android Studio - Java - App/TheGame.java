package com.alchemie;

import com.badlogic.gdx.Game;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.files.FileHandle;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.scenes.scene2d.InputEvent;
import com.badlogic.gdx.scenes.scene2d.InputListener;
import com.badlogic.gdx.scenes.scene2d.ui.Button;

public class TheGame extends Game {
	SpriteBatch batch;
	Texture img;

    private SaveLoad saveLoad;
    private ScreenLoad screenLoad;
    private ScreenStart screenStart;
    private ScreenGame screenGame;
    private Spielstand spielstand;
    private ScreenLexikon screenLexikon;
    private ScreenLevel1 screenLevel1;
    private ScreenLevel2 screenLevel2;
    private ScreenLevel3 screenLevel3;

	private AdsController adsController;

	public TheGame(AdsController adsController){
		this.adsController = adsController;
	}


	@Override
	public void create () {
        Gdx.app.log("thegame", "create");

        saveLoad = new SaveLoad();

        FileHandle file = Gdx.files.local("game.sav");
        if (file.exists() == false) {
            saveLoad.ersterStartApp();
        }
        // Spielstand laden
        spielstand = saveLoad.loadSpielstand();


        showLoadScreen();
	}




    private void showLoadScreen() {
        setScreen(screenLoad = new ScreenLoad(this, new InputListener() {
            public boolean touchDown(InputEvent event, float x, float y, int pointer, int button) {

                getScreen().dispose();
                Gdx.app.log("thegame", "clicked start");

                return true;
            }
        }));
    }



    public void showStartScreen() {
        setScreen(screenStart = new ScreenStart(new InputListener() {
            public boolean touchDown(InputEvent event, float x, float y, int pointer, int button) {
                Button bStart = screenStart.getButtonStart();
                Button bEndlos = screenStart.getButtonEndlos();
                Button bSpiele = screenStart.getButtonMehrSpiele();

                getScreen().dispose();
                Gdx.app.log("thegame", "clicked start");

                if (bStart.isPressed() == true) {
                    showLevelScreen();
                } /*else if (bShop.isPressed() == true) {
					showShopScreen();
				}*/
                return true;
            }
        }));
    }


    private void showGameScreen(int level) {
        setScreen(screenGame = new ScreenGame(level, this, spielstand, new InputListener() {
            public boolean touchDown(InputEvent event, float x, float y, int pointer, int button) {
                Button bExit = screenGame.getButtonExit();
                int level = screenGame.getLevel();

                getScreen().dispose();
                Gdx.app.log("thegame", "clicked start");

                if (bExit.isPressed() == true) {
                    if (level < 21) {
                        showLevelScreen();
                    } else if (level < 41) {
                        showLevelScreen2();
                    } else {
                        showLevelScreen3();
                    }
                }


                return true;
            }
        }));
    }


    public void showLexikonScreen() {
        setScreen(screenLexikon = new ScreenLexikon(spielstand, new InputListener() {
            public boolean touchDown(InputEvent event, float x, float y, int pointer, int button) {
                Button bExit = screenLexikon.getButtonExit();

                getScreen().dispose();
                Gdx.app.log("thegame", "clicked start");

                if (bExit.isPressed() == true) {
                    showLevelScreen();
                }
                return true;
            }
        }));
    }




}
