﻿namespace Metropolis.TipOfTheDay
{
    public class DefaultCaseControlTip : ITipOfTheDay
    {
        public string Tip =>
@"
    <StackPanel>
        <TextBlock>
            <Span FontSize='14' FontWeight='Bold'>Having a default in a switch statement improves your code metrics</Span>
        </TextBlock>
        <Image HorizontalAlignment='left' Height='250' Width='400' Source = '..\Images\TipOfTheDay\MissingDefaultInSwitch.png' />
    </StackPanel>
";

        public string ForMoreInfoUrl => @"https://en.wikipedia.org/wiki/Switch_statement";
    }
}