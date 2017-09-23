namespace diplom
{
    static public class PP
    {
        /// <summary>
        /// Ширина платы
        /// </summary>
        static public int Width { get; set; }
        /// <summary>
        /// Длина платы
        /// </summary>
        static public int Height { get; set; }
        /// <summary>
        /// ДРП
        /// </summary>
        static public int[,] plate { get; set; }

        /// <summary>
        /// Убрать занятые места с платы
        /// </summary>
        public static void Refresh()
        {
            Height = 120;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    plate[i, j] = -1;
        }
    }
}