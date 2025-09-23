using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;

namespace Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Logic logic = new Logic();
        private void button1_Click(object sender, EventArgs e) // create
        {
            try
            {
                
                int id = int.Parse(textBoxId.Text);
                string name = textBoxName.Text;
                int level = int.Parse(textBoxLevel.Text);
                int score = int.Parse(textBoxScore.Text);
                string rank = textBoxRank.Text;
                DateTime date = DateTime.Parse(textBoxDate.Text); 
               
                logic.Create(id, name, level, score, rank, date);

                MessageBox.Show("Игрок создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);


                ClearTextBoxes();
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        private void button3_Click(object sender, EventArgs e) //delete
        {
            try
            {
                int id = int.Parse(textBoxId.Text);
                logic.Delete(id);

                MessageBox.Show("Игрок удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearTextBoxes();
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button4_Click(object sender, EventArgs e) // update
        {
            try
            {
                int id = int.Parse(textBoxId.Text);
                string name = textBoxName.Text;
                int level = int.Parse(textBoxLevel.Text);
                int score = int.Parse(textBoxScore.Text);
                string rank = textBoxRank.Text;
                DateTime date = DateTime.Parse(textBoxDate.Text);

                logic.Update(id, name, level, score, rank, date);

                MessageBox.Show("Игрок обновлён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearTextBoxes();
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e) //dareFilter
        {
            try
            {
                DateTime start = DateTime.Parse(textBoxStartDate.Text);
                DateTime end = DateTime.Parse(textBoxEndDate.Text);

               
                var filteredPlayers = logic.DateGroup(start, end)
                                           .Select(p => new List<string>
                                           {
                                       p.Id.ToString(),
                                       p.Name,
                                       p.Level.ToString(),
                                       p.Score.ToString(),
                                       p.Rank,
                                       p.RegistrationDate.ToString("dd.MM.yyyy")
                                           }).ToList();

                RefreshGrid(filteredPlayers);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void RefreshGrid(List<List<string>> players = null)
        {
            var allPlayers = players ?? logic.Read(); 

            var table = new DataTable();
            table.Columns.Add("Id");
            table.Columns.Add("Имя");
            table.Columns.Add("Уровень");
            table.Columns.Add("Очки");
            table.Columns.Add("Ранг");
            table.Columns.Add("Дата регистрации");

            foreach (var p in allPlayers)
            {
                table.Rows.Add(p.ToArray()); 
            }

            dataGridViewPlayers.DataSource = table;
            dataGridViewPlayers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void ClearTextBoxes()
        {
            textBoxId.Clear();
            textBoxName.Clear();
            textBoxLevel.Clear();
            textBoxScore.Clear();
            textBoxRank.Clear();
            textBoxDate.Clear();
        }
        private void DataGridViewPlayers_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewPlayers.CurrentRow != null)
            {
                
                var row = dataGridViewPlayers.CurrentRow;

                
                textBoxId.Text = row.Cells["Id"].Value.ToString();
                textBoxName.Text = row.Cells["Имя"].Value.ToString();
                textBoxLevel.Text = row.Cells["Уровень"].Value.ToString();
                textBoxScore.Text = row.Cells["Очки"].Value.ToString();
                textBoxRank.Text = row.Cells["Ранг"].Value.ToString();
                textBoxDate.Text = row.Cells["Дата регистрации"].Value.ToString();
            }
        }
        private void dataGridViewPlayers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewPlayers.SelectionChanged += DataGridViewPlayers_SelectionChanged;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e) //rank
        {
            try
            {
                string rankFilter = textBoxRankFilter.Text.Trim();

                var grouped = logic.RankGroup();

                var tableData = new List<List<string>>();

                
                if (string.IsNullOrEmpty(rankFilter))
                {
                    foreach (var kvp in grouped)
                    {
                        foreach (var player in kvp.Value)
                        {
                            tableData.Add(new List<string>
                    {
                        player.Id.ToString(),
                        player.Name,
                        player.Level.ToString(),
                        player.Score.ToString(),
                        kvp.Key,
                        player.RegistrationDate.ToString("dd.MM.yyyy")
                    });
                        }
                    }
                }
                else
                {
                   
                    if (grouped.ContainsKey(rankFilter))
                    {
                        foreach (var player in grouped[rankFilter])
                        {
                            tableData.Add(new List<string>
                    {
                        player.Id.ToString(),
                        player.Name,
                        player.Level.ToString(),
                        player.Score.ToString(),
                        rankFilter,
                        player.RegistrationDate.ToString("dd.MM.yyyy")
                    });
                        }
                    }
                }

                RefreshGrid(tableData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
