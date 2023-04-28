using System;

namespace ControlRoomApplication.GUI.Data
{
	public class RTControlFormData
	{
		public int controlScriptIndex { get; set; }
		public int spectraCyberScanIndex { get; set; }
		public string frequency { get; set; }
		public int DCGainIndex { get; set; }
		public string IFGain { get; set; }
		public string offsetVoltage { get; set; }
		public int integrationStepIndex { get; set; }
		public double azSpeed { get; set; }
		public double elSpeed { get; set; }
		public bool controlledStopBool { get; set; }
		public bool immediateStopBool { get; set; }
		public bool manualControlEnabled { get; set; }
		public bool freeControlEnabled { get; set; }
		public bool scanEnabled { get; set; }


		public RTControlFormData(int controlScriptIndex, int spectraCyberScanIndex, string frequency, int DCGainIndex, string IFGain, string offsetVoltage, 
			int integrationStepIndex, double azSpeed, double elSpeed, bool controlledStopBool, bool immediateStopBool, bool manualControlEnabled, bool freeControlEnabled, bool scanEnabled)
        {
			this.controlScriptIndex = controlScriptIndex;
			this.spectraCyberScanIndex = spectraCyberScanIndex;
			this.frequency = frequency;
			this.DCGainIndex = DCGainIndex;
			this.IFGain = IFGain;
			this.offsetVoltage = offsetVoltage;
			this.integrationStepIndex = integrationStepIndex;
			this.azSpeed = azSpeed;
			this.elSpeed = elSpeed;
			this.immediateStopBool = immediateStopBool;
			this.controlledStopBool = controlledStopBool;
			this.manualControlEnabled = manualControlEnabled;
			this.freeControlEnabled = freeControlEnabled;
			this.scanEnabled = scanEnabled;
		}

		public RTControlFormData()
        {
			this.controlScriptIndex = 0;
			this.spectraCyberScanIndex = 0;
			this.frequency = "" ;
			this.DCGainIndex = 0;
			this.IFGain = "";
			this.offsetVoltage = "";
			this.integrationStepIndex = 0;
			this.azSpeed = 0;
			this.elSpeed = 0;
			this.controlledStopBool = true;
			this.immediateStopBool = false;
			this.manualControlEnabled = false;
			this.freeControlEnabled = false;
			this.scanEnabled = false;

		}

		

	}













}

