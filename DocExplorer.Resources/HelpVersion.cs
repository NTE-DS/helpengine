﻿/***************************************************************************************************
 * NasuTek Developer Studio Development Environment Core DLL
 * Copyright (C) 2005-2014 NasuTek Enterprises
 * Window Docking Portions Copyright (C) 2007-2012 Weifen Luo (email: weifenluo@yahoo.com)
 * Addin Engine Portions Copyright (C) 2001-2012 AlphaSierraPapa for the SharpDevelop Team
 *
 * This library is free software; you can redistribute it and/or modify it under the terms of the 
 * GNU Library General Public License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public License along with this 
 * library; if not, write to the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 ***************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocExplorer.Resources
{
    public partial class HelpVersion {
        public const string MajorCodebase = "5";
        public const string MinorCodebase = "2";
        public const string BuildCodebase = "0";
        public const string RevisionCodebase = "0";
        public const string Major = "2";
        public const string Minor = "0";
        public const string Build = "0";
        public const string Revision = "0";
        public const string ReleaseVersion = Major + "." + Minor + "." + Build + "." + Revision;
        public const string CodebaseVersion = MajorCodebase + "." + MinorCodebase + "." + BuildCodebase + "." + RevisionCodebase;
        public const string FullVersion = CodebaseVersion + "-" + BuildStage;
    }
}
