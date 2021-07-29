using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace AsinAppAmazon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //You may enter any amazon merchant's address here, also you can search it from typing address to URL section and pressing search button.
            //Just type https://www.google.com if you want to make it homepage.
            textBox1.Text = "https://www.amazon.ca/s?i=merchant-items&me=A1U6PAVBPLQVBI";
            webBrowser1.Navigate(textBox1.Text);
            label5.Text = "1";
        }

        //Search Button
        private void button1_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            webBrowser1.Navigate(url);
        }

        //Updating HTML Code section when browser completed,
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            richTextBox1.Text = webBrowser1.DocumentText;
        }

        //Main algorithm for get ASIN data from each page
        private void GetAsin()
        {
            if (webBrowser1.Document != null)
            {
                HtmlElementCollection elems = webBrowser1.Document.GetElementsByTagName("div");
                foreach (HtmlElement elem in elems)
                {
                    String nameStr = elem.GetAttribute("data-asin");
                    if (nameStr != null && nameStr.Length != 0)
                    {
                        //String contentStr = elem.GetAttribute("data-asin");
                        //MessageBox.Show("Document: " + webBrowser1.Url.ToString() + "\nDescription: " + contentStr);
                        String contentStr = elem.GetAttribute("data-asin");
                        richTextBox2.Text += contentStr + "\n";
                    }
                }
            }
        }

        //Needed for count the page and print it to label
        int count = 2;
        //For turn the page
        private void TurnPage()
        {
            
            if (webBrowser1.Document != null)
            {
                HtmlElementCollection elems = webBrowser1.Document.GetElementsByTagName("li");
                foreach (HtmlElement elem in elems)
                {
                    if (elem.InnerText.Contains("Next"))
                    {
                        label5.Text = count.ToString();
                        count++;
                        HtmlElementCollection elementColl = elem.Children;
                        foreach (HtmlElement element in elementColl)
                        {

                            string nextPageLink = element.GetAttribute("href");
                            webBrowser1.Navigate(nextPageLink);
                            richTextBox1.Text = webBrowser1.DocumentText;
                            textBox1.Text = nextPageLink;
                        }
                    }
                }
            }
        }

        //What happens when timer tick;
        private void timer1_Tick(object sender, EventArgs e)
        {
            GetAsin();
            TurnPage();
        }

        //Start button
        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 5000;
        }

        //Stop button
        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
