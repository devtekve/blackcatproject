using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace Client
{
    public partial class ClientForm : Form
    {
        private Client _client;
        private IPEndPoint _ipEndPoint;
        private bool _isEnterPress;

        public ClientForm()
        {
            InitializeComponent();
            _client = new Client(this);
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(ipTextBox.Text), Convert.ToInt32(portTextBox.Text));
        }

        #region GUI stuff
        public void ChangeVisibleButton()
        {
            connectButton.Visible = !connectButton.Visible;
            sendButton.Visible = !sendButton.Visible;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (_client.Connect(_ipEndPoint))
            {
                ChangeVisibleButton();
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (sendButton.Visible)
            {
                _client.Send(messageTextBox.Text);
            }
        }

        delegate void WiteLogsDelegate(string output);

        public void WriteLogs(string output)
        {
            WiteLogsDelegate writer = new WiteLogsDelegate(WriteText);
            this.Invoke(writer, new object[] { output });

        }

        private void WriteText(string output)
        {
            logsRichTextBox.AppendText(DateTime.Now.ToString() + " : " + output + Environment.NewLine);
        }

        private void messageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            _isEnterPress = false;
            if (e.KeyCode == Keys.Enter)
            {
                _isEnterPress = true;
            }
        }

        private void messageTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_isEnterPress)
            {
                if (sendButton.Visible)
                {
                    _client.Send(messageTextBox.Text);
                }
            }
        }

        #endregion
    }
}
