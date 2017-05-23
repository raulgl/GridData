/*
 * Creado por SharpDevelop.
 * Usuario: ics_raul
 * Fecha: 16/05/2017
 * Hora: 13:40
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;

namespace SQLPDF
{
	/// <summary>
	/// Description of Estilo.
	/// </summary>
	public class Estilo
	{
		protected int pos;
		protected string fuente;
		protected int size;
		public Estilo()
		{
		}
		public int x{
			get{
				return pos;
			}
			set{
				pos=value;
			}
		}
		public string fonttype{
			get{
				return fuente;
			}
			set{
				fuente=value;
			}
		}
		public int fontsize{
			get{
				return size;
			}
			set{
				size=value;
			}
		}
	}
}
