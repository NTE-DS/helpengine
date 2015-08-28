using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocExplorer.Resources
{
	public partial class HelpVersion {
		public const string Codename = "Melinda";
		public const string GitBranch = "master";
		public const string GitRevision = "b0b847f57234";
		public const string DateStamp = "20150828_1258";
		public const string LabID = "ndemain";
		public const string BuildStage = "RC1";
		public const string BuildLab = GitBranch + "-" + GitRevision + "_" + Codename + "-" + LabID + "_" + DateStamp;
    }
}
