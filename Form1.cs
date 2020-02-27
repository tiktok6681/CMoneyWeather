using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
/*　
                  ＼○／
                       /
                彡ノ)
━━━━━┒ 
┓┏┓┏┓┃ 
┛┗┛┗┛┃
┓┏┓┏┓┃
┛┗┛┗┛┃＼○／
┓┏┓┏┓┃    /
┛┗┛┗┛┃ノ)
┓┏┓┏┓┃
┛┗┛┗┛┃
┓┏┓┏┓┃
┛┗┛┗┛┃
┓┏┓┏┓┃
┛┗┛┗┛┃川
┓┏┓┏┓┃＼     ／
┛┗┛┗┛┃＼)－○／
┓┏┓┏┓┃
┛┗┛┗┛┃
┓┏┓┏┓┃
┛┗┛┗┛┃
┓┏┓┏┓┃
┛┗┛┗┛┃  川
┓┏┓┏┓┃ヾノ
┛┗┛┗┛┃  ｜
┓┏┓┏┓┃ヽ|ノ
┛┗┛┗┛┃  ○
┓┏┓┏┓┃
┛┗┛┗┛┃
┓┏┓┏┓┃
┛┗┛┗┛┃
┓┏┓┏┓┃
┃┃┃┃┃┃
┻┻┻┻┻┻_(┐「﹃ﾟ｡)__ 
*/
namespace CMoneyWeather
{
    public partial class Form1 : Form
    {
        private JArray jsondata;
        private List<String> city = new List<String>();
        private List<String> town = new List<String>();
        private List<String> dateTime = new List<String>();
        private List<String> temp = new List<String>();
        private List<String> tempH = new List<String>();
        private List<String> tempL = new List<String>();
        private List<String> uvi = new List<String>();
        private List<String> wdsd = new List<String>();
        private List<String> humd = new List<String>();
        public Form1()
        {
            InitializeComponent();
            jsondata = getJson(" https://opendata.cwb.gov.tw/api/v1/rest/datastore/O-A0003-001?Authorization=rdec-key-123-45678-011121314");
            foreach (JObject data in jsondata)
            {
                WeatherInfo wInfo = new WeatherInfo();
                wInfo.city = (string)data["parameter"][0]["parameterValue"];
                wInfo.town = (string)data["parameter"][2]["parameterValue"];
                wInfo.dateTime = (string)data["time"]["obsTime"];
                wInfo.temp = (string)data["weatherElement"][3]["elementValue"];
                wInfo.tempH = (string)data["weatherElement"][14]["elementValue"];
                wInfo.tempL = (string)data["weatherElement"][16]["elementValue"];
                wInfo.uvi = (string)data["weatherElement"][13]["elementValue"];
                wInfo.wdsd = (string)data["weatherElement"][2]["elementValue"];
                wInfo.humd = (string)data["weatherElement"][4]["elementValue"];
                addWeatherInfo(wInfo);

                city.Add((string)data["parameter"][0]["parameterValue"]);
                town.Add((string)data["parameter"][2]["parameterValue"]);
                dateTime.Add((string)data["time"]["obsTime"]);
                temp.Add((string)data["weatherElement"][3]["elementValue"]);
                tempH.Add((string)data["weatherElement"][14]["elementValue"]);
                tempL.Add((string)data["weatherElement"][16]["elementValue"]);
                uvi.Add((string)data["weatherElement"][13]["elementValue"]);
                wdsd.Add((string)data["weatherElement"][2]["elementValue"]);
                humd.Add((string)data["weatherElement"][4]["elementValue"]);

            }

        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            cellStartY = 0;

            List<int> index = new List<int>();
            float tempvalue = float.Parse(textBox1.Text.ToString());
            float uvivalue = float.Parse(textBox2.Text.ToString());
            if (comboBox1.SelectedItem.ToString().Equals(">"))
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (float.Parse(temp[i]) > tempvalue)
                    {

                        if (comboBox2.SelectedItem.ToString().Equals(">"))
                        {
                            if (float.Parse(uvi[i]) > uvivalue)
                            {
                                index.Add(i);
                            }
                        }
                        else if (comboBox2.SelectedItem.ToString().Equals("<"))
                        {
                            if (float.Parse(uvi[i]) < uvivalue)
                            {
                                index.Add(i);
                            }
                        }
                    }
                }
            }
            else if (comboBox1.SelectedItem.ToString().Equals("<"))
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (float.Parse(temp[i]) < tempvalue)
                    {
                        if (comboBox2.SelectedItem.ToString().Equals(">"))
                        {
                            if (float.Parse(uvi[i]) > uvivalue)
                            {
                                index.Add(i);
                            }
                        }
                        else if (comboBox2.SelectedItem.ToString().Equals("<"))
                        {
                            if (float.Parse(uvi[i]) < uvivalue)
                            {
                                index.Add(i);
                            }
                        }
                    }
                }
            }
            foreach (int number in index)
            {
                WeatherInfo wInfo = new WeatherInfo();
                wInfo.city = city[number];
                wInfo.town = town[number];
                wInfo.dateTime = dateTime[number];
                wInfo.temp = temp[number];
                wInfo.tempH = tempH[number]; 
                wInfo.tempL = tempL[number]; 
                wInfo.uvi = uvi[number]; 
                wInfo.wdsd = wdsd [number];
                wInfo.humd = humd[number]; 
                addWeatherInfo(wInfo);
                
            }



        }
        private class WeatherInfo
        {
            public string city;
            public string town;
            public string dateTime;
            public string temp;
            public string tempH;
            public string tempL;
            public string uvi;
            public string wdsd;
            public string humd;
        }

        private int cellStartY = 0;
        private const int DATA_CELL_HEIGHT = 100;
        private const int DATA_CELL_WIDTH = 600;

        private void addWeatherInfo(WeatherInfo info)
        {
            Label lb = new Label();
            lb.BackColor = Color.FromArgb(255, 0x00, 0x88, 0x66);
            lb.AutoSize = false;
            lb.Size = new Size(200, 40);
            lb.TextAlign = ContentAlignment.MiddleCenter;
            lb.Location = new Point(0, cellStartY);
            lb.Text = info.city;
            lb.Font = new Font("新細明體", 16);

            Label lb2 = new Label();
            lb2.BackColor = Color.FromArgb(255, 0x00, 0xDD, 0xAA);
            lb2.AutoSize = false;
            lb2.Size = new Size(200, 40);
            lb2.TextAlign = ContentAlignment.MiddleCenter;
            lb2.Location = new Point(0, lb.Location.Y + lb.Size.Height);
            lb2.Text = info.town;
            lb2.Font = new Font("新細明體", 16);

            Label lb3 = new Label();
            lb3.BackColor = Color.FromArgb(255, 0x33, 0xFF, 0xDD);
            lb3.AutoSize = false;
            lb3.Size = new Size(200, 20);
            lb3.TextAlign = ContentAlignment.MiddleCenter;
            lb3.Location = new Point(0, lb2.Location.Y + lb2.Size.Height);
            lb3.Text = info.dateTime;
            lb3.Font = new Font("新細明體", 14);

            Label lb5 = new Label();
            lb5.BackColor = Color.FromArgb(255, 0xFF, 0x88, 0x00);
            lb5.AutoSize = false;
            lb5.Size = new Size(180, 60);
            lb5.TextAlign = ContentAlignment.TopCenter;
            lb5.Location = new Point(lb.Size.Width + 10, lb2.Location.Y);
            lb5.Text = "今日最高溫" + info.tempH + "度\r\n今日最低溫" + info.tempL + "度\r\n紫外線指數" + info.uvi;
            lb5.Font = new Font("新細明體", 14);

            Label lb6 = new Label();
            lb6.BackColor = Color.FromArgb(255, 0xFF, 0x88, 0x00);
            lb6.AutoSize = false;
            lb6.Size = new Size(180, 60);
            lb6.TextAlign = ContentAlignment.TopCenter;
            lb6.Location = new Point(lb5.Location.X + 10 + lb5.Size.Width, lb5.Location.Y);
            lb6.Text = "風速" + info.wdsd + "公尺/秒\r\n相對溼度" + info.humd;
            lb6.Font = new Font("新細明體", 14);

            Label lb4 = new Label();
            lb4.BackColor = Color.FromArgb(255, 0x88, 0x88, 0x88);
            lb4.AutoSize = false;
            lb4.Size = new Size(400, 100);
            lb4.TextAlign = ContentAlignment.TopCenter;
            lb4.Padding = new Padding(0, 10, 0, 0);
            lb4.Location = new Point(lb.Size.Width, lb.Location.Y);
            lb4.Text = "現在溫度" + info.temp + "度";
            lb4.Font = new Font("新細明體", 14);

            this.panel1.Controls.Add(lb);
            this.panel1.Controls.Add(lb2);
            this.panel1.Controls.Add(lb3);
            this.panel1.Controls.Add(lb5);
            this.panel1.Controls.Add(lb6);
            this.panel1.Controls.Add(lb4);

            cellStartY += DATA_CELL_HEIGHT + 10;
        }
        static public JArray getJson(string uri)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri); //request請求
                req.Timeout = 10000; //request逾時時間
                req.Method = "GET"; //request方式
                HttpWebResponse respone = (HttpWebResponse)req.GetResponse(); //接收respone
                StreamReader streamReader = new StreamReader(respone.GetResponseStream(), Encoding.UTF8); //讀取respone資料
                string result = streamReader.ReadToEnd(); //讀取到最後一行
                respone.Close();
                streamReader.Close();
                JObject jsondata = JsonConvert.DeserializeObject<JObject>(result); //將資料轉為json物件
                return (JArray)jsondata["records"]["location"]; //回傳json陣列
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

    }
}
