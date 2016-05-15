﻿using FluentAssertions;
using Metropolis.Api.Domain;
using Metropolis.Api.Parsers;
using Metropolis.Api.Persistence;
using Metropolis.Api.Services;
using Metropolis.Common.Models;
using Moq;
using NUnit.Framework;

namespace Metropolis.Test.Api.Services
{
    [TestFixture]
    public class CodeBaseServiceTest : StrictMockBaseTest
    {
        private CodebaseService codebaseService;
        private Mock<IMetricsParserFactory> parserFactory;
        private Mock<IProjectRepository> repository;
        private Mock<IClassParser> classParser;

        private const string FileName = @"C:\\myfolder\mycodebase.project";
        private readonly CodeBase workspace = CodeBase.Empty();

        [SetUp]
        public void SetUp()
        {
            parserFactory = CreateMock<IMetricsParserFactory>();
            repository = CreateMock<IProjectRepository>();
            classParser = CreateMock<IClassParser>();

            codebaseService = new CodebaseService(parserFactory.Object, repository.Object);
        }

        [Test]
        public void Save()
        {
            repository.Setup(x => x.Save(workspace, FileName));
            codebaseService.Save(workspace, FileName);
        }

        [Test]
        public void Load()
        {
            repository.Setup(x => x.Load(FileName)).Returns(workspace);
            codebaseService.Load(FileName).Should().BeSameAs(workspace);
        }

        [Test]
        public void LoadDefault()
        {
            repository.Setup(x => x.LoadDefault()).Returns(workspace);
            codebaseService.LoadDefault().Should().BeSameAs(workspace);
        }

        [Test]
        public void GetToxicity()
        {
            parserFactory.Setup(x => x.ParserFor(ParseType.RichardToxicity)).Returns(classParser.Object);
            classParser.Setup(x => x.Parse(FileName)).Returns(workspace);
            codebaseService.GetToxicity(FileName).Should().BeSameAs(workspace);
        }

        [Test]
        public void GetVisualStudioMetrics()
        {
            parserFactory.Setup(x => x.ParserFor(ParseType.VisualStudio)).Returns(classParser.Object);
            classParser.Setup(x => x.Parse(FileName)).Returns(workspace);
            codebaseService.GetVisualStudioMetrics(FileName).Should().BeSameAs(workspace);
        }

        [Test]
        public void Get()
        {
            parserFactory.Setup(x => x.ParserFor(ParseType.EsLint)).Returns(classParser.Object);
            classParser.Setup(x => x.Parse(FileName)).Returns(workspace);
            codebaseService.Get(FileName, ParseType.EsLint).Should().BeSameAs(workspace);
        }
    }
}