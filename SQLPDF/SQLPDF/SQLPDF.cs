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
		public System.Drawing.Font encontrar_font(string nombre){
			Estilo est = destatico[nombre];
			return new System.Drawing.Font(est.fonttype, est.fontsize);
			
		}
		public System.Drawing.Font encontrar_font_row(string nombre){
			Dictionary<string,Object> estilo = (Dictionary<string,Object>)config[nombre];
			return new System.Drawing.Font(estilo["fonttype"].ToString(), (float)Convert.ToDouble(estilo["fontsize"].ToString()));
			
		}
		public float encontrar_x_row(string nombre){
			Dictionary<string,Object> estilo = (Dictionary<string,Object>)config[nombre];
			return (float)Convert.ToDouble(estilo["x"].ToString());
		}
		
		public float encontrar_x(string nombre){
			Estilo est = destatico[nombre];
			return est.x;
		}
		public void ExportToPdf(DataTable dt,string path)
   		{      
			Document document = new Document(PageSize.A4.Rotate());
    		
    		PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
    		document.Open();
            iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5);

    		PdfPTable table = new PdfPTable(dt.Columns.Count);
    		PdfPRow row = null;
    		/*float[] widths = new float[] { 4f, 4f, 4f, 4f };

    		table.SetWidths(widths);*/
    		

    		table.WidthPercentage = 100;
    		int iCol = 0;
    		string colname = "";
    		PdfPCell cell = new PdfPCell(new Phrase("Products"));

    		cell.Colspan = dt.Columns.Count;
    		System.Drawing.Image gif = System.Drawing.Image.FromFile("0001.png");
    		
			Graphics graphics = Graphics.FromImage(gif);
			printar_cabecera(graphics,dt);
			SolidBrush drawBrush = new SolidBrush(Color.Black);
    		foreach (DataColumn c in dt.Columns)
    		{
    			
    			graphics.DrawString(c.ColumnName, encontrar_font(c.ColumnName),drawBrush ,encontrar_x(c.ColumnName),offset);
    			//table.AddCell(new Phrase(c.ColumnName, encontrar_font(c.ColumnName)));
    		}
			int lineaact=0;
    		foreach (DataRow r in dt.Rows)
    		{
        		if (dt.Rows.Count > 0)
        		{
        			int i=0;
        			foreach (DataColumn c in dt.Columns)
    				{
        				//table.AddCell(new Phrase(r[i].ToString(), encontrar_font_row(c.ColumnName)));
        				graphics.DrawString(r[i].ToString(), encontrar_font_row(c.ColumnName),drawBrush ,encontrar_x_row(c.ColumnName),(lineaact*linea)+y);
        				i++;
        			}
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
			
			
    		
			document.Add(iTextSharp.text.Image.GetInstance(gif,System.Drawing.Imaging.ImageFormat.Gif));
    		//document.Add(table);
        	document.Close();
		}
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