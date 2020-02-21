// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace IdentityServer.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "IdentityServer";
        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;
    }
}
