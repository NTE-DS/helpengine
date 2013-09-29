﻿/***************************************************************************************************
 * NasuTek Developer Studio
 * Copyright (C) 2005-2013 NasuTek Enterprises
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 ***************************************************************************************************/

using NasuTek.DevEnvironment.Resources;
using NasuTek.DevEnvironment.Resources.Addins;
using ProtocolPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocExplorer.Resources
{
    public class InitializeResources : ICommand
    {
        public object Owner { get; set; }

        public void Run()
        {
            ResourceService.RegisterImages("DocExplorer.Resources.Properties.Resources", typeof(InitializeResources).Assembly);
        }

        public event EventHandler OwnerChanged;
    }
}