using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelTest
{

    //This class runs the simulation and presents results
    public class OldModel
    {
        /* the following 3 constant variables are constants that cannot be regulated */
        private const double gasConstant = 8.3145;
        private const double preExpontentialFactor = 10000000;
        private const double volume = 50000; // liter
        private const double entalpi = -91800;
        private const double entropi = -198.05;

        private DataPoint currentState = new DataPoint(); //wut

        /* The constructor for Model
         * "dp" is a DataPoint received at the instancing of Model
         * currentState receives the value of dp
         */
        public OldModel(DataPoint dp)
        {
            currentState = dp;
        }

        /* allows outside classes to regulate the current amount of ammonia */
        public double Ammonia
        {
            set
            {
                currentState.nAmmonia = value;
            }

        }

        /* allows outside classes to regulate the current amount of hydrogen */
        public double Hydrogen
        {
            set
            {
                currentState.nHydrogen = value;
            }
        }

        /* allows outside classes to regulate the current amount of nitrogen */
        public double Nitrogen
        {
            set
            {
                currentState.nNitrogen = value;
            }
        }

        /* allows outside classes to regulate the current temperature */
        public double Temperature
        {
            set
            {
                currentState.temperature = value;
            }
        }

        /* utilizes the ideal gas law to calculate a given gas' partial pressure */
        private double calculatePartialPressure(double temperature, double nSubstance, double volume)
        {
            double partialPressure = (nSubstance * gasConstant * temperature) / volume;

            return partialPressure;
        }

        /* calculates the equillibrium constant */
        private double calculateEquillibriumConstant(double temperature)
        {
            double equiConst = Math.Pow(Math.E, -(entalpi / (temperature * gasConstant)) + (entropi / gasConstant));

            return equiConst;
        }

        /* uses the equillibrium fraction to isolate the pressure of ammonia */
        private double calculateAmmoniaAtEquillibrium(double pHydrogen, double pNitrogen, double equiConst)
        {
            double pAmmonia = equiConst * Math.Pow(pHydrogen, 3) * pNitrogen;

            return Math.Sqrt(pAmmonia);
        }

        /* takes a bool to decide which activation energy is used */
        private double calculateActivationEnergy(bool catalyst)
        {
            double activationEnergy = 0;

            if (catalyst)
                activationEnergy = 60;
            else if (!catalyst) // can also be written as else instead of else if (...)
                activationEnergy = 1100;

            return activationEnergy;
        }

        /* calulates the reactionrate constant */
        private double calculateReactionRateConstant(double temperature, double activationEnergy)
        {
            double RRConst = preExpontentialFactor * Math.Pow(Math.E, (activationEnergy / (gasConstant * temperature)));

            return RRConst;
        }

        /* uses the pressure of a reagent to calculate the molar amount */
        private double calculateMolarAmount(double pReagent)
        {
            double molarAmount = (pReagent * volume) / (gasConstant * currentState.temperature);

            return molarAmount;
        }

        /* calculates the pressure of the mixture */
        private double calculateActualPressure(double pAmmonia, double pNitrogen, double pHydrogen)
        {
            double pressure = pAmmonia + pNitrogen + pHydrogen;

            return pressure;
        }

        /* checks if the reaction has reached equilllibrium
         * "atEquillibrium" is a boolean that is true if we reached equillibrium
         * "equiFrac" is a variable calculating the equillibrium fraction
         */
        private bool isAtEquillibrium(double pAmmonia, double pNitrogen, double pHydrogen, double equiConst)
        {
            bool atEquillibrium = false;
            double equiFrac = Math.Pow(pAmmonia, 2) / (pNitrogen * Math.Pow(pHydrogen, 3));

            if (equiFrac.Equals(equiConst))
                atEquillibrium = true;

            return atEquillibrium;
        }

        /* acts like a property to check if we have equillibrium */
        public bool IsAtEquillibrium
        {

            get
            {
                return isAtEquillibrium(
                    calculatePartialPressure(currentState.temperature, currentState.nAmmonia, volume),
                    calculatePartialPressure(currentState.temperature, currentState.nNitrogen, volume),
                    calculatePartialPressure(currentState.temperature, currentState.nHydrogen, volume),
                    calculateEquillibriumConstant(currentState.temperature));
            }
        }

        /// <summary>
        /// Calculates the delta nitrogen.
        /// </summary>
        /// <returns>The delta nitrogen.</returns>
        /// <param name="pNitrogen">Partial pressure of nitrogen.</param>
        /// <param name="RRConst">Reactionrate constant.</param>
        /// <param name="deltaTime">Delta time.</param>
        private double calculateDeltaNitrogen(double pNitrogen, double RRConst, double deltaTime)
        {
            double nextPNitrogen = pNitrogen * Math.Pow(Math.E, -RRConst * deltaTime);
            return pNitrogen - nextPNitrogen;
        }

        private void UpdatePartialPressures(double deltaNitrogen, ref double pNitrogen, ref double pHydrogen, ref double pAmmonia)
        {
            Console.WriteLine("Før: " + pNitrogen); //tell
            pNitrogen = deltaNitrogen;
            Console.WriteLine("Efter: " + pNitrogen); //tell
            pAmmonia += 2 * deltaNitrogen;
            pHydrogen -= 3 * deltaNitrogen;
        }

        /* calculates a DataPoint
         * "deltaTime" is a variable describing the current time
         * "nextState" is a DataPoint containing the upcoming state of the simulation
         * the "local variables" are used as place holders, until they are finally
         * added to the DataPoint "nextState"
         */
        public DataPoint calculateDataPoint(double deltaTime)
        {
            DataPoint nextState = new DataPoint(currentState);
            double equiConst = 0, equiAmmonia = 0,
            pHydrogen = calculatePartialPressure(currentState.temperature, currentState.nHydrogen, volume),
            pNitrogen = calculatePartialPressure(currentState.temperature, currentState.nNitrogen, volume),
            pAmmonia = calculatePartialPressure(currentState.temperature, currentState.nAmmonia, volume),
            deltaNitrogen = 0, reactionRateconst = 0, activationEnergy = 0;

            /* calculate equillibrium constant */
            equiConst = calculateEquillibriumConstant(currentState.temperature);

            /* calculate amount of ammonia */
            equiAmmonia = calculateAmmoniaAtEquillibrium(pHydrogen, pNitrogen, equiConst);

            //Calculates activationenergy and reactionrate
            activationEnergy = calculateActivationEnergy(nextState.catalyst);
            reactionRateconst = calculateReactionRateConstant(currentState.temperature, activationEnergy);

            //Calculate the difference in nitrogen and update partial-pressures
            deltaNitrogen = calculateDeltaNitrogen(pNitrogen, reactionRateconst, deltaTime);
            UpdatePartialPressures(deltaNitrogen, ref pNitrogen, ref pHydrogen, ref pAmmonia);

            //Update the combined pressure
            nextState.pressure = calculateActualPressure(pAmmonia, pNitrogen, pHydrogen);

            /* calculate the molar amounts */
            nextState.nHydrogen = calculateMolarAmount(pHydrogen);
            nextState.nNitrogen = calculateMolarAmount(pNitrogen);
            nextState.nAmmonia = calculateMolarAmount(pAmmonia);

            Console.WriteLine("currentState.nNitrogen: " + currentState.nNitrogen); //tell
            Console.WriteLine("nextState.nNitrogen: " + nextState.nNitrogen); //tell
            Console.WriteLine("currentState.nHydrogen: " + currentState.nHydrogen); //tell
            Console.WriteLine("nextState.nHydrogen: " + nextState.nHydrogen); //tell
            Console.WriteLine("currentState.nAmmonia: " + currentState.nAmmonia); //tell
            Console.WriteLine("nextState.nAmmonia: " + nextState.nAmmonia); //tell
            Console.WriteLine("deltaNitrogen: " + deltaNitrogen); //tell

            //Update time
            nextState.time += deltaTime;

            //Update current state
            currentState = nextState;

            return currentState;
        }
    }
}