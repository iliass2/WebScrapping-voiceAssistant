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
using System.Data.Linq;
using System.Speech.Synthesis;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using DBLIB;
using System.Speech.Recognition;
namespace Web
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        SpeechSynthesizer voz;
        List<VoiceInfo> vozes = new List<VoiceInfo>(); 
        String[] se;
        List<string> titles=new List<string>();
        List<string> articles = new List<string>();
        List<string> urls = new List<string>();
        int here;
        public Form1()
        {
            InitializeComponent();
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


        private IEnumerable<String> get_titles()
        {
            WebClient a = new WebClient();
            var res = a.DownloadString($"https://www.bbc.com/mundo");
            using (var reader = new StringReader(res))
            {
                List<String> s = new List<string>();
                List<String> sf = new List<string>();
                //  String[] se;
                string line;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        if (line.Contains("promo-top-stories-"))
                        {
                            // char[] hy = new char[] { '<' };

                            se = line.Split('<');


                        }
                    }
                } while (line != null);
                String[] ff;

                foreach (var aa in se)
                {
                    if (aa.Contains("promo-top-stories")) s.Add(aa);
                }

                foreach (var aa in s)
                {
                    if (aa.Contains(">"))
                    {
                        sf.Add(aa.Substring(aa.IndexOf(">"), aa.Length - aa.IndexOf(">")));
                    }
                }


                var q = from qq in sf where qq.StartsWith(">") && qq.Length > 2 select qq;
                return q;
            }

        }

        private void get_tit_url_scraping()
        {
            int i = 0;
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.bbc.com/mundo");
            List<string> sorti=new List<string>();
            foreach (var aa in doc.DocumentNode.CssSelect(".e57qer20.bbc-lpu9rr.eom0ln51"))
            {

                var ss = aa.CssSelect("a").First();
                string hh = aa.InnerHtml;
                string url = hh.Substring(hh.IndexOf("href")+6, hh.IndexOf("aria") - hh.IndexOf("href")-8);
                sorti.Add(url);
                urls.Add(url);
                sorti.Add(aa.InnerText);
                titles.Add(aa.InnerText);
               // MessageBox.Show(url);
                //MessageBox.Show(aa.InnerText);
            }
           // return sorti;
        }

        private void save()
        {

           // List<string> ar = get_article();
           // List<string> tit =new List<string>();

          
            DBLIB.bd bd = new bd("server=.;database=hello;integrated security=true");
            //  MessageBox.Show(tit);
            for (int i=0;i<8;i++)
            {
                bd.cmd("insert into web3(titulo,texto) values('"+titles[i]+"','"+articles[i]+"')");

            }
            MessageBox.Show("Succes");


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

        private void get_article()
        {
            string ar="";
            List<string> urls = new List<string>();
           // List<string> articles = new List<string>();
            for(int i=0;i< 20 ;i++)
            {
                urls.Add(this.urls[i].Substring(6, this.urls[i].Length - 6));
            }
            foreach (var item in urls)
            {
                HtmlWeb web = new HtmlWeb();
               // MessageBox.Show("https://www.bbc.com/mundo" + item);
                HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.bbc.com/mundo"+item);
                ar = "";
                foreach (var cc in doc.DocumentNode.CssSelect(".bbc-bm53ic.e1cc2ql70"))
                {
                    ar += cc.InnerText+"\n";
                }
                //MessageBox.Show(ar);
                this.articles.Add(ar);
              //  articles.Add(ar);

            }
          

           // return articles;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;

            get_tit_url_scraping();
             get_article();

            RempliCombo();
            Choices choi = new Choices();
            choi.Add(new string[] { "comenzar", "siguiente", "stop", "pausar", "detener", "continuar", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho" });
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
            for (int i = 0; i < 8; i++)
            {
                richTextBox1.Text += 1+i + "----> " + titles[i] + "\n";
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            
           lire(richTextBox1.Text);
            
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

        
            if(voz != null)
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

        
            if(voz != null)
            {
                voz.Dispose();
            }
            }
            catch (Exception)
            {

                
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            switch (here)
            {
                case 1 : lire(articles[1]);here = 2; break;
                case 2 : lire(articles[2]); here = 3; ; break;
                case 3 : lire(articles[3]); here = 4; ; break;
                case 4 : lire(articles[4]); here = 5; ; break;
                case 5 : lire(articles[5]); here = 6; ; break;
                case 6 : lire(articles[6]); here = 7; ; break;
                case 7 : lire(articles[7]); here = 8; ; break;
                case 8 : lire(articles[0]); here = 1; ; break;

                default:
                    break;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            save();
        }
    }
}
