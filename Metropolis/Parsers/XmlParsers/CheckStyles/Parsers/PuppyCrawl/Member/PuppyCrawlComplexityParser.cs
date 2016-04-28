﻿using Metropolis.Extensions;
using Metropolis.Parsers.XmlParsers.CheckStyles.CheckStylesMemberParsers;
using Metropolis.Parsers.XmlParsers.CheckStyles.CheckStylesMemberParsers.PuppyCrawl;

namespace Metropolis.Parsers.XmlParsers.CheckStyles.Parsers.PuppyCrawl.Member
{
    public class PuppyCrawlComplexityParser : CheckStyleBaseParser, ICheckStylesMemberParser
    {
        public override string Source => PuppyCrawlSources.FanOutComplexity;

        public PuppyCrawlComplexityParser() : base(IntRegex)
        {
        }

        public void Parse(Domain.Member member, CheckStylesItem item)
        {
            member.Name = $"{item.Line}-{item.Column}";
            member.CylomaticComplexity = Parser.Match(item.Message).Value.AsInt();
        }
    }
}