﻿using System;
using System.Collections.Generic;
using System.IO;
using Metropolis.Api.Core.Domain;
using Metropolis.Api.Core.Parsers.XmlParsers;
using NUnit.Framework;
using FluentAssertions;
using System.Linq;

namespace Test.Metropolis.Parsers.CsvParsers
{
    public abstract class CsvParsersBaseTest<T> where T : IClassParser, new()
    {
        protected T Parser;
        protected string FileName;

        [SetUp]
        public void SetUp()
        {
            Parser = new T();
            FileName = $"{typeof(T).Name}.{DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss")}.csv";
            CleanFile(FileName);
        }

        [TearDown]
        public void Teardown()
        {
            CleanFile(FileName);
        }

        protected CodeBase ParseUsingData(string[] data)
        {
            CreateFileFrom(FileName, data);
            return Parser.Parse(FileName, "");
        }

        protected static void CreateFileFrom(string file, IEnumerable<string> data)
        {
            File.WriteAllLines(file, data);
        }

        protected static void CleanFile(string file)
        {
            if (File.Exists(file))
                File.Delete(file);
        }

        //Custom Assertions
        protected static Class AssertHasOneClassEqualTo(Class expected, CodeBase codeBase)
        {
            expected.Should().NotBeNull();
            codeBase.Should().NotBeNull();
            codeBase.AllClasses.Count.Should().Be(1);

            var actual = codeBase.AllClasses.First();

            actual.NameSpace.Should().Be(expected.NameSpace);
            actual.Name.Should().Be(expected.Name);
            actual.NumberOfMethods.Should().Be(expected.NumberOfMethods);
            actual.CyclomaticComplexity.Should().Be(expected.CyclomaticComplexity);
            actual.ClassCoupling.Should().Be(expected.ClassCoupling);
            actual.DepthOfInheritance.Should().Be(expected.DepthOfInheritance);
            actual.LinesOfCode.Should().Be(expected.LinesOfCode);
            return actual;
        }

        protected void AssertHasOneMemberEqualTo(Class actual, Member expected)
        {
            actual.Should().NotBeNull();
            expected.Should().NotBeNull();
            actual.Members.Count.Should().Be(1);

            var actualMember = actual.Members.First();
            actualMember.Name.Should().Be(expected.Name);
            actualMember.LinesOfCode.Should().Be(expected.LinesOfCode);
            actualMember.CylomaticComplexity.Should().Be(expected.CylomaticComplexity);
            actualMember.ClassCoupling.Should().Be(expected.ClassCoupling);
        }
    }
}