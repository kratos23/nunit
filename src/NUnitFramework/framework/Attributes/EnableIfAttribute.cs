// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Framework
{
   /// <summary>
   /// Attribute that can bind to testExecution to some expression, in than way that test fixture or test method get's run only if expression is true.
   /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class EnableIfAttribute : NUnitAttribute, IApplyToTest
    {
        private readonly String _enableProvider;

/// <summary>
/// Creates EnableIfAttribute
/// </summary>
/// <param name="enableProvider">Provider for expression</param>
        public EnableIfAttribute(String enableProvider)
        {
            _enableProvider = enableProvider;
        }

        #region IApplyToTest members
/// <summary>
/// Applyes attribute to test
/// </summary>
/// <param name="test">test to apply</param>
        public void ApplyToTest(Test test)
        {
            bool enable = (bool)CSharpScript.EvaluateAsync(_enableProvider).GetAwaiter().GetResult();
            Console.WriteLine("Apply to test" + enable);
            if (!enable)
            {
                test.RunState = RunState.Skipped;
                test.Properties.Set(PropertyNames.SkipReason, "Test was skipped with EnabledIf attribute");
            }
        }

        #endregion
    }
}
