using System;
using System.Drawing;
using System.Windows.Forms;
using CalculatorApp.Operations;

namespace CalculatorApp
{
    public partial class Form1 : Form
    {
        private readonly CalculatorEngine _engine = new CalculatorEngine();
        private readonly HistoryManager _history = new HistoryManager();

        private TextBox _display;
        private Label _operationLabel;
        private ListBox _historyList;

        // Размеры элементов
        private const int BTN_W = 88;      // Чуть уменьшили ширину кнопки
        private const int BTN_H = 55;
        private const int GAP = 6;

        private const int CONTENT_WIDTH = (BTN_W * 4) + (GAP * 3); // 88*4 + 6*3 = 370px
        private const int MARGIN_X = 12;

        public Form1()
        {
            InitializeComponent();
            BuildUI();

            // ЖЕСТКО ЗАДАЕМ РАЗМЕРЫ ФОРМЫ
            this.ClientSize = new Size(CONTENT_WIDTH + (MARGIN_X * 2), 620);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.AutoScaleMode = AutoScaleMode.None; // Отключаем масштабирование, чтобы не ехали размеры

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }

        private void BuildUI()
        {
            // ===== ДИСПЛЕЙ =====
            _operationLabel = new Label
            {
                Location = new Point(MARGIN_X, 10),
                Size = new Size(CONTENT_WIDTH, 25),
                Text = "",
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                BackColor = Color.FromArgb(45, 45, 45)
            };
            this.Controls.Add(_operationLabel);

            _display = new TextBox
            {
                Location = new Point(MARGIN_X, 38),
                Size = new Size(CONTENT_WIDTH, 55),
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true,
                Text = "0",
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                TabStop = false
            };
            this.Controls.Add(_display);

            // ===== ИСТОРИЯ =====
            _historyList = new ListBox
            {
                Location = new Point(MARGIN_X, 100),
                Size = new Size(CONTENT_WIDTH, 75),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.LightGray,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 10),
                IntegralHeight = false
            };
            this.Controls.Add(_historyList);

            // ===== КНОПКИ =====
            string[,] buttons = {
                { "C",  "CE", "←",  "/" },
                { "7",  "8",  "9",  "*" },
                { "4",  "5",  "6",  "-" },
                { "1",  "2",  "3",  "+" },
                { "±",  "0",  ".",  "=" },
                { "√",  "x²", "1/x","%" },
                { "MC", "MR", "M+", "M-" }
            };

            int startY = 185;

            for (int row = 0; row < buttons.GetLength(0); row++)
            {
                for (int col = 0; col < buttons.GetLength(1); col++)
                {
                    string text = buttons[row, col];
                    Button btn = new Button
                    {
                        Text = text,
                        Location = new Point(
                            MARGIN_X + col * (BTN_W + GAP),
                            startY + row * (BTN_H + GAP)),
                        Size = new Size(BTN_W, BTN_H),
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        ForeColor = Color.White,
                        Cursor = Cursors.Hand,
                        TabStop = false
                    };
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseOverBackColor = GetHoverColor(text);
                    btn.FlatAppearance.MouseDownBackColor = GetDownColor(text);
                    btn.BackColor = GetButtonColor(text);

                    btn.Click += Button_Click;
                    this.Controls.Add(btn);
                }
            }
        }

        private Color GetButtonColor(string text)
        {
            if ("/*-+=".Contains(text) || text == "%")
                return Color.FromArgb(255, 140, 0);
            if (text == "C" || text == "CE" || text == "←")
                return Color.FromArgb(180, 50, 50);
            if ("MCMRM+M-".Contains(text))
                return Color.FromArgb(70, 70, 100);
            if (char.IsDigit(text[0]) || text == "." || text == "±")
                return Color.FromArgb(60, 60, 60);
            return Color.FromArgb(80, 80, 80);
        }

        private Color GetHoverColor(string text)
        {
            Color c = GetButtonColor(text);
            return Color.FromArgb(
                Math.Min(c.R + 25, 255),
                Math.Min(c.G + 25, 255),
                Math.Min(c.B + 25, 255));
        }

        private Color GetDownColor(string text)
        {
            Color c = GetButtonColor(text);
            return Color.FromArgb(
                Math.Max(c.R - 25, 0),
                Math.Max(c.G - 25, 0),
                Math.Max(c.B - 25, 0));
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            HandleButton(btn.Text);
        }

        private void HandleButton(string t)
        {
            try
            {
                switch (t)
                {
                    case "C":
                        _display.Text = _engine.Clear();
                        _operationLabel.Text = "";
                        break;
                    case "CE":
                        _engine.Clear();
                        _display.Text = "0";
                        _operationLabel.Text = "";
                        break;
                    case "←":
                        _display.Text = _engine.Backspace();
                        break;
                    case "±":
                        _display.Text = _engine.Negate();
                        break;
                    case "√":
                        _display.Text = _engine.SquareRoot();
                        AddHistory($"√ = {_display.Text}");
                        break;
                    case "x²":
                        _display.Text = _engine.Square();
                        AddHistory($"x² = {_display.Text}");
                        break;
                    case "1/x":
                        _display.Text = _engine.Inverse();
                        AddHistory($"1/x = {_display.Text}");
                        break;
                    case "=":
                        string res = _engine.Calculate();
                        _operationLabel.Text = "";
                        _display.Text = res;
                        AddHistory($"= {res}");
                        break;
                    case "MC": _engine.MemoryClear(); AddHistory("MC"); break;
                    case "MR": _display.Text = _engine.MemoryRecall(); AddHistory("MR"); break;
                    case "M+": _engine.MemoryAdd(); AddHistory("M+"); break;
                    case "M-": _engine.MemorySubtract(); AddHistory("M-"); break;
                    case ".":
                        if (!_display.Text.Contains("."))
                            _display.Text = _engine.InputDigit(".");
                        break;
                    default:
                        if (char.IsDigit(t[0]))
                        {
                            _display.Text = _engine.InputDigit(t);
                        }
                        else if (OperationFactory.IsOperation(t))
                        {
                            _display.Text = _engine.SetOperation(t);
                            _operationLabel.Text = $" {t} ";
                        }
                        break;
                }
            }
            catch (DivideByZeroException ex)
            {
                ShowError(ex.Message);
            }
            catch (ArgumentException ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _display.Text = _engine.Clear();
            _operationLabel.Text = "";
        }

        private void AddHistory(string entry)
        {
            _history.Add(entry);
            UpdateHistory();
        }

        private void UpdateHistory()
        {
            _historyList.Items.Clear();
            foreach (var item in _history.GetAll())
                _historyList.Items.Add(item);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            string t = e.KeyCode switch
            {
                Keys.D0 or Keys.NumPad0 => "0",
                Keys.D1 or Keys.NumPad1 => "1",
                Keys.D2 or Keys.NumPad2 => "2",
                Keys.D3 or Keys.NumPad3 => "3",
                Keys.D4 or Keys.NumPad4 => "4",
                Keys.D5 or Keys.NumPad5 => "5",
                Keys.D6 or Keys.NumPad6 => "6",
                Keys.D7 or Keys.NumPad7 => "7",
                Keys.D8 or Keys.NumPad8 => "8",
                Keys.D9 or Keys.NumPad9 => "9",
                Keys.Add => "+",
                Keys.Subtract => "-",
                Keys.Multiply => "*",
                Keys.Divide => "/",
                Keys.OemPeriod or Keys.Decimal => ".",
                Keys.Enter => "=",
                Keys.Back => "←",
                Keys.Escape => "C",
                _ => null
            };

            if (t != null)
            {
                e.SuppressKeyPress = true;
                HandleButton(t);
            }
        }
    }
}