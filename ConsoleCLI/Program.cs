using Fuerzas;
using Algebra;

namespace ConsoleCLI
{
    internal class Program
    {
        static int Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

#pragma warning disable CS0219, IDE0059
			const int 
                rowsA = 3, columnsA = 3,
                rowsB = 3, columnsB = 3;
#pragma warning restore CS0219, IDE0059

			Matriz A = new(4, 4, GetRandomNumbs(16, -10));

            Console.WriteLine(A);

            return 0;
        }

#pragma warning disable IDE0079
#pragma warning disable IDE0051, IDE0079
		static List<double> GetRandomNumbs(int c, int m = 0, int M = 10)
		{
			M++;
			Random r = new();
			List<double> l = [];

			for (int i = 0; i < c; i++) l.Add(r.Next(m, M));
			return l;
		}
#pragma warning restore IDE0051, IDE0079
	}
}
