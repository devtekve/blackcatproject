using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SilkroadProxyWithForms;

namespace StartGamePlatformInvoke
{
    class Injector
    {

        #region fields

        private string _currentpath;
        private string _configpath;
        private string _listenport;
        private string _gamepath;
        private string _dllpath;
        private StringBuilder _buffer;
        private MainForm _mainForm;

        #endregion

        #region properties

        public string CurrentPath
        {
            get { return _currentpath; }
            set { _currentpath = value; }
        }

        public string ConfigPath
        {
            get { return _configpath; }
            set { _configpath = value; }
        }

        public string ListenPort
        {
            get { return _listenport; }
            set { _listenport = value; }
        }

        public string GamePath
        {
            get { return _gamepath; }
            set { _gamepath = value; }
        }

        public string DllPath
        {
            get { return _dllpath; }
            set { _dllpath = value; }
        }

        #endregion

        const UInt32 INFINITE = 0xFFFFFFFF;
        const UInt32 WAIT_ABANDONED = 0x00000080;
        const UInt32 WAIT_OBJECT_0 = 0x00000000;
        const UInt32 WAIT_TIMEOUT = 0x00000102;
        const UInt32 CREATE_SUSPENDED = 0x00000004;

        public Injector(MainForm mainForm)
        {
            _buffer = new StringBuilder(3333);
            _currentpath = Environment.CurrentDirectory;
            _mainForm = mainForm;

            this._configpath = this._currentpath + @"\Config.ini";
            this._dllpath = this._currentpath + @"\Detour.dll";

            NativeMethods.WritePrivateProfileString("Config", "Port", "15778", _configpath);
        }

        public void injectDll()
        {
            NativeMethods.GetPrivateProfileString("Config", "Path", "", _buffer, (uint)_buffer.Capacity, _configpath);
            _gamepath = _buffer.ToString();

            //create suspend process
            string cmdLine = "\"" + GamePath + "\"" + "0 /23 0 0";
            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            STARTUPINFO si = new STARTUPINFO();
            SECURITY_ATTRIBUTES pSec = new SECURITY_ATTRIBUTES();
            SECURITY_ATTRIBUTES tSec = new SECURITY_ATTRIBUTES();
            pSec.nLength = Marshal.SizeOf(pSec);
            tSec.nLength = Marshal.SizeOf(tSec);
            bool result = NativeMethods.CreateProcess(null, cmdLine, ref pSec, ref tSec, false, CREATE_SUSPENDED, IntPtr.Zero, null, ref si, out pi);
            if (result == false)
            {
                OpenFileDialog ofd = null;
                try
                {
                    //MessageBox.Show("Error : could not start sro_client.exe");
                    ofd = new OpenFileDialog();
                    ofd.DefaultExt = "exe";
                    ofd.Filter = "| sro_client.exe";
                    ofd.Multiselect = false;
                    ofd.Title = "Select sro_client.exe ...";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string name = ofd.FileName;
                        NativeMethods.WritePrivateProfileString("Config", "Path", name, _configpath);
                        injectDll();
                    }
                }
                finally
                {
                    ofd.Dispose();
                }

                return;
            }

            //create virtual stub memory and injection thread
            IntPtr lpAlloc = NativeMethods.VirtualAllocEx(pi.hProcess, IntPtr.Zero, (uint)DllPath.Length, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);
            UIntPtr temp = UIntPtr.Zero;
            NativeMethods.WriteProcessMemory(pi.hProcess, lpAlloc, Encoding.ASCII.GetBytes(DllPath), (uint)DllPath.Length, out temp);
            uint lpThreadId;
            IntPtr hThread = NativeMethods.CreateRemoteThread(pi.hProcess, IntPtr.Zero, 0, NativeMethods.GetProcAddress(NativeMethods.GetModuleHandle("kernel32.dll"), "LoadLibraryA"), lpAlloc, 0, out lpThreadId);
            NativeMethods.WaitForSingleObject(hThread, INFINITE);

            //exit thread injected
            uint dwExitCode;
            NativeMethods.GetExitCodeThread(hThread, out dwExitCode);
            NativeMethods.CloseHandle(hThread);

            //free virtual stub memory
            NativeMethods.VirtualFreeEx(pi.hProcess, lpAlloc, DllPath.Length, FreeType.Decommit);

            //disable the suspend status of sro_client process
            NativeMethods.ResumeThread(pi.hThread);
            NativeMethods.CloseHandle(pi.hThread);
            NativeMethods.CloseHandle(pi.hProcess);

            if (dwExitCode == 0)
            {
                MessageBox.Show("injected fail !");
            }
           
        }
    }

}
