﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.ScriptProviders;
using DbUp.Tests.TestInfrastructure;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace DbUp.Tests.ScriptProvider
{
    public class EmbeddedScriptAndCodeProviderTests : SpecificationFor<EmbeddedScriptAndCodeProvider>
    {
        private SqlScript[] scriptsToExecute;

        public override EmbeddedScriptAndCodeProvider Given()
        {
            var assembly = typeof(EmbeddedScriptAndCodeProviderTests).GetTypeInfo().Assembly;

            return new EmbeddedScriptAndCodeProvider(assembly, s=>true);
        }
    
        protected override void When()
        {
            var testConnectionManager = new TestConnectionManager(Substitute.For<IDbConnection>());
            testConnectionManager.OperationStarting(new ConsoleUpgradeLog(), new List<SqlScript>());
            scriptsToExecute = Subject.GetScripts(testConnectionManager).ToArray();
        }

        [Then]
        public void it_should_return_all_sql_files()
        {
            scriptsToExecute.Length.ShouldBe(10);
        }

        [Then]
        public void should_provide_content_for_code_script()
        {
            scriptsToExecute
                .Single(s => s.Name.EndsWith("Script20120723_1_Test4.cs"))
                .Contents
                .ShouldBe("test4");
        }
    }
}
