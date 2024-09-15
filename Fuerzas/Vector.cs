namespace Fuerzas
{
	public class Vector
	{
		private double _magnitud;
		public double Magnitud
		{
			get => _magnitud;
			private set => _magnitud = Math.Abs(value);
		}

		private double _angulo;
		public double Angulo
		{
			get => Rad2Deg(_angulo);
			private set => _angulo = value;
		}

		public double FuerzaX => Math.Cos(_angulo) * Magnitud;
		public double FuerzaY => Math.Sin(_angulo) * Magnitud;

		public Vector(double magnitud, double angulo)
		{
			Magnitud = magnitud;
			Angulo = Deg2Rad(angulo);
		}

		public static Vector DesdeComponentes(double fX, double fY)
		{
			double a = Math.Atan(fY / fX);
			double m = Math.Sqrt(Math.Pow(fX, 2) + Math.Pow(fY, 2));
			

			return new(m, Rad2Deg(a));
		}

		public static double Deg2Rad(double d) => d * (Math.PI / 180);
		public static double Rad2Deg(double d) => d * (180 / Math.PI);

		public static Vector operator +(Vector v1, Vector v2)
		{
			return DesdeComponentes(v1.FuerzaX + v2.FuerzaX, v1.FuerzaY + v2.FuerzaY);
		}
	}
}
