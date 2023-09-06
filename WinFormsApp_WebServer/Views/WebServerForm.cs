using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        public WebServerForm(IWebController webController)
        {
            this.controller = webController;

            InitializeComponent();
            btnStop.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbPort.Text) && tbPort.Text.All(c => char.IsDigit(c)) && tbPort.Text.Length < 6)
            {
                int port = Convert.ToInt32(tbPort.Text);
                if (controller.StartServer(port))
                {
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
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
