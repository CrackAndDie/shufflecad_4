using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace shufflecad_4.Helpers
{
    public class FuncadHelper
    {
        public static float Transfunc(float val, List<float> inArr, List<float> outArr)
        {
            float output = 0;
            float min_i;
            float max_i;
            float min_o;
            float max_o;

            if (val < inArr[0])
            {
                output = outArr[0];
            }
            if (val > inArr[inArr.Count - 1])
            {
                output = outArr[inArr.Count - 1];
            }

            for (int i = 0; i < inArr.Count - 1; i++)
            {
                if ((val >= inArr[i]) && (val <= inArr[i + 1]))
                {
                    min_i = inArr[i];
                    max_i = inArr[i + 1];
                    min_o = outArr[i];
                    max_o = outArr[i + 1];
                    output = min_o + (((max_o - min_o) * ((val - min_i) * 100 / (max_i - min_i))) / 100);
                    break;
                }
            }
            return output;
        }

        public static UInt32 BytesToInt(byte[] arr)
        {
            UInt32 wd = ((UInt32)arr[3] << 24) | ((UInt32)arr[2] << 16) | ((UInt32)arr[1] << 8) | (UInt32)arr[0];
            return wd;
        }

        public static byte[] GetInputBytes(Socket clientSocket)
        {
            byte[] rcvLenBytes = new byte[4];
            clientSocket.Receive(rcvLenBytes);
            UInt32 rcvLen = BytesToInt(rcvLenBytes);

            byte[] rcvBytes;
            byte[] clientData;
            List<byte> rcvBytesList = new List<byte>();
            int totalBytes = 0;
            while (totalBytes < rcvLen)
            {
                if (rcvLen - totalBytes < 262144)
                {
                    clientData = new byte[rcvLen - totalBytes];
                }
                else
                {
                    clientData = new byte[262144];
                }
                int bytesReceived = clientSocket.Receive(clientData);
                rcvBytesList.AddRange(clientData.Take(bytesReceived).ToArray());
                totalBytes += bytesReceived;
            }
            rcvBytes = rcvBytesList.ToArray();

            return rcvBytes;
        }

        public static string ValidateBase64EncodedString(string inputText)
        {
            string stringToValidate = inputText;
            stringToValidate = stringToValidate.Replace('-', '+'); // 62nd char of encoding
            stringToValidate = stringToValidate.Replace('_', '/'); // 63rd char of encoding
            stringToValidate = stringToValidate.Replace("\\", ""); // 63rd char of encoding
            stringToValidate = stringToValidate.Replace("\0", ""); // 63rd char of encoding
            switch (stringToValidate.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: stringToValidate += "=="; break; // Two pad chars
                case 3: stringToValidate += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }

            return stringToValidate;
        }

        public static void SetOutputBytes(byte[] data, Socket clientSocket)
        {
            byte[] toSendBytes = data;
            byte[] toSendLenBytes = System.BitConverter.GetBytes(data.Length);
            clientSocket.Send(toSendLenBytes);
            clientSocket.Send(toSendBytes);
        }
    }
}
