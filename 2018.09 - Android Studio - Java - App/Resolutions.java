package com.alchemie;

import com.badlogic.gdx.graphics.Camera;
import com.badlogic.gdx.utils.Array;
import com.badlogic.gdx.utils.Scaling;
import com.badlogic.gdx.utils.viewport.ExtendViewport;
import com.badlogic.gdx.utils.viewport.FillViewport;
import com.badlogic.gdx.utils.viewport.FitViewport;
import com.badlogic.gdx.utils.viewport.ScalingViewport;
import com.badlogic.gdx.utils.viewport.ScreenViewport;
import com.badlogic.gdx.utils.viewport.StretchViewport;
import com.badlogic.gdx.utils.viewport.Viewport;

/**
 * Created by black_000 on 04.04.2017.
 */

public class Resolutions {

    private static final int VIRTUAL_WIDTH = 640;
    private static final int VIRTUAL_HEIGHT = 480;

    public int getVirtualWidth() {
        return VIRTUAL_WIDTH;
    }
    public int getVirtualHeight() {
        return VIRTUAL_HEIGHT;
    }

    public Resolutions() {}

    static public Array<Viewport> getViewports (Camera camera) {
        int minWorldWidth = 640;
        int minWorldHeight = 480;
        int maxWorldWidth = 800;
        int maxWorldHeight = 480;

        Array<Viewport> viewports = new Array();
        viewports.add(new StretchViewport(minWorldWidth, minWorldHeight, camera));
        viewports.add(new FillViewport(minWorldWidth, minWorldHeight, camera));
        viewports.add(new FitViewport(minWorldWidth, minWorldHeight, camera));
        viewports.add(new ExtendViewport(minWorldWidth, minWorldHeight, camera));
        viewports.add(new ExtendViewport(minWorldWidth, minWorldHeight, maxWorldWidth, maxWorldHeight, camera));
        viewports.add(new ScreenViewport(camera));

        ScreenViewport screenViewport = new ScreenViewport(camera);
        screenViewport.setUnitsPerPixel(0.75f);
        viewports.add(screenViewport);

        viewports.add(new ScalingViewport(Scaling.none, minWorldWidth, minWorldHeight, camera));
        return viewports;
    }


}
