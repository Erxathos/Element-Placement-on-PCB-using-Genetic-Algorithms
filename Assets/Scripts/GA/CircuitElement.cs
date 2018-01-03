using UnityEngine;

namespace GeneticAlgorithm
{
    public class CircuitElement
    {
        /// <summary>
        /// Получает или задает длину элемента
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Получает или задает ширину элемента
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Получает или задает координату X элемента
        /// </summary>
        public int x { get; set; }
        /// <summary>
        /// Получает или задает координату Y элемента
        /// </summary>
        public int y { get; set; }
        /// <summary>
        /// Получает или задает название элемента на схеме
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Получает название 3D модели для отрисовки
        /// </summary>
        public string CaseName { get { return caseName; } }
        /// <summary>
        /// Получает или задает название компонента
        /// </summary>
        public string ElementName { get; set; }

        public int used { get; set; }
        public bool isVertical { get; set; }
        public float pwDissipation { get; set; }

        string caseName;

        public CircuitElement(string Name, int Width, int Height, string CaseName, string ElementName, float pwDissipation)
        {
            this.Name = Name;

            int offset;
            if (pwDissipation > 10)
                offset = 10;
            else offset = 3;
            this.Width = (int)(Width + offset);
            this.Height = (int)(Height + offset);
            this.caseName = CaseName;
            this.ElementName = ElementName;
            this.pwDissipation = pwDissipation;
        }

        public void Refresh()
        {
            used = 0;
            isVertical = false;
        }
    }
}
