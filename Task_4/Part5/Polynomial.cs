using System.Text;

namespace Task_4.Part5
{
    public class Polynomial
    {
        private readonly double[] coefficients;

        public Polynomial(params double[] coefficients)
        {
            this.coefficients = coefficients;
        }

        public override string ToString()
        {
            StringBuilder terms = new StringBuilder();
            for (int i = 0; i < coefficients.Length; i++)
            {
                double coeff = coefficients[i];
                if (coeff == 0) continue;
                
                string sign = coeff < 0 ? "-" : "+";
                coeff = Math.Abs(coeff);

                terms.Append($" {sign} {coeff}");
                if (i > 0) terms.Append($"x^{i}");
            }

            return terms.ToString().TrimStart('+', ' ');
        }

        public static Polynomial operator +(Polynomial left, Polynomial right)
        {
            int minLength = Math.Min(left.coefficients.Length, right.coefficients.Length);
            Polynomial longerPolynomial = left.coefficients.Length > right.coefficients.Length ? left : right;
            double[] result = new double[longerPolynomial.coefficients.Length];

            for (int i = 0; i < minLength; i++)
                result[i] = left.coefficients[i] + right.coefficients[i];
                        
            for (int i = minLength; i < longerPolynomial.coefficients.Length; i++)
                result[i] = longerPolynomial.coefficients[i];
            
            return new Polynomial(result);
        }
            
        public static Polynomial operator -(Polynomial left, Polynomial right)
        {
            double[] negativeRight = new double[right.coefficients.Length];
            for (int i = 0; i < right.coefficients.Length; i++)
                negativeRight[i] = -right.coefficients[i];
            return left + new Polynomial(negativeRight);
        }

        public static Polynomial operator *(Polynomial left, Polynomial right)
        {   
            double[] result = new double[left.coefficients.Length + right.coefficients.Length - 1];

            for (int i = 0; i < left.coefficients.Length; i++)
            {
                for (int j = 0; j < right.coefficients.Length; j++)
                {
                    result[i + j] += left.coefficients[i] * right.coefficients[j];
                }
            }

            return new Polynomial(result);
        }
    }
}
