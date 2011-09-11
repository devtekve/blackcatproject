using System;
using System.Windows.Forms;
using Proxy;
using StartGamePlatformInvoke;
using System.Collections.Generic;
using System.Text;

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

        private delegate void WriteDelegate(string msg);

        private void WriteStatusGateway(string msg)
        {
            toolStripStatusLabelGateway.Text = msg;
        }

        public void UpdateStatusGateway(string msg)
        {
            WriteDelegate writer = new WriteDelegate(WriteStatusGateway);
            this.Invoke(writer, new object[] { msg });
        }

        private void WriteStatusAgent(string msg)
        {
            toolStripStatusLabelAgent.Text = msg;
        }

        public void UpdateStatusAgent(string msg)
        {
            WriteDelegate writer = new WriteDelegate(WriteStatusAgent);
            this.Invoke(writer, new object[] { msg });
        }

        private void WriteNotify(string msg)
        {
            toolStripStatusLabelNotify.Text = msg;
        }

        public void UpdateNotify(string msg)
        {
            WriteDelegate writer = new WriteDelegate(WriteNotify);
            this.Invoke(writer, new object[] { msg });
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            if (_injector == null)
            {
                _injector = new Injector(this);
                _injector.injectDll();
            }
        }

        delegate void UpdateLableStartGameButtonDelegate(bool flag);

        private void WriteLabelStartGameButton(bool flag)
        {
            if (flag)
            {
                StartGameButton.Text = "Start Game More";
                StartGameButton.Visible = false;
            }
            else
            {
                StartGameButton.Text = "Start Game";
                _injector = null;
                StartGameButton.Visible = true;
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

        private void WriteCharName(string name)
        {
            lblCharName.Text = name;
        }

        public void UpdateCharName(string name)
        {
            WriteDelegate writer = new WriteDelegate(WriteCharName);
            this.Invoke(writer, new object[] { name });
        }

        delegate void UpdateProgressbarDelegate(uint maxHP, uint valueHP, uint maxMP, uint valueMP);

        private void ChangeProgressBarValue(uint maxHP, uint valueHP, uint maxMP, uint valueMP)
        {
            pbCharHP.Maximum = (int) maxHP;
            pbCharMP.Maximum = (int) maxMP;
            pbCharHP.Value = (int) valueHP;
            pbCharMP.Value = (int) valueMP;
        }

        public void UpdateProgressBarValue(uint maxHP, uint valueHP, uint maxMP, uint valueMP)
        {
            UpdateProgressbarDelegate changer = new UpdateProgressbarDelegate(ChangeProgressBarValue);
            this.Invoke(changer, new object[] { maxHP, valueHP, maxMP, valueMP });
        }

        delegate void UpdateLogDelegate(string msg);

        private void WriteLog(string msg)
        {
            rtbLog.AppendText(msg + Environment.NewLine);
        }

        public void UpdateLog(string msg)
        {
            UpdateLogDelegate writer = new UpdateLogDelegate(WriteLog);
            this.Invoke(writer, new object[] { msg });
        }

        delegate void UpdateCoordDelegate(string msg);

        private void WriteCoord(string coord)
        {
            lblCharCoord.Text = coord;
        }

        public void UpdateCoord(string coord)
        {
            UpdateCoordDelegate writer = new UpdateCoordDelegate(WriteCoord);
            this.Invoke(writer, new object[] { coord });
        }
    }
}
