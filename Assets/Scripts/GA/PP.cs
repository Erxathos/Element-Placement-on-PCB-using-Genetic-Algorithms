namespace GeneticAlgorithm
{
    static public class PP
    {
        static public int Width { get; set; }
        static public int Height { get; set; }
        static public int[,] Plate { get; set; }

        public static void Refresh()
        {
            Height = 120;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    Plate[i, j] = -1;
        }
    }
}