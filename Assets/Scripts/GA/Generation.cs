using System;

namespace GeneticAlgorithm
{
    public class Generation
    {
        public Person[] p;
        public static double SumFitness = 0;

        public Generation(int n)
        {
            p = new Person[n];
            for (int i = 0; i < n; i++)
            {
                p[i] = new Person(); //особи
            }
        }

        public void decode()
        {
            for (int i = 0; i < p.Length; i++)
            {
                Decode(p[i]);
            }
        }

        public void decodeTest()
        {
            for (int i = 0; i < p.Length; i++)
            {
                DecodeTest(p[i]);
            }
            if (DataStorage.F1Min < DataStorage.F2Min)
            {
                DataStorage.p2 = DataStorage.F1Min / DataStorage.F2Min;
                DataStorage.p1 = 1 - DataStorage.p2;
            }
            else
            {
                DataStorage.p1 = DataStorage.F2Min / DataStorage.F1Min;
                DataStorage.p2 = 1 - DataStorage.p1;
            }
            DataStorage.p2_start = DataStorage.p2;
            DataStorage.p1_start = DataStorage.p1;
        }

        static int IndexOfMaxKM(Person p1)
        {
            int max = 0;
            int S = 0;
            int index = -1;
            for (int z = 0; z < DataStorage.N; z++)
            {
				if (DataStorage.cm[z].used == 0)
                {
                    S = DataStorage.cm[z].Width * DataStorage.cm[z].Height;
                    if (S > max)
                    {
                        index = z;
                        max = S;
                    }
                }
            }
            return index;
        }

        static int IndexOfMinKM(Person p1)
        {
			int min = int.MaxValue; 
            int S = 0;
            int index = 0;
            for (int z = 0; z < DataStorage.N; z++)
            {
				if (DataStorage.cm[z].used == 0)
                {
                    S = DataStorage.cm[z].Width * DataStorage.cm[z].Height;
                    if (S < min)
                    {
                        index = z;
                        min = S;
                    }
                }
            }
            return index;
        }

        static void Rotate(int index)
        {
            int temp = DataStorage.cm[index].Height;
            DataStorage.cm[index].Height = DataStorage.cm[index].Width;
            DataStorage.cm[index].Width = temp;
        }

        static void FindPlace(int index) //i - текущая особь, index - размещаемый элемент
		{
			bool ok = false; //флаг продолжения работы
			for (int h = 0; h < (PP.Height - DataStorage.cm[index].Height); h++) //ДРП по вертикали
			{
				if (ok) //если элемент размещен
					break; //выход из цикла
				for (int w = 0; w < (PP.Width - DataStorage.cm[index].Width); w++) //по горизонтали
				{
					if (PP.Plate[w, h] == -1) //если не занято
					{
						if (PP.Plate[w, h + DataStorage.cm[index].Height - 1] == -1)
							for (int ht = h; ht < (h + DataStorage.cm[index].Height); ht++) //по вертикали
							{
								if (PP.Plate[w, ht] == -1) //если не занято
									ok = true;
								else //если занято
								{
									ok = false; //дальше не смотреть
									break; //выход из цикла
								}
							}
						else
						{
							ok = false;
						}
						if (ok) //если по вертикали было свободно
						{
							if (PP.Plate[w + DataStorage.cm[index].Width - 1, h] == -1)
								for (int wt = w; wt < (w + DataStorage.cm[index].Width); wt++) //по горизонтали
								{
									if (PP.Plate[wt, h] == -1)
										ok = true;
									else
									{
										ok = false;
										break;
									}
								}
							else
							{
								ok = false;
							}
						}
						if (ok) //если и по горизонтали и по вертикали свободно
						{
                            SetElem(w, h, index);
							break; //выход из цикла
						}
					}
				}
			}
		}

        static void SetElem(int x, int y, int index)
        {
            DataStorage.cm[index].x = x; DataStorage.cm[index].y = y; //задаются координаты элемента

            int max1 = (y + DataStorage.cm[index].Height); //пределы для цикла
            int max2 = (x + DataStorage.cm[index].Width);

            for (int wt = x; wt < max2; wt++)
            {
                for (int ht = y; ht < max1; ht++)
                {
                    PP.Plate[wt, ht] = index; //на ДРП занимается место
                }
            }

            DataStorage.cm[index].used = index + 1; //метка о том, что элемент размещен

            if (DataStorage.cm[index].Height < DataStorage.cm[index].Width) //горизонтально
            {
                DataStorage.cm[index].isVertical = false;
            }
            else
            {
                DataStorage.cm[index].isVertical = true;
            }
        }

        public static void Decode(Person p1)
        {
            int plateHeight = 0;
            PP.Refresh();

            for (int j = 0; j < DataStorage.N; j++)
            {
                DataStorage.cm[j].Refresh();
            }
            int index = -1;
            for (int j = 0; j < DataStorage.N; j++)
            {
                switch (p1.Chromosome.Genes[j]) //декодирование гена и размещение по эвристикам
                {
                    case 1:
                        {
                            index = IndexOfMaxKM(p1); //Самый большой из неразмещенных элементов

                            if (DataStorage.cm[index].Height > DataStorage.cm[index].Width) //горизонтально
                            {
                                Rotate(index);
                            }

                            FindPlace(index); //разместить элемент
                            break;
                        }
                    case 2:
                        {
                            index = IndexOfMaxKM(p1);
                            if (DataStorage.cm[index].Height < DataStorage.cm[index].Width)
                            {
                                Rotate(index);
                            }
                            FindPlace(index);
                            break;
                        }
                    case 3:
                        {
                            index = IndexOfMinKM(p1);
                            if (DataStorage.cm[index].Height > DataStorage.cm[index].Width)
                            {
                                Rotate(index);
                            }
                            FindPlace(index);
                            break;
                        }
                    case 4:
                        {
                            index = IndexOfMinKM(p1);
                            if (DataStorage.cm[index].Height < DataStorage.cm[index].Width)
                            {
                                Rotate(index);
                            }
                            FindPlace(index);
                            break;
                        }
                }
                if (DataStorage.cm[index].isVertical)
                {
                    if (plateHeight < DataStorage.cm[index].y + DataStorage.cm[index].Height)
                        plateHeight = DataStorage.cm[index].y + DataStorage.cm[index].Height;
                }
                else
                {
                    if (plateHeight < DataStorage.cm[index].y + DataStorage.cm[index].Width)
                        plateHeight = DataStorage.cm[index].y + DataStorage.cm[index].Width;
                }
            }
            PP.Height = plateHeight;
            p1.F = F();
            SumFitness += 1 / p1.F;

            if (p1.F < DataStorage.Best)
            {
                DataStorage.Best = p1.F;
                DataStorage.BestPerson = p1;
            }
        }

        static void DecodeTest(Person p1)
        {
            PP.Refresh();

            for (int j = 0; j < DataStorage.N; j++)
            {
                DataStorage.cm[j].Refresh();
            }

            for (int j = 0; j < DataStorage.N; j++)
            {
                switch (p1.Chromosome.Genes[j]) //декодирование гена и размещение по эвристикам
                {
                    case 1:
                        {
                            int index = IndexOfMaxKM(p1); //Самый большой из неразмещенных элементов
                            if (DataStorage.cm[index].Height > DataStorage.cm[index].Width) //горизонтально
                            {
                                Rotate(index);
                            }
                            FindPlace(index); //разместить элемент
                            break;
                        }
                    case 2:
                        {
                            int index = IndexOfMaxKM(p1);
                            if (DataStorage.cm[index].Height < DataStorage.cm[index].Width)
                            {
                                Rotate(index);
                            }
                            FindPlace(index);
                            break;
                        }
                    case 3:
                        {
                            int index = IndexOfMinKM(p1);
                            if (DataStorage.cm[index].Height > DataStorage.cm[index].Width)
                            {
                                Rotate(index);
                            }
                            FindPlace(index);
                            break;
                        }
                    case 4:
                        {
                            int index = IndexOfMinKM(p1);
                            if (DataStorage.cm[index].Height < DataStorage.cm[index].Width)
                            {
                                Rotate(index);
                            }
                            FindPlace(index);
                            break;
                        }
                }
            }
            FMin();
        }


        static double F()
        {
            double F1 = 0, F2 = 0, fx, fy, d; //целевая функция, расстояние по x, расстояние по y
            for (int i = 0; i < DataStorage.N; i++)
                for (int j = i + 1; j < DataStorage.N; j++)
                {
                    fx = Math.Pow(DataStorage.cm[i].x + (double)DataStorage.cm[i].Width / 2 - DataStorage.cm[j].x + (double)DataStorage.cm[j].Width / 2, 2);
                    fy = Math.Pow(DataStorage.cm[i].y + (double)DataStorage.cm[i].Height / 2 - DataStorage.cm[j].y + (double)DataStorage.cm[j].Height / 2, 2);
                    d = Math.Sqrt(fx + fy);
                    if (DataStorage.C[i, j] != 0)
                    {
                        F1 += d * DataStorage.C[i, j];
                    }
                    F2 += d * Math.Abs((DataStorage.cm[i].pwDissipation - DataStorage.cm[j].pwDissipation));
                }
            double F = (DataStorage.p1 * F1 + DataStorage.p2 * F2); // получить аддитивную функцию от двух частных критериев
            return F;
        }


        static void FMin()
        {
            double F1 = 0, F2 = 0, fx, fy, d; //целевая функция, расстояние по x, расстояние по y
            for (int i = 0; i < DataStorage.N; i++)
                for (int j = i + 1; j < DataStorage.N; j++)
                {
                    fx = Math.Pow(DataStorage.cm[i].x + (double)DataStorage.cm[i].Width / 2 - DataStorage.cm[j].x + (double)DataStorage.cm[j].Width / 2, 2);
                    fy = Math.Pow(DataStorage.cm[i].y + (double)DataStorage.cm[i].Height / 2 - DataStorage.cm[j].y + (double)DataStorage.cm[j].Height / 2, 2);
                    d = Math.Sqrt(fx + fy);
                    if (DataStorage.C[i, j] != 0)
                    {
                        F1 += d * DataStorage.C[i, j];
                    }
                    F2 += d * Math.Abs((DataStorage.cm[i].pwDissipation - DataStorage.cm[j].pwDissipation));
                }
            if (F1 < DataStorage.F1Min)
                DataStorage.F1Min = F1;
            if (F2 < DataStorage.F2Min)
                DataStorage.F2Min = F2;
        }

        public void Mutation()
        {
            Random rnd = new Random(); //выбор случайных особей для мутации
            int t;
            for (int z = 0; z < DataStorage.NMut; z++)
            {
                t = rnd.Next(0, p.Length);
                p[t].Chromosome.Mutation();
            }
        }
    }
}