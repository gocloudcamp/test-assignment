using NAudio.Wave;
using PlaylistClass;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace MusicPlayer
{
    public partial class Form1 : Form
    {
        private WaveOutEvent _outputDevice;
        private AudioFileReader _audioFile;
        private Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();
            LoadMusicInList();
        }

        private void OnMouseEnterButton1(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = System.Drawing.Color.DarkSeaGreen;
        }
        private void OnMouseLeaveButton1(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = System.Drawing.Color.Transparent;
        }


        private void Play_Click(object sender, EventArgs e)
        {
            if (music.SelectedItem == null)
                return;

            Music select = this.listMusic.Find(music.SelectedItem.ToString());


            this.button3.Image = Properties.Resources.material_symbols_pause_outline;
            this.Play(select);
        }

        public void Play(Music mp3)
        {
            if (this._outputDevice == null)
            {
                this._outputDevice = new WaveOutEvent();
                //this._outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (this._audioFile == null)
            {
                this._audioFile = new AudioFileReader(mp3.path);
                this._audioFile.Volume = this.trackBar2.Value * 0.01F;

                this._outputDevice.Init(this._audioFile);
            }

            if (mp3.path != this._audioFile.FileName)
            {
                this.PlaybackStop();
                this.Play(mp3);
            }

            this._outputDevice.Play();

            this.TickStart(1000);
            this.TrackBarStart(mp3);

            this.label3.Text = mp3.duration.ToString(@"mm\:ss");
            this.button3.Click -= new System.EventHandler(this.Play_Click);
            this.button3.Click += new System.EventHandler(this.Stop_Click);
        }

        private void TickStart(int interval)
        {
            timer.Tick += new EventHandler(RefreshLabel);
            timer.Interval = interval;
            timer.Start();
        }

        private void TrackBarStart(Music mp3)
        {
            this.trackBar1.Maximum = (int)mp3.duration.TimeOfDay.TotalSeconds;
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            // утилизация данных потока
            this.PlaybackStop();
        }
        private void PlaybackStop()
        {
            this._outputDevice.Dispose();
            this._audioFile.Dispose();

            this._outputDevice = null;
            this._audioFile = null;
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            this.Stop_music();
        }

        private void Stop_music()
        {
            Music stopMp3 = this.listMusic.Find(music.SelectedItem.ToString());
            //try
            //{
            //if (stopMp3.path != this._audioFile.FileName)
                //{
            this._outputDevice?.Stop();
            this.button3.Image = Properties.Resources.ic_sharp_play_arrow__2_;

            this.button3.Click -= new System.EventHandler(this.Stop_Click);
            this.button3.Click += new System.EventHandler(this.Play_Click);
            //}
            //}
            //catch { return; }

            timer.Stop();
        }

        private void ToNext(LinkedListNode<Music> Next)
        {
            if (Next == null)
                return;

            this.Stop_music();
            this.PlaybackStop();

            Music newMP3 =(Next.Value);

            this.music.SelectedItem = newMP3.name;
            this.button3.Image = Properties.Resources.material_symbols_pause_outline;
            this.Play(newMP3);
        }

        private void Prev_Click(object sender, EventArgs e)
        {
            if (music.SelectedItem == null)
                return;

            Music mp3 = this.listMusic.Find(this._audioFile);
            this.ToNext(this.listMusic.Find(mp3).Previous);
        }

        private void Next_Click(object sender, EventArgs e)
        {
            if (music.SelectedItem == null)
                return;

            Music mp3 = this.listMusic.Find(this._audioFile);
            this.ToNext(this.listMusic.Find(mp3).Next);
        }

        public void deleteMusic_Click(object sender, EventArgs e)
        {
            if (this.music.SelectedItem == null)
                return;

            Music stop = this.listMusic.Find(music.SelectedItem.ToString());
            this.delete(stop);
        }

        public void delete(Music stop)
        {
            if (this._audioFile != null && stop.path == this._audioFile.FileName)
            {
                this.Stop_music();
                this.PlaybackStop();
            }

            this.listMusic.Remove(stop.name);
            this.music.Items.Remove(this.music.SelectedItem);
        }

        private void addSong(string fullName, string name, int duration = 0)
        {
            var audio = new AudioFileReader(fullName);
            var newMusic = new Music(fullName, name, audio.TotalTime.ToString());
            

            if (this.listMusic.Contains(newMusic))
                return;

            this.listMusic.AddLast(newMusic);
            this.music.Items.Add(name);
        }

        private void LoadMusicInList()
        {
            var directory = new DirectoryInfo(this.folderMusic.SelectedPath);
            var files = directory.GetFiles("*.*");

            for (int i = 1; i < files.Length; i++)
                this.addSong(files[i].FullName, files[i].Name);
        }

        private void addSong_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string nameMusic = this.openFileDialog.SafeFileName;
                this.addSong(this.openFileDialog.FileName, nameMusic);
            }
        }

        public void RefreshLabel(object sender, EventArgs e)
        {
            this.label2.Text = this._audioFile.CurrentTime.ToString(@"mm\:ss");
            string totalTime = this._audioFile.TotalTime.ToString(@"mm\:ss");

            this.trackBar1.Value = (int)this._audioFile.CurrentTime.TotalSeconds;


            if (this.label2.Text == totalTime)
            {
                Music mp3 = this.listMusic.Find(_audioFile);
                this.ToNext(this.listMusic.Find(mp3).Next);
            }
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            TimeSpan timeFromSec = TimeSpan.FromSeconds(this.trackBar1.Value);
            this.label2.Text = timeFromSec.ToString(@"mm\:ss");

            this._audioFile.CurrentTime = timeFromSec;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {         
            if (this._audioFile == null)
                return;

            this._audioFile.Volume = this.trackBar2.Value * 0.01F;
        }

        private void Exit(object sender, EventArgs e)
        {
            Close();
        }
    }
}
