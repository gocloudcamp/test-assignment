using System.Collections.Generic;
using NAudio.Wave;

namespace PlaylistClass
{
    public class SetMusic<T> : LinkedList<Music>
    {
        private LinkedList<Music> lstMusic { get; set; }


        public SetMusic()
        {
            this.lstMusic = this;
        }

        //
        // Сводка:
        //     Удаляет первое вхождение указанного объекта из коллекции System.Collections.IList.
        //
        // Параметры:
        //   value:
        //     Строка объекта, которая представляет собой часть удалямого объекта.
        //
        public void Remove(string Value)
        {
            Music findElem = this.Find(Value);

            if (findElem != null)
                this.lstMusic.Remove(findElem);
        }

        public new bool Contains(Music elem)
        {
            Music findElem = this.Find(elem.name);

            if (findElem != null)
                return true;
            return false;
        }

        public Music Find(string elem)
        {
            
            foreach (Music file in this.lstMusic)
            {
                if (file.name == elem)
                    return file;
            }
            return null;
        }

        public Music Find(AudioFileReader audio)
        {

            foreach (Music file in this.lstMusic)
            {
                if (file.path == audio.FileName)
                    return file;
            }
            return null;
        }
    }
}
