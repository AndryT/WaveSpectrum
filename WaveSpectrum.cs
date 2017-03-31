using System;

public class WaveSpectrum
{
    /* 1) Pierson-Moskowitz (PM) spectrum: (DNV-RP-C205)
         * 
         *      S_PM(w) = 5/16*Hs^2*w_p^-5*exp(-5/4*(w/w_p)^-4)
         * 
         * 2) JONSWAP Spectrum: (DNV-RP-C205)
         *
         *      S_J(w) = A_gamma*S_PM(w)*gamma^exp(-0.5*((w-w_p)/(sigma*w_p))^2)
         *      
         *   where:
         *      A_gamma = 1-0.287*ln(gamma)
         *      sigma = sigma_a for w <= w_p - usually sigma_a = 0.07
         *            = sigma_b for x > w_p  - usually sigma_b = 0.09
         *      gamma = 5                           for Tp/Hs^0.5 <= 3.6
         *            = exp(5.75-1.15*Tp/Hs^0.5)    for 3.6 < Tp/Hs^0.5 < 5
         *            = 1                           for Tp/Hs^0.5 >= 5
     */
    private const double sigma_a = 0.07;
    private const double sigma_b = 0.09;

    private double gamma;
    private double hs;
    private double tp, omega_p;
    private double[] omega;

    private double[] S_PM;
    private double[] S_J;

    /* CONSTRUCTOR */
	public WaveSpectrum(double Hs, double Tp, double gamma)
	{        
        this.hs = Hs;
        this.tp = Tp;
        this.gamma = gamma;
        this.omega_p = 2 * System.Math.PI / Tp;
	}
    public WaveSpectrum(double Hs, double Tp) :
        this(Hs, Tp, 0) 
    {
        this.gamma = calculate_gamma(hs, tp);
    }
    /* END */

    /* METHODS */
    public double GetHs()
    {
        return this.hs;
    }
    public double GetTp()
    {
        return this.tp;
    }
    public double GetGamma()
    {
        return this.gamma;
    }

    private double calculate_gamma(double Hs, double Tp)
    {
        double gamma_spectrum;
        double tp_hs_ratio = Tp / (System.Math.Sqrt(Hs));

        if (tp_hs_ratio <= 3.6)
            gamma_spectrum = 5;
        else if (tp_hs_ratio > 3.6 && tp_hs_ratio < 5)
            gamma_spectrum = System.Math.Exp(5.75 - 1.15 * tp_hs_ratio);
        else
            gamma_spectrum = 1;

        return gamma_spectrum;
    }

    public void calculatePM()
    {
        double part1 = 5.0/16.0*hs*hs/ System.Math.Pow(omega_p,5);        
        double exponential_part = 0;
        double part2 = 0;

        S_PM = new double[100];
        omega = initialiseOmega();
        
        for (int i_w = 0; i_w < omega.Length; i_w++) 
        {
            part2 = System.Math.Pow(omega[i_w], -5);
            exponential_part = System.Math.Exp(-5.0/4.0*System.Math.Pow(omega[i_w]/omega_p,-4));
            S_PM[i_w] = part1 * part2 * exponential_part;
        }        
    }

    public double[] GetSpectrumPM()
    {
        return this.S_PM;
    }

    public double[] GetOmega()
    {
        return this.omega;
    }

    private double[] initialiseOmega()
    {
        double delta_t = 5.0;
        delta_t = ((30.0 - 3.0) / 100.0);
        double[] omega_frequency = new double[100];
        for (int i = 0; i < 100; i++)
        {
            omega_frequency[i] = 2 * System.Math.PI / (3 + delta_t * i);
        }
        return omega_frequency;
    }
    
}
