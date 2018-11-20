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
        private InferenceEngine Engine { get; }

        private string _currentAnswer;

        public MainForm()
        {
            InitializeComponent();

            //set working directory for WinForm project to ../../../
            Knowledge = EsParser.ParseKnowledge(File.ReadAllText("Assets/knowledge.txt"));
            Engine = new InferenceEngine(Knowledge, (x, y) => AskAndAwait(x, y));

            var possibleQuestions = Knowledge.RulesByResultPropertyName
                .Where(x => x.Value.Any())
                .Select(x => x.Key as object)
                .ToArray();

            QuestionComboBox.Items.AddRange(possibleQuestions);
            QuestionComboBox.SelectedItem = possibleQuestions.First();
            DetailsGroupBox.Hide();
        }

        public async Task<string> AskAndAwait(string property, IReadOnlyList<string> variants)
        {
            Ask(property, variants);

            while (_currentAnswer == null)
            {
                await Task.Delay(100);
            }

            return _currentAnswer;
        }

        private void Ask(string property, IReadOnlyList<string> variants)
        {
            _currentAnswer = null;
            DetailsLabel.Text = $"Укажите: {property}";
            DetailsListBox.DataSource = variants;
            DetailsListBox.SelectedItem = variants.First();
            DetailsGroupBox.Show();
        }

        private async void RunButton_Click(object sender, EventArgs e)
        {
            var targetProperty = QuestionComboBox.SelectedItem.ToString();
            var result = await Engine.AnalyzeAsync(targetProperty);

            DetailsGroupBox.Hide();

            if (result == null)
            {
                MessageBox.Show("Невозможно найти результат");
            }
            else
            {
                MessageBox.Show($"Результат: {result}");
            }
        }

        private void UnknowButton_Click(object sender, EventArgs e)
        {
            _currentAnswer = string.Empty;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            _currentAnswer = DetailsListBox.SelectedItem.ToString();
        }

        private void DetailsListBox_DoubleClick(object sender, EventArgs e)
        {
            var location = (e as MouseEventArgs)?.Location;

            if (location == null)
            {
                return;
            }

            var index = DetailsListBox.IndexFromPoint(location.Value);
            if (index != ListBox.NoMatches)
            {
                _currentAnswer = DetailsListBox.SelectedItem.ToString();
            }
        }
    }
}
