namespace Redbayesiana
{
    class Program
    {
        static void Main(string[] args)
        {
            RedBayesiana red = new RedBayesiana();

            bool presupuestoAlto = true;
            bool altaEficiencia = true;

            Console.WriteLine("Recomendación basada en el presupuesto alto y alta eficiencia de combustible.");
            red.RecomendarAuto(presupuestoAlto, altaEficiencia);

        }
    }

    public class RedBayesiana
    {
        // Probabilidades a priori de cada tipo de auto
        private Dictionary<string, double>
            probabilidadAuto = new Dictionary<string, double>
            {
            {"Auto Deportivo", 0.3 },
            {"Sedán", 0.7 }
            };

        // Probabilidades condicionales: dado un tipo de auto, la probabilidad de tener ciertas características
        private Dictionary<string, Dictionary<string, double>>
            probabilidadCaracteristicaDadoAuto = new Dictionary<string, Dictionary<string, double>>
            {
            {"PresupuestoAlto", new Dictionary<string, double> { {"Auto Deportivo", 0.9}, {"Sedán", 0.4} } },
            {"AltaEficiencia", new Dictionary<string, double> { {"Auto Deportivo", 0.3}, {"Sedán", 0.8} } }
            };

        // Método para inferir la probabilidad de un tipo de auto dado el presupuesto y la eficiencia
        public double InferirProbabilidadAuto(string tipoAuto, bool presupuestoAlto, bool altaEficiencia)
        {
            // Probabilidad a priori del tipo de auto
            double probAuto = probabilidadAuto[tipoAuto];

            // Probabilidad de presupuesto alto dado el tipo de auto
            double probPresupuestoAlto = probabilidadCaracteristicaDadoAuto["PresupuestoAlto"][tipoAuto];

            // Probabilidad de alta eficiencia dado el tipo de auto
            double probAltaEficiencia = probabilidadCaracteristicaDadoAuto["AltaEficiencia"][tipoAuto];

            // Multiplicamos las probabilidades condicionales
            double unirProb = probAuto;

            if (presupuestoAlto)
                unirProb *= probPresupuestoAlto;
            else
                unirProb *= (1 - probPresupuestoAlto);

            if (altaEficiencia)
                unirProb *= probAltaEficiencia;
            else
                unirProb *= (1 - probAltaEficiencia);

            return unirProb;
        }

        // Método para recomendar un tipo de auto basado en el presupuesto y eficiencia
        public void RecomendarAuto(bool presupuestoAlto, bool altaEficiencia)
        {
            double deportivoProb = InferirProbabilidadAuto("Auto Deportivo", presupuestoAlto, altaEficiencia);
            double sedanProb = InferirProbabilidadAuto("Sedán", presupuestoAlto, altaEficiencia);

            // Normalizamos las probabilidades
            double totalProb = deportivoProb + sedanProb;

            deportivoProb /= totalProb;
            sedanProb /= totalProb;

            // Mostramos el resultado de la recomendación
            Console.WriteLine($"Probabilidad de Auto Deportivo: {deportivoProb * 100:F2}%");
            Console.WriteLine($"Probabilidad de Sedán: {sedanProb * 100:F2}%");
        }
    }
}
