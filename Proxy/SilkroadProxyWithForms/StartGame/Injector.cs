using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

        #region P/invoke members

        const UInt32 INFINITE = 0xFFFFFFFF;
        const UInt32 WAIT_ABANDONED = 0x00000080;
        const UInt32 WAIT_OBJECT_0 = 0x00000000;
        const UInt32 WAIT_TIMEOUT = 0x00000102;
        const UInt32 CREATE_SUSPENDED = 0x00000004;

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern uint GetPrivateProfileString(
           string lpAppName,
           string lpKeyName,
           string lpDefault,
           StringBuilder lpReturnedString,
           uint nSize,
           string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFileName);

        [DllImport("kernel32.dll")]
        static extern bool CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            AllocationType flAllocationType,
            MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            uint nSize,
            out UIntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(
          IntPtr hProcess,
          IntPtr lpThreadAttributes,
          uint dwStackSize,
          UIntPtr lpStartAddress, // raw Pointer into remote process
          IntPtr lpParameter,
          uint dwCreationFlags,
          out uint lpThreadId
        );

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        [DllImport("kernel32.dll")]
        static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        static extern uint ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            int dwSize,
            FreeType dwFreeType);


        #endregion


        public Injector()
        {
            _buffer = new StringBuilder(3333);
            _currentpath = Environment.CurrentDirectory;

            this._configpath = this._currentpath + @"\Config.ini";
            this._dllpath = this._currentpath + @"\Detour.dll";

            WritePrivateProfileString("Config", "Port", "15778", _configpath);
        }

        public void injectDll()
        {
            GetPrivateProfileString("Config", "Path", "", _buffer, (uint)_buffer.Capacity, _configpath);
            _gamepath = _buffer.ToString();

            //create suspend process
            string cmdLine = "\"" + GamePath + "\"" + "0 /23 0 0";
            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            STARTUPINFO si = new STARTUPINFO();
            SECURITY_ATTRIBUTES pSec = new SECURITY_ATTRIBUTES();
            SECURITY_ATTRIBUTES tSec = new SECURITY_ATTRIBUTES();
            pSec.nLength = Marshal.SizeOf(pSec);
            tSec.nLength = Marshal.SizeOf(tSec);
            bool result = CreateProcess(null, cmdLine, ref pSec, ref tSec, false, CREATE_SUSPENDED, IntPtr.Zero, null, ref si, out pi);
            if (result == false)
            {
                //MessageBox.Show("Error : could not start sro_client.exe");
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = "exe";
                ofd.Filter = "| sro_client.exe";
                ofd.Multiselect = false;
                ofd.Title = "Select sro_client.exe ...";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string name = ofd.FileName;
                    WritePrivateProfileString("Config", "Path", name, _configpath);
                    injectDll();
                }
                else
                {
                    return;
                }
                return;
            }

            //create virtual stub memory and injection thread
            IntPtr lpAlloc = VirtualAllocEx(pi.hProcess, IntPtr.Zero, (uint)DllPath.Length, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);
            UIntPtr temp = UIntPtr.Zero;
            WriteProcessMemory(pi.hProcess, lpAlloc, Encoding.ASCII.GetBytes(DllPath), (uint)DllPath.Length, out temp);
            uint lpThreadId;
            IntPtr hThread = CreateRemoteThread(pi.hProcess, IntPtr.Zero, 0, GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA"), lpAlloc, 0, out lpThreadId);
            WaitForSingleObject(hThread, INFINITE);

            //exit thread injected
            uint dwExitCode;
            GetExitCodeThread(hThread, out dwExitCode);
            CloseHandle(hThread);

            //free virtual stub memory
            VirtualFreeEx(pi.hProcess, lpAlloc, DllPath.Length, FreeType.Decommit);

            //disable the suspend status of sro_client process
            ResumeThread(pi.hThread);
            CloseHandle(pi.hThread);
            CloseHandle(pi.hProcess);

            if (dwExitCode == 0)
            {
                MessageBox.Show("injected fail !");
            }
        }
    }

    #region support for P/invoke members

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct STARTUPINFO
    {
        public Int32 cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public Int32 dwX;
        public Int32 dwY;
        public Int32 dwXSize;
        public Int32 dwYSize;
        public Int32 dwXCountChars;
        public Int32 dwYCountChars;
        public Int32 dwFillAttribute;
        public Int32 dwFlags;
        public Int16 wShowWindow;
        public Int16 cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public int dwProcessId;
        public int dwThreadId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES
    {
        public int nLength;
        public IntPtr lpSecurityDescriptor;
        public int bInheritHandle;
    }

    [Flags]
    public enum FreeType
    {
        Decommit = 0x4000,
        Release = 0x8000,
    }

    [Flags]
    public enum AllocationType
    {
        Commit = 0x1000,
        Reserve = 0x2000,
        Decommit = 0x4000,
        Release = 0x8000,
        Reset = 0x80000,
        Physical = 0x400000,
        TopDown = 0x100000,
        WriteWatch = 0x200000,
        LargePages = 0x20000000
    }

    [Flags]
    public enum MemoryProtection
    {
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
        ExecuteWriteCopy = 0x80,
        NoAccess = 0x01,
        ReadOnly = 0x02,
        ReadWrite = 0x04,
        WriteCopy = 0x08,
        GuardModifierflag = 0x100,
        NoCacheModifierflag = 0x200,
        WriteCombineModifierflag = 0x400
    }

    #endregion
    
}
