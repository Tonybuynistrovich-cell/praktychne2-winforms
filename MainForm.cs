using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace PracticeWork2
{
    /// <summary>
    /// Практичне заняття 2. Контейнери елементів Windows Forms. Варіант 1.
    /// Форма замовлення піци та напоїв із двома вкладками (TabControl) і
    /// контейнером FlowLayoutPanel з кнопками Замовити / Відмінити / Вихід.
    /// Інтерфейс будується у коді, тому окремий файл-дизайнер не потрібен.
    /// </summary>
    public class MainForm : Form
    {
        // Списки полів для всіх позицій (обидві вкладки) —
        // потрібні для підрахунку вартості та очищення.
        private readonly List<NumericUpDown> quantities = new List<NumericUpDown>();
        private readonly List<TextBox> prices = new List<TextBox>();

        private Label lblTotal;

        public MainForm()
        {
            BuildUi();
        }

        private void BuildUi()
        {
            Text = "Замовлення піци та напоїв";
            ClientSize = new Size(540, 430);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var tabs = new TabControl { Left = 12, Top = 12, Width = 516, Height = 300 };

            // ---------- Вкладка 1: Піца ----------
            var tabPizza = new TabPage("Піца");
            string[] pizzaNames = { "Маргарита", "Пепероні", "Гавайська" };
            int[] pizzaPrices = { 120, 150, 140 };

            int y = 15;
            AddHeader(tabPizza, y);
            y += 28;
            for (int i = 0; i < pizzaNames.Length; i++)
            {
                AddItemRow(tabPizza, y, pizzaNames[i], pizzaPrices[i]);
                y += 34;
            }

            // PictureBox із зображенням піци (необов'язковий елемент за завданням)
            var pic = new PictureBox
            {
                Left = 330,
                Top = 15,
                Width = 160,
                Height = 160,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            if (File.Exists("pizza.png"))
                pic.Image = Image.FromFile("pizza.png");
            tabPizza.Controls.Add(pic);

            tabs.TabPages.Add(tabPizza);

            // ---------- Вкладка 2: Напої ----------
            var tabDrinks = new TabPage("Напої");
            string[] drinkNames = { "Сік", "Кава", "Чай" };
            int[] drinkPrices = { 35, 40, 25 };

            y = 15;
            AddHeader(tabDrinks, y);
            y += 28;
            for (int i = 0; i < drinkNames.Length; i++)
            {
                AddItemRow(tabDrinks, y, drinkNames[i], drinkPrices[i]);
                y += 34;
            }

            tabs.TabPages.Add(tabDrinks);
            Controls.Add(tabs);

            // ---------- Підсумок ----------
            lblTotal = new Label
            {
                Left = 12,
                Top = 322,
                Width = 516,
                Height = 24,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Text = "Вартість замовлення: 0,00 грн"
            };
            Controls.Add(lblTotal);

            // ---------- FlowLayoutPanel з кнопками ----------
            var flow = new FlowLayoutPanel
            {
                Left = 12,
                Top = 352,
                Width = 516,
                Height = 56,
                FlowDirection = FlowDirection.LeftToRight,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(6)
            };

            var btnOrder = new Button { Text = "Замовити", Width = 150, Height = 36 };
            var btnCancel = new Button { Text = "Відмінити", Width = 150, Height = 36 };
            var btnExit = new Button { Text = "Вихід", Width = 150, Height = 36 };

            btnOrder.Click += Order_Click;
            btnCancel.Click += Cancel_Click;
            btnExit.Click += (s, e) => Close();

            flow.Controls.Add(btnOrder);
            flow.Controls.Add(btnCancel);
            flow.Controls.Add(btnExit);
            Controls.Add(flow);
        }

        // Заголовок «таблиці» на вкладці
        private void AddHeader(TabPage tab, int y)
        {
            tab.Controls.Add(new Label { Left = 15, Top = y, Width = 130, Text = "Назва" });
            tab.Controls.Add(new Label { Left = 150, Top = y, Width = 80, Text = "Кількість" });
            tab.Controls.Add(new Label { Left = 240, Top = y, Width = 80, Text = "Ціна, грн" });
        }

        // Один рядок позиції: Label (назва) + NumericUpDown (кількість) + TextBox (ціна)
        private void AddItemRow(TabPage tab, int y, string name, int price)
        {
            tab.Controls.Add(new Label { Left = 15, Top = y + 3, Width = 130, Text = name });

            var num = new NumericUpDown { Left = 150, Top = y, Width = 70, Minimum = 0, Maximum = 100 };
            tab.Controls.Add(num);
            quantities.Add(num);

            var txt = new TextBox { Left = 240, Top = y, Width = 70, Text = price.ToString() };
            tab.Controls.Add(txt);
            prices.Add(txt);
        }

        // Кнопка «Замовити» — обчислення вартості замовлення
        private void Order_Click(object sender, EventArgs e)
        {
            decimal total = 0;
            for (int i = 0; i < quantities.Count; i++)
            {
                decimal price;
                if (!decimal.TryParse(prices[i].Text.Trim().Replace(',', '.'),
                        NumberStyles.Any, CultureInfo.InvariantCulture, out price))
                {
                    MessageBox.Show("Невірно введена ціна у рядку №" + (i + 1),
                        "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                total += quantities[i].Value * price;
            }

            lblTotal.Text = $"Вартість замовлення: {total:F2} грн";

            if (total == 0)
                MessageBox.Show("Ви не обрали жодної позиції.",
                    "Замовлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show($"Загальна вартість замовлення: {total:F2} грн",
                    "Замовлення оформлено", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Кнопка «Відмінити» — очищення всіх NumericUpDown
        private void Cancel_Click(object sender, EventArgs e)
        {
            foreach (var num in quantities)
                num.Value = 0;
            lblTotal.Text = "Вартість замовлення: 0,00 грн";
        }
    }
}
