using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Windows.Forms.DataVisualization.Charting;

namespace ArduinoVoltmeter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboBoxPorts.Items.AddRange(GetAvailablePorts());
            comboBoxPorts.SelectedIndex = 0;
            comboBoxBauds.SelectedIndex = 0;

            chartVolt.Series[0].XValueType = ChartValueType.Time;
            chartVolt.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Seconds;
            chartVolt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
        }

        private String[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (comboBoxPorts.Text == "" || comboBoxBauds.Text == "")
            {
                labelStatus.Text = "Please select PORT Name and Baud Rate.";
            }
            else
            {
                serialPort1.PortName = comboBoxPorts.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBoxBauds.Text);

                if (serialPort1.IsOpen)
                {
                    labelStatus.Text = serialPort1.PortName + " busy!";
                }
                else
                {
                    try
                    {
                        serialPort1.Open();

                        OpenComponents();

                        labelStatus.Text = "Ready!";
                    }
                    catch (UnauthorizedAccessException)
                    {
                        labelStatus.Text = "Connection failed!";

                    }
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            CloseComponents();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine(textBoxSend.Text);
        }

        private void textBoxSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSend_Click(this, new EventArgs());
            }
        }

        private void OpenComponents()
        {
            buttonOpen.Enabled = false;
            buttonClose.Enabled = true;
            textBoxSend.Enabled = true;
            buttonSend.Enabled = true;
            comboBoxPorts.Enabled = false;
            comboBoxBauds.Enabled = false;
        }

        private void CloseComponents()
        {
            buttonOpen.Enabled = true;
            buttonClose.Enabled = false;
            textBoxSend.Enabled = false;
            buttonSend.Enabled = false;
            comboBoxPorts.Enabled = true;
            comboBoxBauds.Enabled = true;
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SetText(serialPort1.ReadLine());
        }

        delegate void SetTextCallback(String text);

        private void SetText(String text)
        {
            if (this.textBoxReceived.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                textBoxReceived.AppendText(text);
                if (checkBoxAutoScroll.Checked)
                {
                    textBoxReceived.SelectionStart = textBoxReceived.Text.Length;
                    textBoxReceived.ScrollToCaret();
                }
            }
        }
        
    }

  
}
