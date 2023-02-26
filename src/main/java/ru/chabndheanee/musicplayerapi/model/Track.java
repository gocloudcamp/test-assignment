package ru.chabndheanee.musicplayerapi.model;

import lombok.Data;
import lombok.extern.slf4j.Slf4j;

import javax.sound.sampled.*;
import java.io.File;
import java.io.IOException;
import java.util.Map;

@Slf4j
@Data
public class Track {
    String name;
    Long duration;
    File trackFile;
    private Clip clip;
    private AudioInputStream ais;
    private boolean playing = false;

    public Track(File trackFile) throws UnsupportedAudioFileException, IOException {
        name = trackFile.getName();
        AudioFileFormat format = AudioSystem.getAudioFileFormat(trackFile);
        Map<String, Object> properties = format.properties();
        this.duration = (Long) properties.get("duration");
    }

    public void play() {
        try {
            clip = AudioSystem.getClip();
            ais = AudioSystem.getAudioInputStream(trackFile);
            clip.open(ais);
            clip.start();
            playing = true;
        } catch (Exception e) {
            e.getStackTrace();
        }
    }
}
