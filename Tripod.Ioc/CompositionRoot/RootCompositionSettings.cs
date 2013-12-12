﻿using System.Reflection;

namespace Tripod.Ioc
{
    public class RootCompositionSettings
    {
        public bool IsGreenfield { get; set; }
        public Assembly[] FluentValidatorAssemblies { get; set; }
        public Assembly[] QueryHandlerAssemblies { get; set; }
        public Assembly[] CommandHandlerAssemblies { get; set; }
    }
}
