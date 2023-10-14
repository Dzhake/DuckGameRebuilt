﻿using System;
using System.Reflection;

namespace DuckGame.ConsoleEngine
{
    public class Command
    {
        public Func<object[], object> Invoke;
        public string Name;
        public Parameter[] Parameters;

        public Command(string name, Parameter[] parameters, Func<object?[]?, object?> invoke)
        {
            Name = name;
            Parameters = parameters;
            Invoke = invoke;
        }

        /// <param name="methodInfo" />
        public static Command FromMethodInfo(MethodInfo methodInfo)
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            Parameter[] parameters = new Parameter[parameterInfos.Length];

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                ParameterInfo pInfo = parameterInfos[i];
            
                parameters[i] = new Parameter()
                {
                    Name = pInfo.Name ?? "<NULL>",
                    ParameterType = pInfo.ParameterType,
                    IsOptional = pInfo.IsOptional,
                    DefaultValue = pInfo.DefaultValue,
                    IsParams = pInfo.IsDefined(typeof(ParamArrayAttribute), false),
                };
            }

            return new Command(methodInfo.Name, parameters, args => methodInfo.Invoke(null, args));
        }

        public class Parameter
        {
            public string Name;
            public Type ParameterType;
            public bool IsOptional = false;
            public object? DefaultValue = null;
            public bool IsParams = false;
        }
    }
}