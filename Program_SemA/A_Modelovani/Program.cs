using System;

class Program
{
    static void Main()
    {
        int replikace = 10000;     
        double b = 30.0;          // rozměr pouzdra v mm (šířka b podle zadání)
        double cenaZaChybu = 5.0; // USD za neúspěch
        int seed = 30;            

        SpustScenar("Základní (TRIA 7.6–8.4)", false, replikace, b, cenaZaChybu, seed);
        SpustScenar("Vylepšený S3 (TRIA 7.6–8.1)", true, replikace, b, cenaZaChybu, seed);
    }

    static void SpustScenar(string nazev, bool vylepseny, int replikace, double b, double cenaZaChybu, int seed)
    {
        Random rnd = new Random(seed);
        int pocetNevejde = 0;       // kolik komponentů se nevejdou (F < 2 mm),
        int pocetMensiNez4 = 0;     // zaznamenani pripadu, SoucetDelky > 26
        double celkoveNaklady = 0; // Součet nákladů za neúspěšné simulace

        for (int i = 0; i < replikace; i++)
        {
            double d1 = Normalni(rnd, 7.0, 0.4); // S1 – normální rozdělení
            double d2 = 9.1 + (9.9 - 9.1) * rnd.NextDouble(); // S2 – rovnoměrné rozdělení 
            double d3 = Normalni(rnd, 6.5, 0.45); // S3 – normální rozdělení 

            // Výběr hodnoty S4 podle toho, zda je scénář vylepšený nebo ne
            double l;
            if (vylepseny)
            {
                l = Trojuhelnik(rnd, 7.6, 8.0, 8.1); // Vylepšený scénář
            }
            else
            {
                l = Trojuhelnik(rnd, 7.6, 8.0, 8.4); // Základní scénář
            }

            double p1 = 2.0, p2 = 1.5;
            double soucetDelky = (d1 + d2 + d3 + l) - (p1 + p2);

            if (soucetDelky > b - 2)
            {
                pocetNevejde++;
                celkoveNaklady += cenaZaChybu;
            }

            if (soucetDelky > b - 4)
            {
                pocetMensiNez4++;
            }
        }

        // Výpis výsledků
        Console.WriteLine("==============================================================");
        Console.WriteLine($"SCÉNÁŘ: {nazev}");
        Console.WriteLine("--------------------------------------------------------------");
        Console.WriteLine($"Počet simulací:        {replikace:N0}");
        Console.WriteLine($"Rozměr pouzdra:        {b:F1} mm");
        Console.WriteLine($"Cena za chybu:         ${cenaZaChybu:F2}");
        Console.WriteLine();
        Console.WriteLine($"Nevejde (F < 2 mm):    {pocetNevejde,8}x   | {100.0 * pocetNevejde / replikace,6:F2} %");
        Console.WriteLine($"Méně než 4 mm (F < 4): {pocetMensiNez4,8}x | {100.0 * pocetMensiNez4 / replikace,6:F2} %");
        Console.WriteLine();
        Console.WriteLine($"Celkové náklady:     ${celkoveNaklady,8:F2}");
        Console.WriteLine("==============================================================\n");

    }

    // Funkce, která vrací náhodné číslo podle trojúhelníkového rozdělení
    static double Trojuhelnik(Random rnd, double a, double c, double b)
    {
        double u = rnd.NextDouble();
        double Fc = (c - a) / (b - a); // Určuje, kde je vrchol trojúhelníku (nejčastější hodnota)
        if (u < Fc)
        {
            return a + Math.Sqrt(u * (b - a) * (c - a));
        }
        else
        {
            return b - Math.Sqrt((1 - u) * (b - a) * (b - c));
        }
    }

    // Funkce, která vrací náhodné číslo podle normálního rozdělení
    static double Normalni(Random rnd, double stred, double smerodatna)
    {
        // Vygenerují se dvě náhodná čísla mezi 0 a 1
        double u1 = 1.0 - rnd.NextDouble();
        double u2 = 1.0 - rnd.NextDouble();

        // Převod na hodnotu podle normálního rozdělení
        double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

        return stred + smerodatna * z;
    }
}
