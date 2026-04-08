using etrian_odyssey_ap_patcher.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Event
{
    public class EventScriptParser
    {

        private static ushort ReadParam(byte[] scripts, ref int position)
        {
            //ushort result = ByteUtil.ReadUint16(scripts, position);
            //position += 2;
            //return result;
            byte[] data = new byte[2];

            for (int i = 0; i < data.Length; i++)
                data[i] = scripts[position + i];

            ushort result = (ushort)(data[0] << 0x08 | data[1]);
            position += 2;
            return result;
        }

        private static byte ReadByte(byte[] scripts, ref int position)
        {
            byte result = scripts[position];
            position++;
            return result;
        }

        private static EventCommandId ReadCommand(byte[] scripts, ref int position)
        {
            byte[] data = new byte[2];

            for (int i = 0; i < data.Length; i++)
                data[i] = scripts[position + i];

            ushort result = (ushort)(data[0] << 0x08 | data[1]);
            position += 2;

            if (result >= (ushort)EventCommandId.E_COMID_MAX)
                throw new Exception();

            return (EventCommandId)result;
        }


        private static object[] ParseParameters(byte[] scripts, EventCommandId commandId, ref int position)
        {
            ParsingDefinition parsingDefinition = event_parsing_table[commandId];

            List<object> parameters = new List<object>();

            switch (parsingDefinition.Style)
            {
                case ParsingStyle.STANDARD:

                    for (int i = 0; i < parsingDefinition.ParameterCount; i++)
                        parameters.Add(ReadParam(scripts, ref position));

                    return parameters.ToArray();
                case ParsingStyle.STRING:

                    byte curr_char;
                    List<byte> label = new List<byte>();
                    do
                    {
                        curr_char = ReadByte(scripts, ref position);
                        if (curr_char != 0x00)
                            label.Add(curr_char);
                    } while (curr_char != 0x00);

                    parameters.Add(Encoding.ASCII.GetString(label.ToArray()));

                    return parameters.ToArray();
                case ParsingStyle.IF:
                    ushort value1 = ReadParam(scripts, ref position);
                    ushort value2 = ReadParam(scripts, ref position);

                    //int param_number;
                    //{
                    //    uint r5 = value1;
                    //    uint r0 = value2;
                    //
                    //    r0 = r5 + r0;
                    //    r0 = r0 + 1;
                    //    r0 = r0 << 0x10;
                    //    param_number = (int)(r0 >> 0x10);
                    //}

                    // TODO fix.
                    int param_number = ((int)value1 + (int)value2 + 1) * 0x10000 >> 0x10;

                    parameters.Add(param_number);

                    for (int i = 0; i < param_number; i++)
                    {
                        var ifparam = new EventScriptCommandIFParameter();
                        ifparam.parameter1 = ReadParam(scripts, ref position);
                        ifparam.parameter2 = ReadParam(scripts, ref position);
                        ifparam.parameter3 = ReadParam(scripts, ref position);
                        ifparam.parameter4 = ReadParam(scripts, ref position);
                        parameters.Add(ifparam);
                    }

                    return parameters.ToArray();
                default:
                    throw new NotImplementedException();
            }
        }



        public static EventScript ParseEventScript(byte[] scripts, uint script_offset)
        {
            Initialize();

            int position = (int)script_offset;

            EventScript eventScript = new EventScript();
            EventCommandId commandId;
            do
            {
                commandId = ReadCommand(scripts, ref position);

                object[] parameters = ParseParameters(scripts, commandId, ref position);

                eventScript.Commands.Add(new EventScriptCommand(commandId, parameters));

            } while (commandId != EventCommandId.E_COMID_EV_START_NUM_TO);

            return eventScript;
        }

        enum ParsingStyle
        {
            STANDARD,
            STRING,
            IF,

        }

        class ParsingDefinition
        {
            public ParsingDefinition(ParsingStyle style, int parameterCount = 0)
            {
                Style = style;
                ParameterCount = parameterCount;
            }

            public ParsingStyle Style;
            public int ParameterCount;
        }

        private static void Initialize()
        {
            if (event_parsing_table.Count > 0)
                return;

            event_parsing_table.Add((EventCommandId)0, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)1, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)2, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)3, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)4, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)5, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)6, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)7, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)8, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)9, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)10, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)11, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)12, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)13, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)14, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)15, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)16, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)17, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)18, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)19, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)20, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)21, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)22, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)23, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)24, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)25, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)26, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)27, new ParsingDefinition(ParsingStyle.IF));
            event_parsing_table.Add((EventCommandId)28, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)29, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)30, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)31, new ParsingDefinition(ParsingStyle.STANDARD, 3));
            //event_parsing_table.Add((EventCommandId)32,??
            event_parsing_table.Add((EventCommandId)33, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)34, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)35, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)36, new ParsingDefinition(ParsingStyle.STANDARD, 3));
            event_parsing_table.Add((EventCommandId)37, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)38, new ParsingDefinition(ParsingStyle.STANDARD, 2));
            event_parsing_table.Add((EventCommandId)39, new ParsingDefinition(ParsingStyle.STANDARD, 2));
            event_parsing_table.Add((EventCommandId)40, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)41, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)42, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)43, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)44, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)45, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)46, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)47, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)48, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)49, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)50, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)51, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)52, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)53, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)54, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)55, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)56, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)57, new ParsingDefinition(ParsingStyle.STANDARD, 00));
            event_parsing_table.Add((EventCommandId)58, new ParsingDefinition(ParsingStyle.IF));
            event_parsing_table.Add((EventCommandId)59, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)60, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)61, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)62, new ParsingDefinition(ParsingStyle.IF));
            event_parsing_table.Add((EventCommandId)63, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)64, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)65, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)66, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)67, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)68, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)69, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)70, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)71, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)72, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)73, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)74, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)75, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)76, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)77, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)78, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)79, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)80, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)81, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)82, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)83, new ParsingDefinition(ParsingStyle.STRING));
            event_parsing_table.Add((EventCommandId)84, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)85, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)86, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)87, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)88, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)89, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)90, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)91, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)92, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)93, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)94, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)95, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)96, new ParsingDefinition(ParsingStyle.STANDARD, 2));
            event_parsing_table.Add((EventCommandId)97, new ParsingDefinition(ParsingStyle.STANDARD, 2));
            event_parsing_table.Add((EventCommandId)98, new ParsingDefinition(ParsingStyle.STANDARD, 2));
            event_parsing_table.Add((EventCommandId)99, new ParsingDefinition(ParsingStyle.STANDARD, 2));
            event_parsing_table.Add((EventCommandId)100, new ParsingDefinition(ParsingStyle.STANDARD, 2));
            event_parsing_table.Add((EventCommandId)101, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)102, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)103, new ParsingDefinition(ParsingStyle.STANDARD, 2));
            event_parsing_table.Add((EventCommandId)104, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)105, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)106, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)107, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)108, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)109, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)110, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)111, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)112, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)113, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)114, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)115, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)116, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)117, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)118, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)119, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)120, new ParsingDefinition(ParsingStyle.STANDARD, 3));
            event_parsing_table.Add((EventCommandId)121, new ParsingDefinition(ParsingStyle.STANDARD, 0));
            event_parsing_table.Add((EventCommandId)122, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)123, new ParsingDefinition(ParsingStyle.STANDARD, 4));
            event_parsing_table.Add((EventCommandId)124, new ParsingDefinition(ParsingStyle.STANDARD, 1));
            event_parsing_table.Add((EventCommandId)125, new ParsingDefinition(ParsingStyle.STANDARD, 1));
        }


        private static readonly Dictionary<EventCommandId, ParsingDefinition> event_parsing_table = new Dictionary<EventCommandId, ParsingDefinition>();



    }
}
