using System;
using System.Collections.Generic;

namespace diplom
{
    public static class DataStorage
    {
        /*КОНСТРУКТИВНЫЕ МОДУЛИ*/
        /// <summary>
        /// Количество элементов
        /// </summary>
        static public int N;
        /// <summary>
        /// Массив элементов для размещения
        /// </summary>
        static public CircuitElement[] km { get; set; }
        /// <summary>
        /// Матрица смежности
        /// </summary>
        static public int[,] C { get; set; }
        /// <summary>
        /// Список названий моделей элементов для отрисовки
        /// </summary>
        static public List<string> caseNames = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        static public int CurrentProject;
        static public List<string> ProjectNames = new List<string>();
        static public int CurrentPlate;
        static public List<List<int>> PlateNumbers = new List<List<int>>();

        /*НАСТРОЙКИ ГЕНЕТИЧЕСКОГО АЛГОРИТМА*/
        /// <summary>
        /// Количество особей в популяции
        /// </summary>
        static public int NPer = 20;
        /// <summary>
        /// Вероятность скрещивания
        /// </summary>
        static public double PCross = 0.5;
        /// <summary>
        /// Вероятность мутации
        /// </summary>
        static public double PMut = 0.1; 
        /// <summary>
        /// Количество потомков
        /// </summary>
        static public int NCross = Convert.ToInt32(NPer * PCross);
        /// <summary>
        /// Количество мутируемых особей
        /// </summary>
        static public int NMut = Convert.ToInt32(NPer * PMut);
        /// <summary>
        /// Лучшее значение фитнесс функции
        /// </summary>
        static public double Best = double.MaxValue;
        /// <summary>
        /// Индекс лучшего поколения для получения скорости сходимости алгоритма
        /// </summary>
        static public int IndexBest { get; set; }
        /// <summary>
        /// Лучшая особь для декодирования
        /// </summary>
        static public Person BestPerson { get; set; }

        static public void Reload()
        {
            NCross = Convert.ToInt32(NPer * PCross);
            NMut = Convert.ToInt32(NPer * PMut);
            IndexBest = 0;
            BestPerson = null;
            Best = double.MaxValue;
        }

        public static double F1Min = double.MaxValue, F2Min = double.MaxValue;

        public static double p2_start { get; set; }
        public static double p1_start { get; set; }

        public static double p2 { get; set; }
        public static double p1 { get; set; }
    }
}
