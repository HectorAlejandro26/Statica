using Fuerzas;
using Fuerzas.Utils;

namespace ConsoleCLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
			Console.OutputEncoding = System.Text.Encoding.UTF8;
            const double distancia = 0.03;
            CargaElectrica
                q1 = new(0.000007d),
                q2 = new(-0.000008d),
                q3 = new(-0.000010d);

            Vector
                v1 = q3.Sum(q1, distancia, 180d),
                v2 = q3.Sum(q2, distancia, 300d);

            Console.WriteLine($"{v1}{v1.ToString(VectorStrFormat.Cartesiana)}");
            Console.WriteLine($"{v2}{v2.ToString(VectorStrFormat.Cartesiana)}");
            Console.WriteLine($"{(v1 + v2).ToString(VectorStrFormat.Tabla)}");
        }
    }
}
