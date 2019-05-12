using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class OTPSimulation
    {
        public string MakeOTP()
        {
            string UpperCase = "QWERTYUIOPASDFGHJKLZXCVBNM";
            string Number = "0123456789";
            string allCharacters = UpperCase + Number;
            Random ran = new Random();
            string OTP ="";
            var length = new SystemConfigDAO().GetByID("LENGTHOTP");
            for(int i = 0; i < Int32.Parse(length.Value) ; i++)
            {
                double rand = ran.NextDouble();
                OTP += allCharacters.ToCharArray()[(int)Math.Floor(rand * allCharacters.Length)];
            }
            return OTP;
        }
    }
}
