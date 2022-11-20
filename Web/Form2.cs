using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Web
{
    public partial class Form2 : Form
    {
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        SpeechSynthesizer voz;
        List<VoiceInfo> vozes = new List<VoiceInfo>();
        String[] se;
        List<string> titles = new List<string>();
        List<string> articles = new List<string>();
        int here;
        DataTable tab;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DBLIB.bd db = new DBLIB.bd("server=.;database=hello;integrated security=true");
           tab= db.get("SELECT TOP (8) [titulo],[texto],[id]  FROM [hello].[dbo].[web3] order by id desc");

            dataGridView1.DataSource = tab;
            RempliCombo();
            Choices choi = new Choices();
            choi.Add(new string[] { "comenzar","siguiente", "stop", "pausar","detener" ,"continuar", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho" });
            Grammar gr = new Grammar(new GrammarBuilder(choi));
            try
            {
                rec.SetInputToDefaultAudioDevice();
                rec.LoadGrammar(gr);
                rec.SpeechRecognized += rec_speech;
                rec.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception)
            {

                throw;
            }
            foreach (DataRow item in tab.Rows)
            {
                string a = item["titulo"].ToString();
                string b = item["texto"].ToString();
               // MessageBox.Show(b);
                titles.Add(a);
                articles.Add(b);
            }
        }

        private void rec_speech(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Text == "comenzar")
            {
               
                button1_Click(null, null);
            }
            if (e.Result.Text == "uno")
            {

               lire(articles[0]);
                here = 1;

            }
            if (e.Result.Text == "dos")
            {

                lire(articles[1]);
                here = 2;


            }
            if (e.Result.Text == "tres")
            {

                lire(articles[2]);
                here = 3;


            }
            if (e.Result.Text == "cuatro")
            {

                lire(articles[3]);
                here = 4;


            }
            if (e.Result.Text == "cinco")
            {

                lire(articles[4]);
                here = 5;


            }
            if (e.Result.Text == "seis")
            {

                lire(articles[5]);
                here = 5;

            }
            if (e.Result.Text == "siete")
            {

                lire(articles[6]);
                here = 6;

            }
            if (e.Result.Text == "ocho")
            {

                lire(articles[7]);
                here = 8;

            }

            if (e.Result.Text == "pausar")
            {
                button2_Click(null, null);
            }
            if (e.Result.Text == "detener")
            {
                button3_Click(null, null);
            }
            if (e.Result.Text == "siguiente")
            {
                button4_Click(null, null);
            }


        }


        private void RempliCombo()
        {
            voz = new SpeechSynthesizer();

            foreach (var x in voz.GetInstalledVoices())
            {
                vozes.Add(x.VoiceInfo);
                comboBox1.Items.Add(x.VoiceInfo.Name);

            }
        }



        private void lire(String a)
        {
            try
            {

                if (voz != null) voz.Dispose();

                voz = new SpeechSynthesizer();
                double volume = volumen.Value;
                double frq = trackBar2.Value;
                voz.SelectVoice(comboBox1.Text);
                voz.Volume = (int)volume;
                voz.Rate = (int)frq;
                voz.SpeakAsync(a);

            }
            catch (Exception)
            {

                //throw;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tit="";
            for (int i=0;i<titles.Count;i++)
            {
                tit += i + 1 + "  "+titles[i]+"\n";
            }
          lire(tit);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {


                if (voz != null)
                {
                    if (voz.State == SynthesizerState.Speaking)
                    {
                        voz.Pause(); button2.Text = "Continue";
                    }
                    else
                    {
                        voz.Resume(); button2.Text = "Pause";


                    }
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {


                if (voz != null)
                {
                    voz.Dispose();
                }
            }
            catch (Exception)
            {


            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            switch (here)
            {
                case 1: lire(articles[1]); here = 2; break;
                case 2: lire(articles[2]); here = 3; ; break;
                case 3: lire(articles[3]); here = 4; ; break;
                case 4: lire(articles[4]); here = 5; ; break;
                case 5: lire(articles[5]); here = 6; ; break;
                case 6: lire(articles[6]); here = 7; ; break;
                case 7: lire(articles[7]); here = 8; ; break;
                case 8: lire(articles[0]); here = 1; ; break;

                default:
                    break;
            }

        }
    }
}
