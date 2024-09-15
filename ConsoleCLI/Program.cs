using Fuerzas;

namespace ConsoleCLI
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Vector vector = new Vector(850, 27) + new Vector(300, 185) + new Vector(75, 300);
			Console.WriteLine($"Magnitud: {vector.Magnitud}\nAngulo: {vector.Angulo}");
			Console.WriteLine($"Fx: {vector.FuerzaX}\nFy: {vector.FuerzaY}");
		}
	}
}
