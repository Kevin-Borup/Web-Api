using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp_WebServer.Controllers;
using WinFormsApp_WebServer.Interfaces;

namespace WinFormsApp_WebServer.Views
{
    internal partial class WebServerForm : Form
    {
        IWebController controller;
        string defaultPortNr = "23005";
        public WebServerForm(IWebController webController)
        {
            this.controller = webController;

            InitializeComponent();
            btnStop.Enabled = false;
            tbPort.PlaceholderText = defaultPortNr;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPort.Text)) tbPort.Text = defaultPortNr;

            if (tbPort.Text.All(c => char.IsDigit(c)) && tbPort.Text.Length < 6)
            {
                int port = Convert.ToInt32(tbPort.Text);

                // max size of 16-bit integer, the number of ports
                if (port > 65535)
                {
                    port = 65535;
                }

                tbPort.Text = port.ToString();


                if (controller.StartServer(port))
                {
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                    Process.Start(new ProcessStartInfo("http://localhost:" + port) { UseShellExecute = true });
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            controller.StopServer();
            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }

        /// <summary>
        /// Validates input to limit it to numbers, but not allowing editing while the server is already running.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool isNotControl = !char.IsControl(e.KeyChar);
            bool isNotDigit = !char.IsDigit(e.KeyChar);
            bool isServerOn = btnStop.Enabled;

            e.Handled = isServerOn;

            if (isNotControl && isNotDigit)
            {
                e.Handled = true;
            }
        }
    }
}
