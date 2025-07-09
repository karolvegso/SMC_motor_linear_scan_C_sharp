using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace SMC_motor_ConsoleApp_01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Query for COM port number of SMC motor
            Console.WriteLine("Specify number of COM port of SMC motor:");
            // save COM port number as  string
            string COM_port_no_SMC_motor = Console.ReadLine();
            // full name of COM port
            string COM_port_name_SMC_motor = "COM" + COM_port_no_SMC_motor;
            // print full name of COM port
            Console.WriteLine(COM_port_name_SMC_motor);

            // Query X position to start horizontal/lateral scanning
            Console.WriteLine("Specify X starting position:");
            string X_start = Console.ReadLine();
            float X_start_float = float.Parse(X_start);

            // Query X position to stop horizontal/lateral scanning
            Console.WriteLine("Specify X stop position:");
            string X_stop = Console.ReadLine();
            float X_stop_float = float.Parse(X_stop);

            // Query X position to move with step
            Console.WriteLine("Specify X step:");
            string X_step = Console.ReadLine();
            float X_step_float = float.Parse(X_step);

            // if condition to check X_start and X_step
            if (X_start_float <= X_stop_float)
            {
                Console.WriteLine("The X values are OK. The X starting value is is less or equal than X stop value.");
            }
            else
            {
                Console.WriteLine("The X values are not OK!!! The X starting value is is more than X stop value!!!");
                // wait 300 seconds or 5 minutes
                Thread.Sleep(300000);
                Console.WriteLine("We are now waiting for 5 minutes.");
                Console.WriteLine("I recommend to close program by force!!!");
            }

            // calculate number of horiozntal/lateral points
            int no_X_points = (int)Math.Round(Math.Abs(X_stop_float - X_start_float) / X_step_float) + 1;
            // print number of X points in single lateral scan
            Console.WriteLine($"Number of X points in single lateral scan is: {no_X_points}");

            float[] X_positions = new float[no_X_points];
            for (int index_0=0; index_0 < no_X_points; index_0++)
            {
                X_positions[index_0] = X_start_float + index_0 * X_step_float;
            }

            // create new communication with COM port of SMC motor
            SerialPort serialPort_SMC_motor = new SerialPort(COM_port_name_SMC_motor, 57600, Parity.None, 8, StopBits.One);
            // open communication with COM port of SMC motor
            serialPort_SMC_motor.Open();
            // set CRLF as the terminator or end-of-line sequence
            serialPort_SMC_motor.NewLine = "\r\n";
            // get SMC motor stage identifier
            serialPort_SMC_motor.WriteLine("1ID?");
            // read response
            string response = serialPort_SMC_motor.ReadLine();
            // print SMC motor stage identifier
            Console.WriteLine("The SMC motor stage identifier is: " + response);
            // qet home search velocity of SMC motor
            serialPort_SMC_motor.WriteLine("1OH?");
            // read response
            response = serialPort_SMC_motor.ReadLine();
            // print home search velocity of SMC motor
            Console.WriteLine("The home search velocity of SMC motor is: " + response);
            // qet velocity of SMC motor
            serialPort_SMC_motor.WriteLine("1VA?");
            // read response
            response = serialPort_SMC_motor.ReadLine();
            // print home search velocity of SMC motor
            Console.WriteLine("The velocity of SMC motor is: " + response);
            // Get positioner error and controller state
            serialPort_SMC_motor.WriteLine("1TS");
            response = serialPort_SMC_motor.ReadLine();
            // wait 1000 miliseconds
            Thread.Sleep(1000);
            int str_length = response.Length;
            // read controller state, ef
            string controller_state = response.Substring(str_length - 2);
            Console.WriteLine(response);
            if (str_length == 9 && controller_state == "0A")
            {
                Console.WriteLine("NOT REFERENCED from reset.");
                // execute home search
                serialPort_SMC_motor.WriteLine("1OR");
                // print start homing
                Console.WriteLine("The homing of SMC motor just started.");
                while (true)
                {
                    // Get positioner error and controller state
                    serialPort_SMC_motor.WriteLine("1TS");
                    response = serialPort_SMC_motor.ReadLine();
                    // wait 1000 miliseconds
                    Thread.Sleep(1000);
                    str_length = response.Length;
                    // read controller state, ef
                    controller_state = response.Substring(str_length - 2);
                    Console.WriteLine(response);
                    if (str_length == 9 && controller_state == "1E")
                    {
                        Console.WriteLine("The SMC motor is Homing commanded from RS-232-C.");
                        continue;
                    }
                    else if (str_length == 9 && controller_state == "32")
                    {
                        Console.WriteLine("The SMC motor is Ready from Homing.");
                        break;
                    }
                    else if (str_length == 9 && controller_state != "1E" && controller_state != "32")
                    {
                        Console.WriteLine("The SMC motor is responding something else.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The SMC motor is not responding correctly.");
                        break;
                    }
                }
            }
            else if (str_length == 9 && controller_state == "0B")
            {
                Console.WriteLine(" NOT REFERENCED from HOMING.");
                // execute home search
                serialPort_SMC_motor.WriteLine("1OR");
                // print start homing
                Console.WriteLine("The homing of SMC motor just started.");
                while (true)
                {
                    // Get positioner error and controller state
                    serialPort_SMC_motor.WriteLine("1TS");
                    response = serialPort_SMC_motor.ReadLine();
                    // wait 1000 miliseconds
                    Thread.Sleep(1000);
                    str_length = response.Length;
                    // read controller state, ef
                    controller_state = response.Substring(str_length - 2);
                    Console.WriteLine(response);
                    if (str_length == 9 && controller_state == "1E")
                    {
                        Console.WriteLine("The SMC motor is Homing commanded from RS-232-C.");
                        continue;
                    }
                    else if (str_length == 9 && controller_state == "32")
                    {
                        Console.WriteLine("The SMC motor is Ready from Homing.");
                        break;
                    }
                    else if (str_length == 9 && controller_state != "1E" && controller_state != "32")
                    {
                        Console.WriteLine("The SMC motor is responding something else.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The SMC motor is not responding correctly.");
                        break;
                    }
                }
            }
            else if (str_length == 9 && controller_state == "0C")
            {
                Console.WriteLine("NOT REFERENCED from CONFIGURATION. ");
                // execute home search
                serialPort_SMC_motor.WriteLine("1OR");
                // print start homing
                Console.WriteLine("The homing of SMC motor just started.");
                while (true)
                {
                    // Get positioner error and controller state
                    serialPort_SMC_motor.WriteLine("1TS");
                    response = serialPort_SMC_motor.ReadLine();
                    // wait 1000 miliseconds
                    Thread.Sleep(1000);
                    str_length = response.Length;
                    // read controller state, ef
                    controller_state = response.Substring(str_length - 2);
                    Console.WriteLine(response);
                    if (str_length == 9 && controller_state == "1E")
                    {
                        Console.WriteLine("The SMC motor is Homing commanded from RS-232-C.");
                        continue;
                    }
                    else if (str_length == 9 && controller_state == "32")
                    {
                        Console.WriteLine("The SMC motor is Ready from Homing.");
                        break;
                    }
                    else if (str_length == 9 && controller_state != "1E" && controller_state != "32")
                    {
                        Console.WriteLine("The SMC motor is responding something else.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The SMC motor is not responding correctly.");
                        break;
                    }
                }
            }
            else if (str_length == 9 && controller_state == "0D")
            {
                Console.WriteLine("NOT REFERENCED from DISABLE.");
                // execute home search
                serialPort_SMC_motor.WriteLine("1OR");
                // print start homing
                Console.WriteLine("The homing of SMC motor just started.");
                while (true)
                {
                    // Get positioner error and controller state
                    serialPort_SMC_motor.WriteLine("1TS");
                    response = serialPort_SMC_motor.ReadLine();
                    // wait 1000 miliseconds
                    Thread.Sleep(1000);
                    str_length = response.Length;
                    // read controller state, ef
                    controller_state = response.Substring(str_length - 2);
                    Console.WriteLine(response);
                    if (str_length == 9 && controller_state == "1E")
                    {
                        Console.WriteLine("The SMC motor is Homing commanded from RS-232-C.");
                        continue;
                    }
                    else if (str_length == 9 && controller_state == "32")
                    {
                        Console.WriteLine("The SMC motor is Ready from Homing.");
                        break;
                    }
                    else if (str_length == 9 && controller_state != "1E" && controller_state != "32")
                    {
                        Console.WriteLine("The SMC motor is responding something else.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The SMC motor is not responding correctly.");
                        break;
                    }
                }
            }
            else if (str_length == 9 && controller_state == "0E")
            {
                Console.WriteLine("NOT REFERENCED from READY.");
                // execute home search
                serialPort_SMC_motor.WriteLine("1OR");
                // print start homing
                Console.WriteLine("The homing of SMC motor just started.");
                while (true)
                {
                    // Get positioner error and controller state
                    serialPort_SMC_motor.WriteLine("1TS");
                    response = serialPort_SMC_motor.ReadLine();
                    // wait 1000 miliseconds
                    Thread.Sleep(1000);
                    str_length = response.Length;
                    // read controller state, ef
                    controller_state = response.Substring(str_length - 2);
                    Console.WriteLine(response);
                    if (str_length == 9 && controller_state == "1E")
                    {
                        Console.WriteLine("The SMC motor is Homing commanded from RS-232-C.");
                        continue;
                    }
                    else if (str_length == 9 && controller_state == "32")
                    {
                        Console.WriteLine("The SMC motor is Ready from Homing.");
                        break;
                    }
                    else if (str_length == 9 && controller_state != "1E" && controller_state != "32")
                    {
                        Console.WriteLine("The SMC motor is responding something else.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The SMC motor is not responding correctly.");
                        break;
                    }
                }
            }
            else if (str_length == 9 && controller_state == "0F")
            {
                Console.WriteLine("NOT REFERENCED from MOVING.");
                // execute home search
                serialPort_SMC_motor.WriteLine("1OR");
                // print start homing
                Console.WriteLine("The homing of SMC motor just started.");
                while (true)
                {
                    // Get positioner error and controller state
                    serialPort_SMC_motor.WriteLine("1TS");
                    response = serialPort_SMC_motor.ReadLine();
                    // wait 1000 miliseconds
                    Thread.Sleep(1000);
                    str_length = response.Length;
                    // read controller state, ef
                    controller_state = response.Substring(str_length - 2);
                    Console.WriteLine(response);
                    if (str_length == 9 && controller_state == "1E")
                    {
                        Console.WriteLine("The SMC motor is Homing commanded from RS-232-C.");
                        continue;
                    }
                    else if (str_length == 9 && controller_state == "32")
                    {
                        Console.WriteLine("The SMC motor is Ready from Homing.");
                        break;
                    }
                    else if (str_length == 9 && controller_state != "1E" && controller_state != "32")
                    {
                        Console.WriteLine("The SMC motor is responding something else.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The SMC motor is not responding correctly.");
                        break;
                    }
                }
            }
            else if (str_length == 9 && controller_state == "10")
            {
                Console.WriteLine("NOT REFERENCED ESP stage error.");
                // execute home search
                serialPort_SMC_motor.WriteLine("1OR");
                // print start homing
                Console.WriteLine("The homing of SMC motor just started.");
                while (true)
                {
                    // Get positioner error and controller state
                    serialPort_SMC_motor.WriteLine("1TS");
                    response = serialPort_SMC_motor.ReadLine();
                    // wait 1000 miliseconds
                    Thread.Sleep(1000);
                    str_length = response.Length;
                    // read controller state, ef
                    controller_state = response.Substring(str_length - 2);
                    Console.WriteLine(response);
                    if (str_length == 9 && controller_state == "1E")
                    {
                        Console.WriteLine("The SMC motor is Homing commanded from RS-232-C.");
                        continue;
                    }
                    else if (str_length == 9 && controller_state == "32")
                    {
                        Console.WriteLine("The SMC motor is Ready from Homing.");
                        break;
                    }
                    else if (str_length == 9 && controller_state != "1E" && controller_state != "32")
                    {
                        Console.WriteLine("The SMC motor is responding something else.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The SMC motor is not responding correctly.");
                        break;
                    }
                }
            }
            else if (str_length == 9 && controller_state == "11")
            {
                Console.WriteLine("NOT REFERENCED from JOGGING.");
                // execute home search
                serialPort_SMC_motor.WriteLine("1OR");
                // print start homing
                Console.WriteLine("The homing of SMC motor just started.");
                while (true)
                {
                    // Get positioner error and controller state
                    serialPort_SMC_motor.WriteLine("1TS");
                    response = serialPort_SMC_motor.ReadLine();
                    // wait 1000 miliseconds
                    Thread.Sleep(1000);
                    str_length = response.Length;
                    // read controller state, ef
                    controller_state = response.Substring(str_length - 2);
                    Console.WriteLine(response);
                    if (str_length == 9 && controller_state == "1E")
                    {
                        Console.WriteLine("The SMC motor is Homing commanded from RS-232-C.");
                        continue;
                    }
                    else if (str_length == 9 && controller_state == "32")
                    {
                        Console.WriteLine("The SMC motor is Ready from Homing.");
                        break;
                    }
                    else if (str_length == 9 && controller_state != "1E" && controller_state != "32")
                    {
                        Console.WriteLine("The SMC motor is responding something else.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The SMC motor is not responding correctly.");
                        break;
                    }
                }
            }

            for (int index_0=0; index_0 < no_X_points; index_0++)
            {
                string str_cmd_moveabs_SMC_motor = "1PA" + X_positions[index_0].ToString("F3");
                // print command to abolsute move
                Console.WriteLine(str_cmd_moveabs_SMC_motor);
                // move absolutely SMC motor
                serialPort_SMC_motor.WriteLine(str_cmd_moveabs_SMC_motor);
                while (true)
                {
                    // Get positioner error and controller state
                    serialPort_SMC_motor.WriteLine("1TS");
                    response = serialPort_SMC_motor.ReadLine();
                    // wait 1000 miliseconds
                    Thread.Sleep(1000);
                    str_length = response.Length;
                    // read controller state, ef
                    controller_state = response.Substring(str_length - 2);
                    Console.WriteLine(response);
                    if (str_length == 9 && controller_state == "28")
                    {
                        Console.WriteLine("The SMC motor is Moving.");
                        continue;
                    }
                    else if (str_length == 9 && controller_state == "33")
                    {
                        Console.WriteLine("The SMC motor is Ready from Moving.");
                        break;
                    }
                    else if (str_length == 9 && controller_state != "28" && controller_state != "33")
                    {
                        Console.WriteLine("The SMC motor is responding something else.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The SMC motor is not responding correctly.");
                        break;
                    }
                }
            }

            // close communication with COM port of SMC motor
            serialPort_SMC_motor.Close();
            Thread.Sleep(60000);
        }
    }
}
