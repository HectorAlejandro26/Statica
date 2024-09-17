namespace Fuerzas
{
	public class CargaElectrica(double carga)
	{
		private readonly static long k = 9000000000L;
		public double Carga { get; private set; } = carga;

		public Vector Sum(CargaElectrica other, double distancia, double angulo = 0d) =>
			Sum(this, other, distancia, angulo);

		public static Vector Sum(CargaElectrica q1, CargaElectrica q2, double distancia, double angulo = 0d) => new(
				k * (q1.Carga * q2.Carga / Math.Pow(distancia, 2d)),
				angulo
			);
	}
}
