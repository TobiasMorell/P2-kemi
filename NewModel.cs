using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quadruple;

 
namespace modelTest
{
    /* the following 3 constant variables are constants that cannot be regulated */

    class NewModel
    {
        /* the following 3 constant variables are constants that cannot be regulated */
        private Quad gasConstant = 8.314472;
        private  Quad gasConstantCal = 1.987;
        private  Quad preExpontentialFactor = 884900000000000;
        private  Quad volume = 50000; //         liter
        private  Quad entalpi = -91800; //       J/mol
        private  Quad entropi = -198.05; //      J/(mol*kelvin)
        private Quad EaCatalyst = 170560; //     J/mol
        private  Quad EaNoCatalyst = 40765; //   kcal/kmol


        DataPoint currentState = new DataPoint();

        public Quad nAmmonia { set { currentState.nAmmonia = (double)value; } }
        public Quad nHydrogen { set { currentState.nHydrogen = (double)value; } }
        public Quad nNitrogen { set { currentState.nNitrogen = (double)value; } }
        public Quad temperature { set { currentState.temperature = (double)value; } }

        public NewModel(DataPoint InitData){
            currentState = InitData;
        }

        public DataPoint Fetch()
        {
            return currentState;
        }

        public void Update(double deltaTime){
            Console.WriteLine("befor!!!");
            Console.WriteLine("nAmmonia: " + currentState.nAmmonia);
            Console.WriteLine("nNitrogen: " + currentState.nNitrogen);
            Console.WriteLine("nHydrogen: " + currentState.nHydrogen);
            Console.WriteLine("\n\n\n");

            DataPoint NextState;
            CalculateNextState(deltaTime, out NextState);
            if(NextState.nHydrogen > 0 && NextState.nNitrogen > 0 && NextState.nAmmonia > 0)
                currentState = NextState;

            Console.WriteLine("Afta!!!");
            Console.WriteLine("nAmmonia: " + currentState.nAmmonia);
            Console.WriteLine("nNitrogen: " + currentState.nNitrogen);
            Console.WriteLine("nHydrogen: " + currentState.nHydrogen);
            Console.WriteLine("\n\n\n");
        }

        private DataPoint CalculateNextState(double deltaTime, out DataPoint nextState)
        {
            nextState = new DataPoint();

            Quad pNitrogen = CalculatePartialPressure(currentState.nNitrogen);
            Quad pHydrogen = CalculatePartialPressure(currentState.nHydrogen);
            Quad pAmmonia = CalculatePartialPressure(currentState.nAmmonia);

            Quad Y = CalculateEquilibriumConstant();
            Console.WriteLine("Y: " + Y);
            if (CalculateEquilibrium(Y, pNitrogen, pHydrogen, pAmmonia))
                return nextState = currentState;

            
            Quad rateConstant;
            if (currentState.catalyst)
                rateConstant = CalculateRateConstant(EaCatalyst);
            else
                rateConstant = CalculateRateConstant(EaNoCatalyst);

            Quad halfLife = CalculateHalfLife(rateConstant);
            Quad nextPNitrogen = CalculateNextPartialPressureFirstOrder(pNitrogen, rateConstant, deltaTime);
            Quad nextPAmmonia;
            if (pAmmonia > 0)
                nextPAmmonia = CalculateNextPartialPressureZerothOrder(pAmmonia, rateConstant, deltaTime);
            else
                nextPAmmonia = 0;
            Console.WriteLine("PNitrogen: " + pNitrogen);
            Console.WriteLine("PHydrogen: " + pHydrogen);
            Console.WriteLine("PAmmonia: " + pAmmonia);
            Console.WriteLine("nextPNitrogen: " + nextPNitrogen);
            Console.WriteLine("nextPAmmonia: " + nextPAmmonia);
            Console.WriteLine("rateconstant: " + rateConstant);
            Console.WriteLine("\n\n\n");

            Quad nextPHydrogen = pHydrogen - (3 * (pNitrogen - nextPNitrogen));
            CalculateChanges(ref nextPAmmonia, ref nextPNitrogen, ref nextPHydrogen, pAmmonia, pNitrogen, pHydrogen);

            Console.WriteLine("nextPHydrogen: " + nextPHydrogen);
            Console.WriteLine("\n\n\n");

            nextState.nAmmonia = (double)CalculateMolarAmount(nextPAmmonia);
            nextState.nHydrogen = (double)CalculateMolarAmount(nextPHydrogen);
            nextState.nNitrogen = (double)CalculateMolarAmount(nextPNitrogen);
            nextState.temperature = currentState.temperature;
            nextState.catalyst = currentState.catalyst;
            nextState.time = currentState.time + deltaTime;

            return new DataPoint();
        }

        private void CalculateChanges(ref Quad nextPAmmonia, ref Quad nextPNitrogen, ref Quad nextPHydrogen,
                                          Quad pAmmonia,         Quad pNitrogen,          Quad pHydrogen)
        {
            Quad tempAmmonia = nextPAmmonia;
            Quad tempNitrogen = nextPNitrogen;
            Quad tempHydrogen = nextPHydrogen;
            Console.WriteLine("nextPAmmonia before: " + nextPAmmonia);
            Console.WriteLine("NextPNitrogen before: " + nextPNitrogen);
            Console.WriteLine("pAmmonia: " + pAmmonia);
            nextPAmmonia = tempAmmonia + (2 * (pNitrogen - tempNitrogen));
            nextPNitrogen = tempNitrogen + (0.5 * (pAmmonia - tempAmmonia));
            nextPHydrogen = tempHydrogen + (1.5 * (pAmmonia - tempAmmonia));
            Console.WriteLine("nextPAmmonia after: " + nextPAmmonia);
            Console.WriteLine("NextPNitrogen after: " + nextPNitrogen);
        }

        private Quad CalculateNextPartialPressureFirstOrder(Quad pSubstance, Quad rateConstant, Quad deltaTime)
        {
            return pSubstance * Quad.Pow(Math.E, (double)(-rateConstant * deltaTime));
        }

        private Quad CalculateNextPartialPressureZerothOrder(Quad pSubstance, Quad rateConstant, Quad deltaTime)
        {
            return (double)(-rateConstant * deltaTime + pSubstance);
        }

        private Quad CalculateHalfLife(Quad rateConstant)
        {
            return Math.Log(2, Math.E)/rateConstant;
        }

        private Quad CalculateRateConstant(Quad Ea)
        {
            return preExpontentialFactor * Quad.Pow(Math.E, (double)(-Ea / (gasConstantCal * currentState.temperature)));
        }

        private bool CalculateEquilibrium(Quad Y, Quad pNitrogen, Quad pHydrogen, Quad pAmmonia)
        {
            Console.WriteLine("Y2: " + (Quad.Pow(pAmmonia, 2) / (pNitrogen * Quad.Pow(pHydrogen, 3))));
            if((Quad.Pow(pAmmonia, 2) / (pNitrogen * Quad.Pow(pHydrogen, 3))) >= Y)
                return true;
            else
                return false;
        }

        private Quad CalculateEquilibriumConstant()
        {
            return Quad.Pow(Math.E, (double)(-(entalpi / (gasConstant * currentState.temperature)) + entropi / gasConstant));
        }

        private Quad CalculatePartialPressure(Quad nSubstance)
        {
            return (nSubstance * gasConstant * currentState.temperature) / volume;
        }

        private Quad CalculateMolarAmount(Quad pSubstance)
        {
            return (pSubstance * volume) / (gasConstant * currentState.temperature);
        }
    }
}
