using System;
using System.Windows.Forms;
using Proxy;
using StartGamePlatformInvoke;

namespace SilkroadProxyWithForms
{
    public partial class MainForm : Form
    {
        private SilkroadProxy _silkroadProxy;
        private Injector _injector;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                _silkroadProxy = new SilkroadProxy(this);
                _silkroadProxy.StartProxy();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private delegate void UpdateStatusDelegate(string msg);

        private void WriteStatusGateway(string msg)
        {
            toolStripStatusLabelGateway.Text = msg;
        }

        public void UpdateStatusGateway(string msg)
        {
            UpdateStatusDelegate writer = new UpdateStatusDelegate(WriteStatusGateway);
            this.Invoke(writer, new object[] { msg });
        }

        private void WriteStatusAgent(string msg)
        {
            toolStripStatusLabelAgent.Text = msg;
        }

        public void UpdateStatusAgent(string msg)
        {
            UpdateStatusDelegate writer = new UpdateStatusDelegate(WriteStatusAgent);
            this.Invoke(writer, new object[] { msg });
        }

        private void WriteNotify(string msg)
        {
            toolStripStatusLabelNotify.Text = msg;
        }

        public void UpdateNotify(string msg)
        {
            UpdateStatusDelegate writer = new UpdateStatusDelegate(WriteNotify);
            this.Invoke(writer, new object[] { msg });
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            _injector = new Injector();
            _injector.injectDll();
        }

        delegate void UpdateLableStartGameButtonDelegate(bool flag);

        private void WriteLabelStartGameButton(bool flag)
        {
            if (flag)
            {
                StartGameButton.Text = "Start Game More";
            }
            else
            {
                StartGameButton.Text = "Start Game";
            }
        }

        public void UpdateLabelStartGameButton(bool flag)
        {
            UpdateLableStartGameButtonDelegate writer = new UpdateLableStartGameButtonDelegate(WriteLabelStartGameButton);
            this.Invoke(writer, new object[] { flag });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_silkroadProxy != null)
            {
                _silkroadProxy.Dispose();
            }
        }
    }
}
