using System;
using System.Windows.Forms;
using System.Management;
using Binarysharp.MemoryManagement;
using System.Diagnostics;
using VanillaSSinn3r.Properties;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;

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

		public static IntPtr[] CamDistAddy = new IntPtr[2];

		public static int SelectedProcess;

		public static string Version;

		public static float GameDefaultNameplateRange;

		public static float GameDefaultFOV;

		public static float GameDefaultCameraDistance;

		public static float GameDefaultCameraDistanceLimit;

		public static bool versionChecked = false;

		public static string warningText = "=== Need to Enter in the World First Then Use This Tool ===";

		public static IntPtr tmpPtr;

		ThreadStart delegateRetrieveData;

		Thread mainThread;

		public Form1()
		{
			InitializeComponent();
			Text = "WoW Vanilla/TBC/WotLK Tool By SSinist3r";
			Load += new EventHandler(Form1_Load);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			FormClosing += new FormClosingEventHandler(Form1_FormClosing);
			ProcessWatcherInit();
			Init();
			Print(warningText);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!Form1.Attached)
			{
				return;
			}
			Form1.AppClosing = true;
			save_n_reset();
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
				Print("Nameplate Range: " + (value).ToString("F") + " yards");
			else if (forText.Contains("fov"))
				Print("FOV: " + value + "	(Use In Game: \' /console ReloadUI \')");
			else if (forText.Contains("camera"))
				Print("Camera Distance: " + value);
		}


		private void namePlateRangeSlider_Scroll(object sender, EventArgs e)
        {
			if (!Form1.Attached || !namePlateCheckBox.Checked)
			{
				return;
			}
			Form1.Sharp.Write<float>(Form1.RangeAddy, (float)(Math.Pow(namePlateRangeSlider.Value, 2)), false);
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
				infoBox.Text = "";
				Print("==================== Attaching ====================");
				Form1.SelectedProcess = int.Parse(GetSubstringByString("[", "]", processBox.SelectedItem.ToString()));
				Form1.Version = GetFileVerion(Form1.SelectedProcess);
				Print("Attaching to WoW with id [" + Form1.SelectedProcess + "]...");
				Form1.Sharp = new MemorySharp(Form1.SelectedProcess);
				Form1.Attached = true;
				Print("Successfully attached to WoW [" + Form1.SelectedProcess + "]");
				Hacks();
				if (Form1.Attached)
				{
					namePlateCheckBox.Checked = Settings.Default.DefaultRangeBool;
					fovCheckBox.Checked = Settings.Default.DefaultFOVBool;
					camDistCheckBox.Checked = Settings.Default.DefaultCamDistBool;
					freezeCheckBox.Checked = Settings.Default.DefaultFreezeCamBool;
					if (namePlateCheckBox.Checked)
                    {
						Form1.Sharp.Write<float>(Form1.RangeAddy, (float)(Math.Pow(Settings.Default.DefaultRange, 2)), false);
						namePlateRangeSlider.Value = (int)(Math.Sqrt(Form1.Sharp.Read<float>(Form1.RangeAddy, false)));
						infoPrint("nameplate", namePlateRangeSlider.Value);

					}
					if(fovCheckBox.Checked)
                    {
						Form1.Sharp.Write<float>(Form1.FOVAddy, (float)Settings.Default.DefaultFOV, false);
						fovTextBox.Text = (float)Form1.Sharp.Read<float>(Form1.FOVAddy, false) + "";
						infoPrint("fov", float.Parse(fovTextBox.Text));
					}
					if (camDistCheckBox.Checked)
					{
						try
						{
							Form1.Sharp.Write<float>(Form1.CamDistAddy[0], (float)Settings.Default.DefaultCamDist, false);
							Form1.Sharp.Write<float>(Form1.CamDistAddy[1], (float)Settings.Default.DefaultCamDist, false);
							camDistSlider.Value = (int)Form1.Sharp.Read<float>(Form1.CamDistAddy[0], false);
							infoPrint("camera", camDistSlider.Value);
						}
						catch
						{ 
							Print(warningText);
							Clipboard.SetText(warningText);
							Application.Exit(); 
						}
					}
					//the methods that will be executed by the main thread is "retrieveData"
					delegateRetrieveData = new ThreadStart(cameraDistHack);
					mainThread = new Thread(delegateRetrieveData);
					mainThread.IsBackground = true;

					//start the main thread
					mainThread.Start();
				}
			}
			catch (Exception ex)
			{
				if (debugCheckBox.Checked)
					Print(ex.Message);
			}
		}

		private void Hacks()
        {
			try
            {
				if (Form1.Version.Contains("5875")) // Vanilla
				{
					Form1.RangeAddy = new IntPtr(12900744); // 0xC4D988
					Form1.FOVAddy = new IntPtr(8423860); // 0x8089B4
					tmpPtr = getAddress(0xB4B2BC, new List<int> { 0x65B8, 0x198 });
					if (tmpPtr != IntPtr.Zero)
						Form1.CamDistAddy[0] = tmpPtr;
					tmpPtr = getAddress(0xB4B2BC, new List<int> { 0x65B8, 0xEC });
					if (tmpPtr != IntPtr.Zero)
						Form1.CamDistAddy[1] = tmpPtr;

					if (!versionChecked)
					{
						Print("World of Warcraft [" + Form1.Version + "] detected!");
						GameDefaultNameplateRange = (float)Form1.Sharp.Read<float>(Form1.RangeAddy, false);
						GameDefaultFOV = (float)Form1.Sharp.Read<float>(Form1.FOVAddy, false);
						GameDefaultCameraDistanceLimit = (float)Form1.Sharp.Read<float>(Form1.CamDistAddy[0], false);
						GameDefaultCameraDistance = (float)Form1.Sharp.Read<float>(Form1.CamDistAddy[1], false);
						versionChecked = true;
					}
				}
				else if (Form1.Version.Contains("8606")) // TBC
				{
					Form1.RangeAddy = new IntPtr(12209040); // 0xBA4B90
					Form1.FOVAddy = new IntPtr(9132548); // 0x8b5a04
					tmpPtr = getAddress(0xC6ECCC, new List<int> { 0x732C, 0x1B4 });
					if (tmpPtr != IntPtr.Zero)
						Form1.CamDistAddy[0] = tmpPtr;
					tmpPtr = getAddress(0xC6ECCC, new List<int> { 0x732C, 0x100 });
					if (tmpPtr != IntPtr.Zero)
						Form1.CamDistAddy[1] = tmpPtr;
					if (!versionChecked)
                    {
						Print("World of Warcraft [" + Form1.Version + "] detected!");
						GameDefaultNameplateRange = (float)Form1.Sharp.Read<float>(Form1.RangeAddy, false);
						GameDefaultFOV = (float)Form1.Sharp.Read<float>(Form1.FOVAddy, false);
						GameDefaultCameraDistanceLimit = (float)Form1.Sharp.Read<float>(Form1.CamDistAddy[0], false);
						GameDefaultCameraDistance = (float)Form1.Sharp.Read<float>(Form1.CamDistAddy[1], false);
						versionChecked = true;
					}						
				}
				else if (Form1.Version.Contains("12340")) // Wotlk
				{
					Form1.RangeAddy = new IntPtr(11381372); // 0xADAA7C
					Form1.FOVAddy = new IntPtr(10390920); // 0x9e8d88
					tmpPtr = getAddress(0xB7436C, new List<int> { 0x7E20, 0x1E8 });
					if (tmpPtr != IntPtr.Zero)
						Form1.CamDistAddy[0] = tmpPtr;
					tmpPtr = getAddress(0xB7436C, new List<int> { 0x7E20, 0x118 });
					if (tmpPtr != IntPtr.Zero)
						Form1.CamDistAddy[1] = tmpPtr;
					if (!versionChecked)
                    {
						Print("World of Warcraft [" + Form1.Version + "] detected!");
						GameDefaultNameplateRange = (float)Form1.Sharp.Read<float>(Form1.RangeAddy, false);
						GameDefaultFOV = (float)Form1.Sharp.Read<float>(Form1.FOVAddy, false);
						GameDefaultCameraDistanceLimit = (float)Form1.Sharp.Read<float>(Form1.CamDistAddy[0], false);
						GameDefaultCameraDistance = (float)Form1.Sharp.Read<float>(Form1.CamDistAddy[1], false);
						versionChecked = true;
					}						
				}
				else
				{
					Print("World of Warcraft [" + Form1.Version + "] is not supported.");
					Form1.Sharp.Handle.Close();
					Form1.Sharp.Dispose();
					Form1.Attached = false;
					Print("Detached from WoW [" + Form1.SelectedProcess + "]");
				}
			}
			catch (Exception ex)
			{
				if(debugCheckBox.Checked)
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
			Clipboard.SetText("/console ReloadUI");
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

        private void resetBtn_Click(object sender, EventArgs e)
        {
			if (!Form1.Attached)
			{
				return;
			}
			infoBox.Text = "";
			Print("======= Resetting Values To Game Default/Original =======");
			save_n_reset();
			infoPrint("nameplate", (float)Math.Sqrt(GameDefaultNameplateRange));
			infoPrint("fov", GameDefaultFOV);
			infoPrint("camera", GameDefaultCameraDistanceLimit);
			Print("==================== Detached ====================");
		}

		private void save_n_reset()
		{
			Hacks();
			Form1.Attached = false;
			versionChecked = false;
			if (freezeCheckBox.Checked)
            {
				freezeCheckBox.Checked = false;
				try
				{
					if (mainThread.IsAlive)
						mainThread.Abort();
				}
				catch { }
				freezeCheckBox.Checked = true;
			}
			saveSettings();
			Form1.Sharp.Write<float>(Form1.RangeAddy, GameDefaultNameplateRange, false);
			Form1.Sharp.Write<float>(Form1.FOVAddy, GameDefaultFOV, false);
            Form1.Sharp.Write<float>(Form1.CamDistAddy[0], GameDefaultCameraDistanceLimit, false);
			Form1.Sharp.Write<float>(Form1.CamDistAddy[1], GameDefaultCameraDistance, false);
		}

		private void saveSettings()
		{
			if (namePlateCheckBox.Checked)
				Settings.Default.DefaultRange = (int)namePlateRangeSlider.Value;
			if (fovCheckBox.Checked)
				Settings.Default.DefaultFOV = float.Parse(fovTextBox.Text);
			if (camDistCheckBox.Checked)
				Settings.Default.DefaultCamDist = (int)camDistSlider.Value;
			Settings.Default.DefaultRangeBool = namePlateCheckBox.Checked;
			Settings.Default.DefaultFOVBool = fovCheckBox.Checked;
			Settings.Default.DefaultCamDistBool = camDistCheckBox.Checked;
			Settings.Default.DefaultFreezeCamBool = freezeCheckBox.Checked;
			Settings.Default.Save();
		}
		private void saveBtn_Click(object sender, EventArgs e)
        {
			saveSettings();
			Print("================== Settings Saved ==================");
		}

		private IntPtr getAddress(int baseM, List<int> offsets)
		{
			try
			{
				IntPtr Base = (IntPtr)baseM;
				foreach (int ptr in offsets)
					Base = Form1.Sharp.Read<IntPtr>(Base, false) + ptr;

				return Base;
			}
			catch (Exception ex)
			{
				if (debugCheckBox.Checked)
					Print(ex.Message);
				return IntPtr.Zero;
			}
		}

		private void cameraDistHack()
		{
			try
			{
				while (true)
				{
					if (freezeCheckBox.Checked)
					{
						camDistSlider.Enabled = false;
						camDistHackCode();


					}
					else
					{
						camDistHackCode();
						camDistSlider.Enabled = true;
						break;
					}
				}
			}
			catch { }				
		}

		private void camDistHackCode()
        {
			if (!Form1.Attached || !camDistCheckBox.Checked)
			{
				return;
			}
			Hacks();
			Form1.Sharp.Write<float>(CamDistAddy[0], (float)camDistSlider.Value, false);
			Form1.Sharp.Write<float>(CamDistAddy[1], (float)camDistSlider.Value, false);
			if (!Form1.AppClosing)
			{
				Settings.Default.DefaultCamDist = camDistSlider.Value;
			}
			Settings.Default.Save();
		}

		private void camDistSlider_Scroll(object sender, EventArgs e)
        {
			if (!Form1.Attached || !camDistCheckBox.Checked)
			{
				return;
			}
			cameraDistHack();
			infoPrint("camera", camDistSlider.Value);
		}

        private void camDistCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			Settings.Default.DefaultCamDistBool = camDistCheckBox.Checked;
			Settings.Default.Save();
			camDistSlider.Value = Settings.Default.DefaultCamDist;
		}

        private void freezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			Settings.Default.DefaultFreezeCamBool = freezeCheckBox.Checked;
			Settings.Default.Save();
			if (!Form1.Attached)
				return;
			if (freezeCheckBox.Checked)
			{ 
				camDistSlider.Enabled = false;
				try
				{
					if (!mainThread.IsAlive)
					{
						delegateRetrieveData = new ThreadStart(cameraDistHack);
						mainThread = new Thread(delegateRetrieveData);
						mainThread.IsBackground = true;
						mainThread.Start();
					}
				}
				catch { }
			}
			else if (!freezeCheckBox.Checked)
            {
				camDistSlider.Enabled = true;
				try
				{
					if (mainThread.IsAlive)
						mainThread.Abort();
				}
				catch { }
			}
		}

        private void infoBox_TextChanged(object sender, EventArgs e)
        {}
        private void debugCheckBox_CheckedChanged(object sender, EventArgs e)
        {}
    }
}
