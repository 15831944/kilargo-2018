using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kilargo
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public partial class ProductDetails : Form
    {
        System.Windows.Forms.Form frmProgress = new System.Windows.Forms.Form();
        PictureBox loading = new PictureBox();
        Label lblStatus = new Label();
        string urlRevit = @"http://kilargo.designcontent.com.au/Kilargo2014Families/";
        Image imgname = kilargo.Properties.Resources.loading_gif;
        Image fileName = kilargo.Properties.Resources.loginbgkilargo;

        //string imgname = (System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase), "Formbg", "loading.gif")).Replace("file:\\", "");
        //string fileName = (System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase), "Formbg", "loginbgkilargo.jpg")).Replace("file:\\", "");

        string path2;

        public ProductDetails(ExternalCommandData commadet, ref string messdet, Autodesk.Revit.DB.ElementSet eledet)
        {
            InitializeComponent();


            properities.tempCommandData = commadet;
            properities.tempElementSet = eledet;
            properities.tempMessage = messdet;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }




        private void ProductDetails_Load(object sender, EventArgs e)
        {


            detailsimage.ImageLocation = null;
            productName.Text = properities.RevitFilePath;
            prodescription.Text = properities.fileDescription;
            detailsimage.ImageLocation = @"http://kilargo.designcontent.com.au/Kilargo2014Families/" + properities.RevitFilePath + ".png";


            properities.RevitFileName = urlRevit + properities.RevitFilePath+".rfa";


            if (properities.RevitFilePath == null)
            {
                lblRevitFileName.Text = "File is not available";
                button1.Enabled = false;
                dload.Enabled = false;
            }
            else
            {
               
                RemoteFileExists(properities.RevitFileName);

                lblRevitFileName.Text = properities.RevitFilePath + ".rfa"; 
                button1.Enabled = true;
                dload.Enabled = true;
            }



        }

        private void dload_Click(object sender, EventArgs e)
        {

            
            String fname = String.Empty;
            if (Directory.Exists(@"C:/Temp"))
            {
                string[] filePaths = Directory.GetFiles(@"c:\Temp\", "*.rfa", SearchOption.AllDirectories);
                fname = properities.RevitFilePath.ToString();

              

                foreach (string path in filePaths)
                {
                    string path1;
                    path1 = path;
                    int s = path1.LastIndexOf("\\");
                    path1 = path1.Remove(0, s + 1);
                    path1 = path1.Replace(".rfa", "");
                    if (path1 == fname)
                    {
                        path2 = path1;
                        break;

                    }
                }
            }

            if (path2 == fname)
            {

                DialogResult dialogResult = MessageBox.Show("File Already Exists! \n Are you sure you wish to overwrite existing file?", "Caroma", MessageBoxButtons.YesNo);


                if (dialogResult == DialogResult.Yes)
                {

                    properities.download = true;
                    frmProgress = new System.Windows.Forms.Form();
                    loading = new PictureBox();
                    loading.BackColor = System.Drawing.Color.Transparent; 
                    lblStatus = new Label();

                    frmProgress.StartPosition = FormStartPosition.CenterScreen;
                    //frmProgress.Size = new Size(PictureViewmage.Image.Size.Width, PictureViewmage.Image.Size.Height);
                    frmProgress.Size = new Size(150, 75);
                    frmProgress.FormBorderStyle = FormBorderStyle.None;

                    frmProgress.BackgroundImage = fileName;
                   // frmProgress.BackgroundImage = Image.FromFile(fileName);
                    frmProgress.BackgroundImageLayout = ImageLayout.Stretch;
                    loading.Size = new Size(40, 40);

                    loading.Image = new Bitmap(kilargo.Properties.Resources.loading_gif);
                    loading.SizeMode = PictureBoxSizeMode.StretchImage;
                    loading.Location = new System.Drawing.Point(58, 10);
                    lblStatus.Text = "Loading...";
                    lblStatus.Font = new Font("Tahoma", 12);
                    lblStatus.Location = new System.Drawing.Point(40, 50);
                    lblStatus.ForeColor = System.Drawing.Color.FromArgb(84, 85, 89);
                    lblStatus.BackColor = System.Drawing.Color.Transparent;

                    frmProgress.Size = new Size(150, 75);
                    frmProgress.Controls.Add(loading);
                    frmProgress.Controls.Add(lblStatus);

                    frmProgress.Load += new EventHandler(frmProgress_Load);
                    frmProgress.ShowDialog();
                    //do something
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
            else
            {


                properities.download = true;
                frmProgress = new System.Windows.Forms.Form();
                loading = new PictureBox();
                lblStatus = new Label();
                loading.BackColor = System.Drawing.Color.Transparent; 
                frmProgress.StartPosition = FormStartPosition.CenterScreen;
                //frmProgress.Size = new Size(PictureViewmage.Image.Size.Width, PictureViewmage.Image.Size.Height);
                frmProgress.Size = new Size(150, 75);
                frmProgress.FormBorderStyle = FormBorderStyle.None;

                frmProgress.BackgroundImage = fileName;
                frmProgress.BackgroundImageLayout = ImageLayout.Stretch;
                loading.Size = new Size(30, 30);

                loading.Image = new Bitmap(kilargo.Properties.Resources.loading_gif);
                loading.SizeMode = PictureBoxSizeMode.StretchImage;
                loading.Location = new System.Drawing.Point(60, 10);
                lblStatus.Text = "Loading...";
                lblStatus.Font = new Font("Tahoma", 12);
                lblStatus.Location = new System.Drawing.Point(40, 50);
                lblStatus.ForeColor = System.Drawing.Color.FromArgb(84, 85, 89);
                lblStatus.BackColor = System.Drawing.Color.Transparent;

                frmProgress.Size = new Size(150, 75);
                frmProgress.Controls.Add(loading);
                frmProgress.Controls.Add(lblStatus);

                frmProgress.Load += new EventHandler(frmProgress_Load);
                frmProgress.ShowDialog();
                //do something



            }
        }

     






        private void frmProgress_Load(object sender, EventArgs e)
        {
            //String RevitFilePath = String.Empty;
            //System.Web.HttpUtility.UrlEncode(string url);
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            string fname = properities.RevitFilePath.ToString();

            //int k = fname.LastIndexOf("/");
            //fname = fname.Remove(0, k + 1);
            //fname = fname.Replace("%20", " ");
            //fname = fname.Replace(".rfa", "");
            properities.filename = fname;

            String pathrft = @"http://kilargo.designcontent.com.au/Kilargo2014Families/" + properities.RevitFilePath + ".rfa";
            properities.RevitFileName = pathrft;
            webClient.DownloadFileAsync(new Uri(pathrft), @"c:\Temp\" + fname + ".rfa");
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);


        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //bar.Value = e.ProgressPercentage;
        }



        public void Completed(object sender, AsyncCompletedEventArgs e)
        {
            lblStatus.Text = "Completed";
            frmProgress.Refresh();
            System.Threading.Thread.Sleep(2000);
            string FamilyPath = Path.GetFullPath(@"c:\Temp\" + properities.filename + ".rfa");
            frmProgress.Close();
            properities.minwindow = true;  
            this.Hide();
     
            }





        private bool RemoteFileExists(string url)
        {
            try
            {

                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TURE if the Status code == 200
               
                Int64 value = response.ContentLength;
                response.Close();
                String Size = SizeSuffix(value);
                revitfilesize.Text = Size;


                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }




        private void button1_Click(object sender, EventArgs e)
        {
            if (properities.RevitFilePath == null)
            {
                button1.Enabled = false;
                lblRevitFileName.Text = "File is not available";
            }
            else
            {
                button1.Enabled = true;
                lblRevitFileName.Text = properities.RevitFilePath;
                var dialog = new SaveFileDialog();
                dialog.Filter = "Revit (*.rfa)|*.rfa";

                String Str = Path.ChangeExtension(properities.RevitFilePath, null);
                dialog.FileName = Str;

                var result = dialog.ShowDialog(); //shows save file dialog
                if (result == DialogResult.OK)
                {
                    // Console.WriteLine("writing to: " + dialog.FileName); //prints the file to save
                    if (RemoteFileExists(properities.RevitFileName))
                    {
                        var wClient = new WebClient();
                        wClient.DownloadFile(properities.RevitFileName, dialog.FileName);
                        this.Close();
                    }

                    else
                    {
                        MessageBox.Show(Str + " File Does Not Exist.");
                    }
                }
            }
        }










        
    }
}
