using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MLE4WCSharp
{
	/// <summary> 
	/// This class is a wrapper around all the provided MATLAB scripts provided by the university staaf for the EPO4 project.
	/// </summary> 
	/// <remarks> 
	/// You should provide all the scripts in one folder.
	/// </remarks> 
    public class MATLABWrapper : IDisposable
    {
		MLApp.MLApp matlab;
		// Flag: Has Dispose already been called? 
		bool disposed = false;
		// The group indentifier on some figures
		String groupID;
		// Directory with all the matlab code.
		DirectoryInfo mcodepath;
		// Used for function calles that return no value
		object Dummy = null;

		public MATLABWrapper(string _mcodepath, string _groupID)
		{
			groupID = _groupID;
			mcodepath = new DirectoryInfo(_mcodepath);
			if (!mcodepath.Exists)
			{
				throw new Exception("The mcode folder can not be found.");
			}
			var activationContext = Type.GetTypeFromProgID("matlab.application.single");
			matlab = (MLApp.MLApp)Activator.CreateInstance(activationContext);
			matlab.Visible = 0;
			String output;
			output= matlab.Execute("cd " + mcodepath.FullName);
			output=matlab.Execute("pwd");
			var currentDirCheck = matlab.GetVariable("ans", "base");
			DirectoryInfo currentDirCheckDi = new DirectoryInfo(currentDirCheck);
			if (currentDirCheckDi.FullName != mcodepath.FullName)
			{
				throw new Exception("Changing MATLAB working directory to mcode directory failed. Working directory stuck at " + currentDirCheck);
			}
		}
		public MATLABWrapper() : this("mcode","EPO4 Group") { }

		~MATLABWrapper()
		{
			Dispose(false);
		}
		/// <summary>
		/// Kills the MATLAB instance and frees all other resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		// Protected implementation of Dispose pattern. 
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				// Free any other managed objects here. 
				//
				matlab.Quit();
				matlab = null;
			}

			// Free any unmanaged objects here. 
			//			
			disposed = true;
		}

		protected string matlabCmd(string command)
		{
			return matlab.Execute(command);
		}

		#region Public MATLAB methods
		/// <summary>
		/// Executes text command on the matlab instance. Know what you are doing!
		/// </summary>
		public bool MATLAB_Command(string command, out string result)
		{
			result = matlabCmd(command);
			return true;
		}
		/// <summary>
		/// Executes text command on the matlab instance. Know what you are doing! When you don't want the result.
		/// </summary>
		public bool MATLAB_Command(string command)
		{
			matlabCmd(command);
			return true;
		}
		
		/// <summary>
		/// EPO-4: script for fitting range-power radar equation
		/// MTSR, EWI, TU Delft, 2012
		/// contact: o.a.krasnov@tudelft.nl
		/// </summary>
		/// <param name="ranges">Your set of ranges.</param>
		/// <param name="power">Your set of measured signal amplitudes.</param>
		public bool radar_epo_4(double[] ranges, double[] power)
		{
			matlab.Feval("radar_epo_4", 0, out Dummy, ranges, power, 0);
			Dummy = null;
			return true;
		}
		/// <summary>
		/// Clears the MATLAB workspace
		/// </summary>
		public bool ClearAll()
		{
			matlabCmd("clear all;");
			return true;
		}
		/// <summary>
		/// Closes all figures.
		/// </summary>
		public bool CloseAll()
		{
			matlabCmd("close all;");
			return true;
		}		
		#endregion
	}
}
