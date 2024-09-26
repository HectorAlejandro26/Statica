using Fuerzas.Utils;

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

        public Vector VUnitario
        {
            get
            {
                var v = DesdeComponentes(FuerzaX / _magnitud, FuerzaY / _magnitud);
                return v.Magnitud == 1 ? v : new(double.NaN, double.NaN);
            }
        }

        public double FuerzaX => Math.Cos(_angulo) * Magnitud;
        public double FuerzaY => Math.Sin(_angulo) * Magnitud;
        public Cuadrante Cuadrante
        {
            get
            {
                if      (FuerzaX == 0d && FuerzaY == 0d) return Cuadrante.Cero;
                else if (        FuerzaX == 0d         ) return Cuadrante.EjeY;
                else if (        FuerzaY == 0d         ) return Cuadrante.EjeX;
                else if (FuerzaX >= 0d && FuerzaY >= 0d) return Cuadrante.Uno;
                else if (FuerzaX <  0d && FuerzaY >= 0d) return Cuadrante.Dos;
                else if (FuerzaX <  0d && FuerzaY <  0d) return Cuadrante.Tres;
                else                                     return Cuadrante.Cuatro;
            }
        }

        public Vector(double magnitud, double angulo)
        {
            Magnitud = magnitud;
            Angulo = Deg2Rad(angulo);
        }

        public static Vector DesdeComponentes(double fX, double fY) => new(
            Math.Sqrt(Math.Pow(fX, 2) + Math.Pow(fY, 2)),
            Rad2Deg(Math.Atan(fY / fX))
        );

        public static double Deg2Rad(double d) => d * (Math.PI / 180);
        public static double Rad2Deg(double d) => d * (180 / Math.PI);

        public static Vector Sum(Vector v1, Vector v2) => DesdeComponentes(v1.FuerzaX + v2.FuerzaX, v1.FuerzaY + v2.FuerzaY);
        public static Vector Sub(Vector v1, Vector v2) => DesdeComponentes(v1.FuerzaX - v2.FuerzaX, v1.FuerzaY - v2.FuerzaY);

        public static Vector operator +(Vector v1, Vector v2) => Sum(v1, v2);
        public static Vector operator -(Vector v1, Vector v2) => Sub(v1, v2);

        public string ToString(VectorStrFormat f)
        {
			return f switch
			{
				VectorStrFormat.Cartesiana => $"({FuerzaX:0.##}, {FuerzaY:0.##})",
				VectorStrFormat.Tabla => 
                                $" V = {ToString(VectorStrFormat.Polar)}\n" +
								$"Qᶜᵗ: {Cuadrante.Format()}\n" +
								$"  ɤ: {Magnitud}\n" +
								$"  θ: {Angulo}\n" +
								$" 𝐹x: {FuerzaX}\n" +
								$" 𝐹y: {FuerzaY}\n",
				_ => $"{Magnitud:0.##}∠{Angulo:0.##}°",
			};
		}
        public override string ToString() => ToString(VectorStrFormat.Polar);
    }

    public enum VectorStrFormat
    {
        Polar,
        Cartesiana,
        Tabla
    }
}
