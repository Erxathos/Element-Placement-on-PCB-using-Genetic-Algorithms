using System;

namespace GeneticAlgorithm
{
    internal class Chromosome
    {
        int[] a;

        public int[] Genes
        {
            get { return a; }
            set { }
        }

        public Chromosome()
        {
            a = new int[DataStorage.N];
            Random r = new Random();
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = r.Next(1, 5);
            }
        }

        public void Mutation()
        {
            Random rand = new Random();
            int x = rand.Next(0, a.Length - 1); int y = rand.Next(x, a.Length);
            int temp;
            while (x < y)
            {
                temp = a[x];
                a[x] = a[y];
                a[y] = temp;
                y--; x++;
            }
        }
    }
}
