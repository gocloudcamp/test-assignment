package ru.chabndheanee.musicplayerapi.model;

import lombok.Data;

import javax.sound.sampled.*;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.Objects;

//@Slf4j
@Data
public class Track {
    String name;
    Long duration;
    File trackFile;
    private Clip clip;
    private AudioInputStream ais;
    FileInputStream fileInputStream;
    long clipPos = 0;

    private boolean playing = false;

    public Track(File trackFile) throws IOException {
        this.trackFile = trackFile;
        name = trackFile.getName();
        fileInputStream = new FileInputStream(trackFile);
    }

    public void play() {
        try {
            clip = AudioSystem.getClip();
            ais = AudioSystem.getAudioInputStream(trackFile);
            clip.open(ais);
            if (clipPos != 0) {
                clip.setMicrosecondPosition(clipPos);
            }
            clip.start();
        } catch (Exception e) {
            e.getStackTrace();
        }
    }

    public void pause() {
        clipPos = clip.getMicrosecondPosition();
        clip.stop();
        clip.close();
    }

    public void stop() {
        clip.stop();
        clip.close();
    }

    public void setDuration() {
        try {
            duration = Objects.requireNonNull(fileInputStream).getChannel().size() / 128;
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }

    public long getPosition() {
        return clip.getMicrosecondPosition();
    }

    public void setPosition(long position) {
        clipPos = position;
    }
}
