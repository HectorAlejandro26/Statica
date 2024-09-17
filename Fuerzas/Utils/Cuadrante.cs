namespace Fuerzas.Utils
{
	public enum Cuadrante
	{
		Cero = 0b0000,
		Uno = 0b0001,
		Dos = 0b0010,
		Tres = 0b0011,
		Cuatro = 0b0100,
		EjeX = 0b0101,
		EjeY = 0b0110
	}

	public static class CuadranteExtensions
	{
		public static string Format(this Cuadrante c)
		{
			return c switch
			{
				Cuadrante.Cero => "Origen",
				Cuadrante.Uno => "I",
				Cuadrante.Dos => "II",
				Cuadrante.Tres => "III",
				Cuadrante.Cuatro => "IV",
				Cuadrante.EjeX => "Eje X",
				Cuadrante.EjeY => "Eje Y",
				_ => "",
			};
		}
	}
}
