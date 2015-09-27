using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocExplorer.Resources
{
	public partial class HelpVersion {
		public const string Codename = "Melinda";
		public const string GitBranch = "master";
		public const string GitRevision = "5238795a6fc7";
		public const string DateStamp = "20150828_1314";
		public const string LabID = "ndemain";
		public const string BuildStage = "RC1";
		public const string BuildLab = GitBranch + "-" + GitRevision + "_" + Codename + "-" + LabID + "_" + DateStamp;
    }
}
