using System.Collections;
using System.Numerics;
using System.Text;

namespace Algebra
{
    public class Matriz : ICloneable, IEquatable<Matriz>, IEnumerable<double>
    {
        private readonly List<List<double>> values;
        public List<List<double>> Values => values;

        public int NumFilas => values.Count;
        public int NumCols => NumFilas == 0 ? 0 : this[0].Count;
        public int NumElem => NumFilas * NumCols;
        public double Det
        {
            get
            {
				if (!EsCuadrada)
					throw new InvalidOperationException(
						"Operación solo valida con matrices cuadradas.");

				if (NumElem == 1) return this[0, 0];

				else if (NumElem == 4)
					return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];

				else if (NumElem == 9)
					return
						(this[0, 0] * this[1, 1] * this[2, 2] +
						 this[1, 0] * this[2, 1] * this[0, 2] +
						 this[2, 0] * this[0, 1] * this[1, 2]) -
                        (this[0, 2] * this[1, 1] * this[2, 0] +
                         this[1, 2] * this[2, 1] * this[0, 0] +
                         this[2, 2] * this[0, 1] * this[1, 0]);

				else throw new NotImplementedException(
					"Determinante solo valido para matrices 1x1, 2x2 y 3x3.");
			}
        }

        public bool EsCuadrada => NumFilas == NumCols;

        #region Constructores

        public Matriz(int filas, int columnas, double fill = double.NaN)
            : this(filas, columnas, [.. Enumerable.Repeat(fill, filas * columnas)]) { }
        public Matriz(int filas, int columnas, List<double> numbs) : this()
        {
            if (filas <= 0 || columnas <= 0)
            {
                throw new ArgumentException(
                    "Para tener matrices nulas use la propiedad estática \"Matriz.Nula\".");
            }
            if (numbs.Count != filas * columnas)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(numbs),
                    numbs.Count,
                    $"La cantidad de números debe ser igual a la cantidad de espacios({filas * columnas})");
            }

            int c = 0;
            IniciarMatriz(filas, columnas);
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    this[i, j] = numbs[c++];
                }
            }
        }
        private Matriz() => values = [];

        public static Matriz Nula => new();
        public static Matriz DesdeElementos(IEnumerable<IEnumerable<double>> numbs)
        {
            int filas = numbs.Count(), cols = -1;

            if (!numbs.All(e =>
            {
                var n = e.Count();
                if (cols != -1 && cols != n)
                    return false;
                else { cols = n; return true; }
            }))
            {
                throw new ArgumentException(
                    "Las dimensiones de los números no son consistentes.",
                    nameof(numbs));
            }

            List<double> n = [];
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    n.Add(numbs.ElementAt(i).ElementAt(j));
                }
            }
            
            return new(filas, cols, n);
        }

        #endregion

        #region Metodos privados

        private void IniciarMatriz(int f, int c)
        {
            for (int i = 0; i < f; i++)
            {
                values.Add([]);
                for (int j = 0; j < c; j++)
                {
                    this[i].Add(double.NaN);
                }
            }
        }

        private List<double> this[int r]
        {
            get => values[r];
            set
            {
                values[r] = value.Count == NumCols
                    ? value
                    : throw new ArgumentException(
                        "El valor introducido no corresponde a la dimension objetivo.");
            }
        }
        private double this[int r, int c]
        {
            get => values[r][c];
            set => values[r][c] = value;
        }

        #endregion

        #region Metodos publicos

        public static Matriz Multiply(Matriz m1, Matriz m2)
        {
            if (m1.NumCols != m2.NumFilas)
            { 
                throw new InvalidOperationException(
                    "El numero de columnas de la matriz A debe ser igual al" +
                    "numero de filas de la matriz B.");
            }

            List<double> resNumbs = [];

            for (int i = 0; i < m1.NumFilas; i++)
            {
                for (int j = 0; j < m2.NumCols; j++)
                {
                    double res = 0;
                    for (int k = 0; k < m1.NumCols; k++)
                    {
                        res += m1[i, k] * m2[k, j];
                    }
                    resNumbs.Add(res);
                }
            }

            return new(m1.NumFilas, m2.NumCols, resNumbs);
        }
        public static Matriz operator *(Matriz m1, Matriz m2) => Multiply(m1, m2);

        public static Matriz MultiplyByN(Matriz m, double n) => 
            new(m.NumFilas, m.NumCols, ForEachOperation(m, (num, f, c) => num * n));
        public static Matriz operator *(Matriz m, double n) => MultiplyByN(m, n);
        public static Matriz operator *(double n, Matriz m) => MultiplyByN(m, n);

        public static Matriz Sum(Matriz m1, Matriz m2) => new(
            m1.NumFilas,
            m1.NumCols,
            SameSizeOperation(m1, m2, (n1, n2, r, c) => n1 + n2));
        public static Matriz operator +(Matriz m1, Matriz m2) => Sum(m1, m2);

        public static Matriz Sub(Matriz m1, Matriz m2) => new(
            m1.NumFilas,
            m1.NumCols,
            SameSizeOperation(m1, m2, (n1, n2, r, c) => n1 - n2));
        public static Matriz operator -(Matriz m1, Matriz m2) => Sub(m1, m2);

        private delegate double SameSizeOperationHandler(double n1, double n2, int f, int c);
        private delegate double ForEachOperationHandler(double n, int f, int c);
        private static List<double> SameSizeOperation(Matriz m1, Matriz m2, SameSizeOperationHandler op)
        {
            if (m1.NumFilas != m2.NumFilas || m1.NumCols != m2.NumCols)
            {
                throw new InvalidOperationException($"Las dimensiones de las matrices deben ser iguales");
            }
            List<double> numbs = [];

            for (int i = 0; i < m1.NumFilas; i++)
            {
                for (int j = 0; j < m2.NumCols; j++)
                {
                    numbs.Add(op(m1[i][j], m2[i][j], i, j));
                }
            }
            return numbs;
        }
        private static List<double> ForEachOperation(Matriz m, ForEachOperationHandler func)
        {
            List<double> l = [];
            for (int i = 0; i < m.NumFilas; i++)
            {
                for (int j = 0; j < m.NumCols; j++)
                {
                    l.Add(func(m[i][j], i, j));
                }
            }
            return l;
        }


        /* Objetivo
         * ┌   ┐   ┌     ┐
         * │1 2│ & │ 1  2│
         * │3 4│ & │ 3 12│
         * └   ┘   └     ┘
         */
        public override string ToString()
        {
            if (NumElem == 0) return "Ø";

            StringBuilder strOut = new();

            static string GetChars(char c, int count) => string.Concat(Enumerable.Repeat(c, count));
            static string GetNumStr(double n) => n.ToString(n % 1 == 0 ? "0" : "0.##");

            // Caracteres a usar: │┌└┐┘
            const char
                borde = '│',
                esquinaLT = '┌',
                esquinaLB = '└',
                esquinaRT = '┐',
                esquinaRB = '┘';

            int strLarga = 1;
            values.ForEach(l => l.ForEach(n =>
            {
                int lenStr = GetNumStr(n).Length;
                if (strLarga < lenStr) strLarga = lenStr;
            }));

            string espacioL = GetChars(' ', ((strLarga + 1) * NumCols) - 1);

            for (int i = 0; i < NumFilas; i++)
            {
                if (i == 0) strOut.AppendLine($"{esquinaLT}{espacioL}{esquinaRT}");
                for (int j = 0; j < NumCols; j++)
                {
                    // Impresión de borde
                    if (j == 0) strOut.Append(borde);
                        
                    // Numero formateado
                    string numStr = GetNumStr(this[i, j]);

                    // Espacio antes del numero
                    string espacioCB = GetChars(' ', strLarga - numStr.Length + (j == 0 ? 0 : 1));

                    strOut.Append($"{espacioCB}{numStr}");

                    if (j == NumCols - 1) strOut.AppendLine(borde.ToString());
                }
                if (i == NumFilas - 1) strOut.Append($"{esquinaLB}{espacioL}{esquinaRB}");
            }

            return strOut.ToString();
        }

        public object Clone() => DesdeElementos(values);

        public static bool operator ==(Matriz m1, Matriz m2) => m1.Equals(m2);
        public static bool operator !=(Matriz m1, Matriz m2) => !m1.Equals(m2);

		public override bool Equals(object? obj)
		{
			if (obj is not Matriz otherMatriz)
				return false;

			if (NumFilas != otherMatriz.NumFilas || NumCols != otherMatriz.NumCols)
				return false;

			for (int i = 0; i < NumFilas; i++)
			{
				for (int j = 0; j < NumCols; j++)
				{
					if (this[i, j] != otherMatriz[i, j])
						return false;
				}
			}

			return true;
		}
        public override int GetHashCode()
		{
			int hash = 17;

			hash = hash * 31 + NumFilas.GetHashCode();
			hash = hash * 31 + NumCols.GetHashCode();

			foreach (var fila in values)
			{
				foreach (var valor in fila)
				{
					hash = hash * 31 + valor.GetHashCode();
				}
			}

			return hash;
		}

        public bool Equals(Matriz? other) => Equals(other);

		public IEnumerator<double> GetEnumerator()
		{
			for (int i = 0; i < NumFilas; i++)
			{
				for (int j = 0; j < NumCols; j++)
				{
					yield return this[i, j];
				}
			}
		}

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion
	}
}
