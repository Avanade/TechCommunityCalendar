using System.Collections.Generic;

namespace TechCommunityCalendar.Concretions
{
    public class TimeZone
    {
        public TimeZone(string code, string name, double relativeToGMT)
        {
            Code = code;
            Name = name;
            RelativeToGMT = relativeToGMT;
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public double RelativeToGMT { get; set; }

        public static IEnumerable<TimeZone> All()
        {
            return new TimeZone[]
            {
                new TimeZone("GMT","Greenwich Mean Time",0),
                new TimeZone("UTC","Universal Coordinated Time",0),
                new TimeZone("ECT","European Central Time",1),
                new TimeZone("EET","Eastern European Time",2),
                new TimeZone("ART","(Arabic) Egypt Standard Time",2),
                new TimeZone("EAT","Eastern African Time",3),
                new TimeZone("MET","Middle East Time",3),
                new TimeZone("NET","Near East Time",4),
                new TimeZone("PLT","Pakistan Lahore Time",5),
                new TimeZone("IST","India Standard Time",5.3),
                new TimeZone("BST","Bangladesh Standard Time",6),
                new TimeZone("VST","Vietnam Standard Time",7),
                new TimeZone("CTT","China Taiwan Time",8),
                new TimeZone("JST","Japan Standard Time",9),
                new TimeZone("ACT","Australia Central Time",9.5),
                new TimeZone("AET","Australia Eastern Time",10),
                new TimeZone("SST","Solomon Standard Time",11),
                new TimeZone("NST","New Zealand Standard Time",12),
                new TimeZone("MIT","Midway Islands Time",-11),
                new TimeZone("HST","Hawaii Standard Time",-10),
                new TimeZone("AST","Alaska Standard Time",-9),
                new TimeZone("PST","Pacific Standard Time",-8),
                new TimeZone("PNT","Phoenix Standard Time",-7),
                new TimeZone("MST","Mountain Standard Time",-7),
                new TimeZone("CST","Central Standard Time",-6),
                new TimeZone("EST","Eastern Standard Time",-5),
                new TimeZone("IET","Indiana Eastern Standard Time",-5),
                new TimeZone("PRT","Puerto Rico and US Virgin Islands Time",-4),
                new TimeZone("CNT","Canada Newfoundland Time",-3.5),
                new TimeZone("AGT","Argentina Standard Time",-3),
                new TimeZone("BET","Brazil Eastern Time",-3),
                new TimeZone("CAT","Central African Time",-1)
            };
        }
    }
}
