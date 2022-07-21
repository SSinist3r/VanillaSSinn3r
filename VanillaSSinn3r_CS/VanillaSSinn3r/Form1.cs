using System;
using System.Windows.Forms;
using System.Management;
using Binarysharp.MemoryManagement;
using System.Diagnostics;
using VanillaSSinn3r.Properties;
using System.Collections;

namespace VanillaSSinn3r
{
	public partial class Form1 : Form
	{
		public static Process[] Proc;

		public static MemorySharp Sharp;

		public static bool Attached;

		public static bool AppClosing;

		public static IntPtr RangeAddy;

		public static IntPtr FOVAddy;

		public static int SelectedProcess;

		public static string Version;

		public Form1()
		{
			InitializeComponent();
			Text = "WoW Vanilla/TBC Tool By SSinist3r";
			Load += new EventHandler(Form1_Load);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			FormClosing += new FormClosingEventHandler(Form1_FormClosing);
			ProcessWatcherInit();
			Init();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!Form1.Attached)
			{
				return;
			}
			Form1.AppClosing = true;
			if(namePlateCheckBox.Checked)
				Settings.Default.DefaultRange = (int)namePlateRangeSlider.Value;
			if(fovCheckBox.Checked)
				Settings.Default.DefaultFOV = float.Parse(fovTextBox.Text);
			Settings.Default.DefaultRangeBool = namePlateCheckBox.Checked;
			Settings.Default.DefaultFOVBool = fovCheckBox.Checked;
			Settings.Default.Save();
			Form1.Sharp.Write<float>(Form1.RangeAddy, 400f, false);
			Form1.Sharp.Write<float>(Form1.FOVAddy, 1.5708f, false);
		}

		public static bool InProcIgnoreList(string processName)
		{
			string[] procIgnoreList = { "wowreeb" };
			foreach (string procIgnore in procIgnoreList)
            {
				if(processName.ToLower().Contains(procIgnore.ToLower()))
				{ return true; }
            }
			return false;
		}

		public static Process[] GetProcessesByName(string processName, string machineName)
		{
			if (processName == null)
			{
				processName = string.Empty;
			}

			Process[] processes = Process.GetProcesses(machineName);
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < processes.Length; i++)
			{
				if (processes[i].ProcessName.ToLower().Contains(processName.ToLower()) && !InProcIgnoreList(processes[i].ProcessName))
				{
					arrayList.Add(processes[i]);
				}
				else
				{
					processes[i].Dispose();
				}
			}

			Process[] array = new Process[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		private void Init()
		{
			processBox.Invoke(new MethodInvoker(delegate ()
			{
				processBox.Items.Clear();
			}));
			// Form1.Proc = Process.GetProcessesByName("Wow");
			Form1.Proc = GetProcessesByName("Wow", ".");
			if (Form1.Proc.Length == 0)
			{
				return;
			}
			Process[] proc = Form1.Proc;
			for (int i = 0; i < proc.Length; i++)
			{
				Process instance = proc[i];
				processBox.Invoke(new MethodInvoker(delegate ()
				{
					processBox.Items.Add(instance.ProcessName + " [" + instance.Id + "]");
				}));
			}
			processBox.Invoke(new MethodInvoker(delegate ()
			{
				processBox.SelectedIndex = 0;
			}));
		}

		public void ProcessWatcherInit()
		{
			ManagementEventWatcher managementEventWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
			managementEventWatcher.EventArrived += OnProcessStart;
			managementEventWatcher.Start();
			ManagementEventWatcher managementEventWatcher2 = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
			managementEventWatcher2.EventArrived += OnProcessStop;
			managementEventWatcher2.Start();
		}

		public void OnProcessStart(object sender, EventArrivedEventArgs e)
		{
			if (e.NewEvent.Properties["ProcessName"].Value.ToString().ToLower().Contains("wow")) // == "wow.exe")
			{
				Init();
			}
		}

		public void OnProcessStop(object sender, EventArrivedEventArgs e)
		{
			string text = e.NewEvent.Properties["ProcessName"].Value.ToString();
			int num = int.Parse(e.NewEvent.Properties["ProcessID"].Value.ToString());
			if (text.ToLower().Contains("wow")) // == "wow.exe")
			{
				Init();
			}
			if (!Form1.Attached || num != Form1.SelectedProcess)
			{
				return;
			}
			Print("WoW closed detaching.");
			Form1.Sharp.Handle.Close();
			Form1.Sharp.Dispose();
			Form1.Attached = false;
		}

		public void Print(string msg)
		{
			infoBox.Invoke(new MethodInvoker(delegate ()
			{
				infoBox.AppendText(DateTime.Now.ToString("[hh:mm:ss tt]   ") + msg + Environment.NewLine);
			}));
		}

		public void infoPrint(string forText, float value)
		{
			if(forText.Contains("nameplate"))
				Print("Nameplate Range: " + (int)((value / 100)*2.75) + " yards");
			else if (forText.Contains("fov"))
				Print("FOV: " + value + "	(Use In Game: \' /console ReloadUI \')");
		}


		private void namePlateRangeSlider_Scroll(object sender, EventArgs e)
        {
			if (!Form1.Attached || !namePlateCheckBox.Checked)
			{
				return;
			}
			Form1.Sharp.Write<float>(Form1.RangeAddy, (float)namePlateRangeSlider.Value, false);
			if (!Form1.AppClosing)
			{
				Settings.Default.DefaultRange = namePlateRangeSlider.Value;
				infoPrint("nameplate", namePlateRangeSlider.Value);
			}
			Settings.Default.Save();
		}

        private void refreshBtn_Click(object sender, EventArgs e)
        {
			Init();
		}

        private void attachBtn_Click(object sender, EventArgs e)
        {
			if (Form1.Attached)
			{
				Print("We are already attached to WoW");
				return;
			}
			if (processBox.SelectedIndex == -1)
			{
				Print("You must select a valid instance of WoW to attach");
				return;
			}
			try
			{
				Form1.SelectedProcess = int.Parse(GetSubstringByString("[", "]", processBox.SelectedItem.ToString()));
				Form1.Version = GetFileVerion(Form1.SelectedProcess);
				Print("Attaching to WoW with id [" + Form1.SelectedProcess + "]...");
				Form1.Sharp = new MemorySharp(Form1.SelectedProcess);
				Form1.Attached = true;
				Print("Successfully attached to WoW [" + Form1.SelectedProcess + "]");
				if (Form1.Version.Contains("5875")) // Vanilla
				{
					Print("World of Warcraft [" + Form1.Version + "] detected!");
					Form1.RangeAddy = new IntPtr(12900744); // 0xC4D988
					Form1.FOVAddy = new IntPtr(8423860); // 0x8089B4
				}
				else if (Form1.Version.Contains("8606")) // TBC
				{
					Print("World of Warcraft [" + Form1.Version + "] detected!");
					Form1.RangeAddy = new IntPtr(12209040); // 0xBA4B90
					Form1.FOVAddy = new IntPtr(9132548); // 0x8b5a04
				}
				else if (Form1.Version.Contains("12340")) // Wotlk
				{
					Print("World of Warcraft [" + Form1.Version + "] detected!");
					Form1.RangeAddy = new IntPtr(12209040); // 0xBA4B90 // still need to find this
					Form1.FOVAddy = new IntPtr(10390920); // 0x9e8d88
				}
				else
				{
					Print("World of Warcraft [" + Form1.Version + "] is not supported.");
					Form1.Sharp.Handle.Close();
					Form1.Sharp.Dispose();
					Form1.Attached = false;
					Print("Detached from WoW [" + Form1.SelectedProcess + "]");
				}
				if (Form1.Attached)
				{
					namePlateCheckBox.Checked = Settings.Default.DefaultRangeBool;
					fovCheckBox.Checked = Settings.Default.DefaultFOVBool;
					if (namePlateCheckBox.Checked)
                    {
						Form1.Sharp.Write<float>(Form1.RangeAddy, (float)Settings.Default.DefaultRange, false);
						namePlateRangeSlider.Value = (int)Form1.Sharp.Read<float>(Form1.RangeAddy, false);
						infoPrint("nameplate", namePlateRangeSlider.Value);

					}
					if(fovCheckBox.Checked)
                    {
						Form1.Sharp.Write<float>(Form1.FOVAddy, (float)Settings.Default.DefaultFOV, false);
						fovTextBox.Text = (float)Form1.Sharp.Read<float>(Form1.FOVAddy, false) + "";
						infoPrint("fov", float.Parse(fovTextBox.Text));
					}
				}
			}
			catch (Exception ex)
			{
				Print(ex.Message);
			}
		}

		public string GetFileVerion(int procId)
		{
			return Process.GetProcessById(procId).MainModule.FileVersionInfo.FileVersion;
		}

		public string GetSubstringByString(string a, string b, string c)
		{
			return c.Substring(c.IndexOf(a, StringComparison.Ordinal) + a.Length, c.IndexOf(b, StringComparison.Ordinal) - c.IndexOf(a, StringComparison.Ordinal) - a.Length);
		}

        private void fovSetBtn_Click(object sender, EventArgs e)
        {
			if (!Form1.Attached || !fovCheckBox.Checked)
			{
				return;
			}
			Form1.Sharp.Write<float>(Form1.FOVAddy, float.Parse(fovTextBox.Text), false);
			if (!Form1.AppClosing)
			{
				if(fovTextBox.Text != null)
                {
					Settings.Default.DefaultFOV = float.Parse(fovTextBox.Text);
					infoPrint("fov", float.Parse(fovTextBox.Text));
				}
			}
			Settings.Default.Save();
		}

        private void namePlateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			Settings.Default.DefaultRangeBool = namePlateCheckBox.Checked;
			Settings.Default.Save();
			namePlateRangeSlider.Value = Settings.Default.DefaultRange;
		}

        private void fovCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			Settings.Default.DefaultFOVBool = fovCheckBox.Checked;
			Settings.Default.Save();
			fovTextBox.Text = Settings.Default.DefaultFOV + "";
		}
    }
}
