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
using ExpertSystem.Data;

namespace ExpertSystem.WinForms
{
    public partial class MainForm : Form
    {
        private EsKnowledge Knowledge { get; }

        public MainForm()
        {
            InitializeComponent();

            //set working directory for WinForm project to ../../../
            Knowledge = EsParser.ParseKnowledge(File.ReadAllText("Assets/knowledge.txt"));
        }
    }
}
