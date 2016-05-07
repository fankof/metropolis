using Metropolis.Api.Extensions;

namespace Metropolis.Api.Core.Parsers.XmlParsers.CheckStyles.Parsers.PuppyCrawl.Class
{
    public class PuppyCrawlClassDataAbstractionCouplingParser : CheckStyleBaseParser, ICheckStylesClassParser
    {
        public override string Source => PuppyCrawlSources.ClassDataAbstractionCoupling;
        
        public void Parse(Domain.Class type, CheckStylesItem item)
        {
            type.ClassDataAbstractionCoupling = IntParser.Match(item.Message).Value.AsInt();
        }
    }
}