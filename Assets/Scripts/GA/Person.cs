namespace diplom
{
    public class Person
    {
        Chromosome chromosome;
        public double F { get; set; }
        public bool isSelected { get; set; }

        internal Chromosome Chromosome
        {
            get { return chromosome; }
            set { chromosome = value; }
        }

        public Person()
        {
            chromosome = new Chromosome();
        }
    }
}
