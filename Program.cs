using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSpectrum
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Hs [m] = ");
            double inputHs = double.Parse(Console.ReadLine());
            Console.Write("Tp [m] = ");
            double inputTp = double.Parse(Console.ReadLine());
            
            WaveSpectrum ws = new WaveSpectrum(Hs: inputHs, Tp: inputTp);
            Console.Write("\nHs = " + ws.GetHs() + "\n");
            Console.Write("Tp = " + ws.GetTp() + "\n");
            Console.Write("gamma = " + ws.GetGamma() + "\n\n");

            ws.calculatePM();
            for (int i = 0; i < 100; i++)
            {
                Console.Write(ws.GetOmega()[i] + "\t" + ws.GetSpectrumPM()[i] + "\n");
            }                
            Console.Read();
        }
    }
}
