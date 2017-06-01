/*
 * Creado por SharpDevelop.
 * Usuario: ics_raul
 * Fecha: 21/04/2017
 * Hora: 12:14
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
//using com.itextpdf.text;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
namespace SQLPDF
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class SQLPDF
	{
		protected Dictionary<string, Object> config;
		protected Dictionary<string,Estilo> destatico;
		protected float offset;
		protected float linea;
		protected float y;
		protected int n_tickets_pag;
		
		/// <summary>
		/// Constructor: coge el json parseado y lo guarda como diccionario tambien coge del json:
		/// offset: la distancia desde el origen y de la pagina hasta donde se pone la cabecera
		/// y: la distancia desde el origne y de la pagina hasta donde se pone la informacion
		/// linea: distancia de interlineado
		/// n_tickets_pag: numero de lineas que tiene cada pagina sin contar la cabecera
		/// destatico: diccionario donde para cada texto estatico se guarda su estilo
		/// </summary>
		/// <param name="pdfjson">path donde esta el.json que se parsea</param>
		/// <example>ejemplo de json:
		/// {"offset":{"y":75},
		///   "n_tickets_pag":20,
    	///	  "estatico":[
        ///		{"x":50,"fonttype":"ArialBlack","fontsize":20,"texto":"espectaculo"},
        ///		{"x":150,"fonttype":"ArialBlack","fontsize":20,"texto":"abonado"},
        ///		{"x":270,"fonttype":"ArialBlack","fontsize":20,"texto":"devoluciones"},
        ///		{"x":1000,"fonttype":"ArialBlack","fontsize":20,"texto":"importe"}      
    	///	   ],
    	///	   "offsetres":{"linea":"20","y":100},
    	///	   "espectaculo":{"x":50,"fonttype":"verdana","fontsize":17},
    	///	   "abonado":{"x":150,"fonttype":"verdana","fontsize":17},
		///	   "devoluciones":{"x":270,"fonttype":"verdana","fontsize":17},
    	///	   "importe":{"x":1000,"fonttype":"verdana","fontsize":17}
		///	}
		/// </example>
		public SQLPDF(String pdfjson){
			 config = new Dictionary<string, Object>();
             StreamReader r = new StreamReader(pdfjson);
             string sjson = r.ReadToEnd();   
             
			 System.Web.Script.Serialization.JavaScriptSerializer serializer = new
             System.Web.Script.Serialization.JavaScriptSerializer();             
             config = serializer.Deserialize<Dictionary<string, Object>>(sjson);
             ArrayList estaticos = (ArrayList)config["estatico"];
             destatico= new Dictionary<string, Estilo>();
             foreach(Dictionary<string,Object> estatico in estaticos){
             	Estilo est = new Estilo();
             	est.x = Convert.ToInt32(estatico["x"]);
             	est.fontsize = Convert.ToInt32(estatico["fontsize"]);
             	est.fonttype = estatico["fonttype"].ToString();
             	destatico.Add(estatico["texto"].ToString(),est);
             }
             Dictionary<string,Object> doffset = (Dictionary<string,Object>)config["offset"];
             offset = (float)Convert.ToDouble(doffset["y"].ToString());
             Dictionary<string,Object> offsetres = (Dictionary<string,Object>)config["offsetres"];
             linea = (float)Convert.ToDouble(offsetres["linea"].ToString());
             y = (float)Convert.ToDouble(offsetres["y"].ToString());
             n_tickets_pag = Convert.ToInt32(config["n_tickets_pag"].ToString());
             
             
			
		}
		
		 
		
		/// <summary>
		/// Esta funcion busca la fuente de un texto estatico que pone nombre
		/// </summary>
		/// <param name="nombre"> texto del campo estatico del que queremos saber la fuente</param>
		/// <returns></returns>
		public System.Drawing.Font encontrar_font(string nombre){
			Estilo est = destatico[nombre];
			return new System.Drawing.Font(est.fonttype, est.fontsize);
			
		}
		/// <summary>
		/// Esta funcion busca el campo nombre en el json parseado y dentro de este busca la fuente
		/// </summary>
		/// <param name="nombre">nombre del campo del json del que queremos saber su fuente</param>
		/// <returns></returns>
		public System.Drawing.Font encontrar_font_row(string nombre){
			Dictionary<string,Object> estilo = (Dictionary<string,Object>)config[nombre];
			return new System.Drawing.Font(estilo["fonttype"].ToString(), (float)Convert.ToDouble(estilo["fontsize"].ToString()));
			
		}
		
		/// <summary>
		/// Esta funcion busca que x tiene el campo estatico con texto nombre
		/// </summary>
		/// <param name="nombre">nombre del campo del json del que queremos saber la x</param>
		/// <returns></returns>
		public float encontrar_x_row(string nombre){
			Dictionary<string,Object> estilo = (Dictionary<string,Object>)config[nombre];
			return (float)Convert.ToDouble(estilo["x"].ToString());
		}
		/// <summary>
		/// Esta funcion nos dice en que x tiene que ir el texto estatico nombre
		/// </summary>
		/// <param name="nombre">texto del campo estatico del que queremos saber su x</param>
		/// <returns></returns>
		public float encontrar_x(string nombre){
			Estilo est = destatico[nombre];
			return est.x;
		}
		
		/// <summary>
		/// La funcion que hace la exportacion a pdf
		/// </summary>
		/// <param name="dt">Tabla que vamos a printar en pdf</param>
		/// <param name="path">path donde se guardara el pdf final</param>
		public void ExportToPdf(DataTable dt,string path)
   		{
			/*Abrimos un fichero de tipo pdf*/			
			Document document = new Document(PageSize.A4.Rotate());    		
    		PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
    		document.Open();
    		/*Cargamos como imagen la que tenemos como plantilla*/
    		System.Drawing.Image gif = System.Drawing.Image.FromFile("0001.png");    		
			Graphics graphics = Graphics.FromImage(gif);
			printar_cabecera(graphics,dt);
			SolidBrush drawBrush = new SolidBrush(Color.Black);
			int lineaact=0;
			/*Para cada la printamos en la x que se nos pasa el json y como y la linea en la que 
			 * esta por el interlineado mas la distancia desde donde comienza a 
			 * printarse la informacion
			 */
    		foreach (DataRow r in dt.Rows)
    		{
        		if (dt.Rows.Count > 0)
        		{
        			int i=0;
        			foreach (DataColumn c in dt.Columns)
    				{
        				graphics.DrawString(r[i].ToString(), encontrar_font_row(c.ColumnName),drawBrush ,encontrar_x_row(c.ColumnName),(lineaact*linea)+y);
        				i++;
        			}
        			/*si es la ultima linea guardamos la imagen y cogemos como nueva imagen
        			 * la de la plantilla y le printamos la cabecera.Sino decimos que la linea actual para la proxima iteración será está mas uno
        			 */
        			if(lineaact==n_tickets_pag){
        				lineaact=0;
        				document.Add(iTextSharp.text.Image.GetInstance(gif,System.Drawing.Imaging.ImageFormat.Gif));
        				gif = System.Drawing.Image.FromFile("0001.png");
    					graphics = Graphics.FromImage(gif);
    					printar_cabecera(graphics,dt);
        				
        			}
        			else{
        				lineaact++;
        			}
        		}          
    		} 
			
			
    		/*añadimos la ultima imagen y cerramos el documento*/
			document.Add(iTextSharp.text.Image.GetInstance(gif,System.Drawing.Imaging.ImageFormat.Gif));
        	document.Close();
		}
		/// <summary>
		/// Printa la cabecera en la imagen
		/// </summary>
		/// <param name="graphics">el componente para printar en la imagen</param>
		/// <param name="dt">el DataTable con los datos</param>
		public void printar_cabecera(Graphics graphics, DataTable dt){
			SolidBrush drawBrush = new SolidBrush(Color.Black);
    		foreach (DataColumn c in dt.Columns)
    		{
    			
    			graphics.DrawString(c.ColumnName, encontrar_font(c.ColumnName),drawBrush ,encontrar_x(c.ColumnName),offset);
    			//table.AddCell(new Phrase(c.ColumnName, encontrar_font(c.ColumnName)));
    		}
		}
	}
	
}