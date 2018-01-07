using System;
using System.Diagnostics;

namespace GeneticAlgorithm
{
    public class Main : UnityEngine.MonoBehaviour
    {
        Generation g1, g2, g3;

        void NewGeneration()
        {
            OutbreedingCross(); //Proceed crossingover
            Generation.SumFitness = 0;
            //Combine populations
            for (int i = 0; i < DataStorage.NPer; i++)
            {
                g3.p[i] = g1.p[i];
            }
            for (int i = 0; i < DataStorage.NCross; i++)
            {
                g3.p[i + DataStorage.NPer] = g2.p[i];
            }
            
            g3.Mutation(); 
            g3.decode(); //Decode all chromosomes
            Selection(); //Selection
        }

        private void TestAlg()
        {
            Generation test = new Generation(10);
            test.decodeTest();
        }

        public System.Collections.IEnumerator MainCoroutine()
        {
            yield return null;
            Stopwatch sWatch = new Stopwatch();
            sWatch.Start();

            DataStorage.Reload();

            TestAlg(); //make a test generation to estimate fitness functions

            g1 = new Generation(DataStorage.NPer);
            g2 = new Generation(DataStorage.NCross);
            g3 = new Generation(DataStorage.NPer + DataStorage.NCross);

            double temp = DataStorage.Best, before = DataStorage.Best;

            for (int i = 2, cnt = 1, waitFor = 20; cnt < waitFor; i++) //цикл while с объявлением переменных
            {
                NewGeneration(); //create a new generation

                if (temp > DataStorage.Best) //if this solution is better
                {
                    temp = DataStorage.Best; //save this solution
                    cnt = 1;
                }

                else cnt++;
                //if (cnt == waitFor)
                //    IndexBest = i - cnt; //convergence of the algorithm
            }
            Generation.Decode(DataStorage.BestPerson); //decode the best solution
            sWatch.Stop();

            View.DebugText("Action took " + (sWatch.ElapsedMilliseconds / 1000) + " sec");

            if (before != DataStorage.Best)
                Messenger.Broadcast(GameEvent.GUI_UPDATED);
        }

        void OutbreedingCross() //make a crossingover between maximal different persons
        {
            for (int i = 0; i < DataStorage.NCross / 2; i++)
            {
                int index = 0;
                System.Random r = new System.Random();
                int x = r.Next(0, DataStorage.NPer);
                for (int j = 0; j < DataStorage.NPer; j++)
                {
                    int cnt = 0, maxdif = 0;
                    for (int z = 0; z < DataStorage.N; z++)
                    {
                        if (g1.p[x].Chromosome.Genes[z] != g1.p[j].Chromosome.Genes[z])
                            cnt++;
                    }
                    if (cnt > maxdif)
                    {
                        maxdif = cnt;
                        index = j;
                    }
                }
                Crossingover(g1.p[x], g1.p[index], out g2.p[i], out g2.p[i + DataStorage.NCross / 2]);
            }
        }

        void Crossingover(Person _1, Person _2, out Person p1, out Person p2)
        {
            int a = 0; //starting point in the chromosome
            int x = DataStorage.N - 3; //максимальный номер гена для первой точки скрещивания
            int x1 = UnityEngine.Random.Range(a, x); //случайный номер первого гена
            p1 = new Person(); p2 = new Person();
            for (int j = 0; j < 3; j++)
            {
                for (int i = a; i < x1; i++)
                {
                    if ((j % 2) == 0)
                    {
                        //Crossingover
                        p1.Chromosome.Genes[i] = _1.Chromosome.Genes[i];
                        p2.Chromosome.Genes[i] = _2.Chromosome.Genes[i];
                    }
                    else
                    {
                        p1.Chromosome.Genes[i] = _2.Chromosome.Genes[i];
                        p2.Chromosome.Genes[i] = _1.Chromosome.Genes[i];
                    }
                }
                a = x1; //сдвиг начала вложенного цикла
                x++; //максимальный номер гена для следующей точки скрещивания
                x1 = UnityEngine.Random.Range(a, x); //следующая случайная точка скрещивания
            }
        }

        void Selection() //roulette selection
        {
            foreach (Person p1 in g3.p)
            {
                p1.isSelected = false;
            }
            bool loop = true;
            for (int i = 0; i < DataStorage.NPer; i++)
            {
                while (loop)
                {
                    double slice = UnityEngine.Random.Range(0.0f, 1.0f) * Generation.SumFitness;
                    double curFitness = 0.0;
                    foreach (Person p1 in g3.p)
                    {
                        curFitness += 1 / p1.F;
                        if (curFitness >= slice && !p1.isSelected)
                        {
                            loop = false;
                            g1.p[i] = p1;

                            p1.isSelected = true;
                            Generation.SumFitness = Generation.SumFitness - p1.F;

                            break;
                        }
                    }
                }
            }
        }
    }
}