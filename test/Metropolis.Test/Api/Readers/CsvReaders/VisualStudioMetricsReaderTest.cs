﻿using System.Linq;
using Metropolis.Api.Domain;
using Metropolis.Api.Readers.CsvReaders;
using Metropolis.Test.Extensions;
using Metropolis.Test.Utilities;
using NUnit.Framework;

namespace Metropolis.Test.Api.Readers.CsvReaders
{
    [TestFixture]
    public class VisualStudioMetricsReaderTest
    {
        [SetUp]
        public void SetUp()
        {
            reader = new VisualStudioMetricsReaderDouble();
        }

        private VisualStudioMetricsReaderDouble reader;
        [Test]
        public void Parse_NoMembers()
        {
            var typ = MakeType("Clock", loc: 1, dit: 2, cyclo: 3, cc: 4);

            var actual = reader.TestParse(new[] {typ});

            Validate.Begin().IsNotNull(actual, "actual")
                .IsEqual(actual.AllInstances.Count, 1, "class Count")
                .Check()
                .IsEqual(actual.LinesOfCode, 1, "loc")
                .IsEqual(actual.NumberOfTypes, 1, "# Types")
                .IsEqual(actual.AverageToxicity(), 0, "avg toxicity")
                .Check();
        }

        [Test]
        public void ParseSingleMember()
        {
            var typ = MakeType("Clock", loc: 1, dit: 2, cyclo: 3, cc: 4);
            var mbr = MakeMember("Today()");

            var actual = reader.TestParse(new[] {typ, mbr});

            Validate.Begin().IsNotNull(actual, "actual").IsEqual(actual.AllInstances.Count, 1, "class Count").Check()
                .IsEqual(actual.LinesOfCode, 1, "loc")
                .IsEqual(actual.NumberOfTypes, 1, "# Types")
                .IsEqual(actual.AverageToxicity(), 0, "avg toxicity")
                .Check();


            var cls = actual.AllInstances.First();
            Validate.Begin()
                .IsNotNull(cls, "cls")
                .IsEqual(cls.Members.Count, 1, "# members")
                .IsTrue(
                    cls.Members[0].ReflectionEquals(
                        new Member("Today()", mbr.LinesOfCode, mbr.ClassCoupling, mbr.ClassCoupling), true),
                    "member equals")
                .Check();
        }
        
        private static VisualStudioCsvLineItem MakeType(string type, string ns = "ns", int loc = 0, int cyclo = 0, int dit = 0, int cc = 0)
        {
            return new VisualStudioCsvLineItem
            {
                Scope = "Type",
                Namespace = ns,
                Type = type,
                LinesOfCode = loc,
                CyclomaticComplexity = cyclo,
                DepthOfInheritance = dit,
                ClassCoupling = cc
            };
        }

        private static VisualStudioCsvLineItem MakeMember(string member, string type = "Clock", string ns = "ns",
            int loc = 0, int cyclo = 0, int dit = 0, int cc = 0)
        {
            return new VisualStudioCsvLineItem
            {
                Scope = "Member",
                Member = member,
                Namespace = ns,
                Type = type,
                LinesOfCode = loc,
                CyclomaticComplexity = cyclo,
                DepthOfInheritance = dit,
                ClassCoupling = cc
            };
        }

    }

    public class VisualStudioMetricsReaderDouble : VisualStudioMetricsReader
    {
        public CodeBase TestParse(VisualStudioCsvLineItem[] lines)
        {
            return ParseLines(lines);
        }
    }
}