package ru.chabndheanee.musicplayerapi.model.player;

import lombok.extern.slf4j.Slf4j;
import ru.chabndheanee.musicplayerapi.model.Track;

import java.io.File;
import java.io.IOException;
import java.util.LinkedList;

@SuppressWarnings("deprecation")
@Slf4j
//@Data
public class Player {
    final LinkedList<Track> playlist = new LinkedList<>();
    Track track;
    int currentTrack = 0;
    SongThread thread = new SongThread();


    public void play() {
        track = playlist.get(currentTrack);

        track.play();
        track.setDuration();

        thread = new SongThread();
        thread.setDuration(track.getDuration());
        thread.start();
    }

    public void pause() {
        track.pause();
        thread.stop();
    }

    private void stop() {
        track.stop();
        thread.stop();
    }

    public void next() {
        currentTrack++;
        stop();
        play();
    }

    public void prev() {
        if (track.getPosition() > 10000000) {
            pause();
            track.setPosition(0);
            play();
            return;
        }
        currentTrack--;
        stop();
        play();
    }

    public void addSong(String path) {
        try {
            Track track = new Track(new File(path));
            playlist.add(track);
        } catch (IOException e) {
            e.getStackTrace();
        }
    }
}

class SongThread extends Thread {
    long duration = 0;

    public void setDuration(long duration) {
        this.duration = duration;
    }

    @Override
    public void run() {
        try {
            Thread.sleep(duration);
            Controller.next();
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
    }
}
