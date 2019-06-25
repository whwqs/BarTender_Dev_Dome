using Seagull.BarTender.Print;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarTender_Dev_Dome
{
	public partial class 鼎龙 : Form
	{
		public 鼎龙()
		{
			InitializeComponent();

			Printers printers = new Printers();
			lstBox.Items.Clear();
			foreach (Printer printer in printers)
			{
				lstBox.Items.Add(printer.PrinterName);
			}
		}

		string btwfile;

		string jpgtmp = Path.Combine(Application.StartupPath, "tmp.jpg");

		private void button1_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				btwfile = openFileDialog1.FileName;
				pictureBox1.Image = null;
				string jpgtmp = Path.Combine(Application.StartupPath, "tmp.jpg");
				using (Engine btEngine = new Engine(true))
				{
					LabelFormatDocument labelFormat = btEngine.Documents.Open(btwfile);
					labelFormat.ExportImageToFile(jpgtmp, ImageType.JPEG, Seagull.BarTender.Print.ColorDepth.ColorDepth24bit, new Resolution(800, 600), OverwriteOptions.Overwrite);

					Image image = Image.FromFile(jpgtmp);
					Bitmap NmpImage = new Bitmap(image);
					pictureBox1.Image = NmpImage;
					image.Dispose();				
				}
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			using (Engine btEngine = new Engine(true))
			{
				LabelFormatDocument labelFormat = btEngine.Documents.Open(btwfile);
				try
				{					
					labelFormat.SubStrings.SetSubString("No", txtN.Text);
					labelFormat.SubStrings.SetSubString("日期", txtD.Text);
					labelFormat.SubStrings.SetSubString("重量", txtK.Text);
					labelFormat.SubStrings.SetSubString("条码", txtB.Text);
				}
				catch (Exception ex)
				{
					MessageBox.Show("修改内容出错 " + ex.Message, "操作提示");
					return;
				}

				labelFormat.ExportImageToFile(jpgtmp, ImageType.JPEG, Seagull.BarTender.Print.ColorDepth.ColorDepth24bit, new Resolution(800, 600), OverwriteOptions.Overwrite);

				Image image = Image.FromFile(jpgtmp);
				Bitmap NmpImage = new Bitmap(image);
				pictureBox1.Image = NmpImage;
				image.Dispose();

				if (chkPrint.Checked)
				{
					if (lstBox.SelectedItem == null)
					{
						MessageBox.Show("请选择打印机");
						return;
					}
					string printerName = lstBox.SelectedItem.ToString();

					labelFormat.PrintSetup.PrinterName = printerName;

					string jobName = "BarPrint" + DateTime.Now;

					labelFormat.Print(jobName);
				}
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{

		}
	}
}
