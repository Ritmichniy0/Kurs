using System;
using System.Collections;
using System.Collections.Generic;

namespace SudentOOP
{
    class StudyGroup //Учебная группа
    {
        public string Name { get; set; }

        public StudyGroup(string name)
        {
            Name = name;
        }
    }

    class Stream : IEnumerable<StudyGroup> //Поток
    {
        private List<StudyGroup> studyGroups = new List<StudyGroup>();

        public void AddGroup(StudyGroup group) //Добавление ггруппы
        {
            studyGroups.Add(group);
        }
        
        public int Count() //Количество в группе
        {
            return studyGroups.Count;
        }
        public IEnumerator<StudyGroup> GetEnumerator() //Возрощает объект IEnumerator
        {
            return studyGroups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() //IEnumerator
        {
            return GetEnumerator();
        }
    }

    class StreamComparator : IComparer<Stream> //Сравнения количества групп в Потоке
    {
        public int Compare(Stream stream1, Stream stream2)
        {
            return stream1.Count().CompareTo(stream2.Count());
        }
    }

    class StreamService //ПотокСервис
    {
        public void SortStreams(List<Stream> streams)
        {
            streams.Sort(new StreamComparator());
        }
    }

    class Controller //Контроллер
    {
        StreamService streamService = new StreamService();

        public void SortStreams(List<Stream> streams) //Сортировка потока
        {
            streamService.SortStreams(streams);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stream stream1 = new Stream();
            stream1.AddGroup(new StudyGroup("1-11"));
            stream1.AddGroup(new StudyGroup("1-12"));

            Stream stream2 = new Stream();
            stream2.AddGroup(new StudyGroup("2-11"));

            List<Stream> streams = new List<Stream> { stream1, stream2 };

            Controller controller = new Controller();
            controller.SortStreams(streams);

            foreach (var stream in streams)
            {
                Console.Write($"Поток содержит {stream.Count()} групп, а именно |");
                foreach(var people in stream)
                {
                    Console.Write($" {people.Name} |");
                }
                Console.WriteLine();
            }
        }
    }
}
