using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnowFlake
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string result = string.Empty;

            for (int i = 0; i < 100; i++)
            {
                result = string.Format("{0}{1}\r\n", result, SnowFlakeAlg.GetGuid());
            }

            Console.WriteLine(result);
            textBox_Msg.Text = result;

        }
    }
}
