using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quadruple;

namespace modelTest
{

    //This class runs the simulation and presents results
    public class Model
    {
        /* the following 3 constant variables are constants that cannot be regulated */
        private Quad gasConstant = 8.3145;
        private Quad preExpontentialFactor = 1;
        private Quad volume = 50000; // liter
        private Quad entalpi = -91800;
        private Quad entropi = -198.05;

        private DataPoint currentState = new DataPoint(); //wut

        /* The constructor for Model
         * "dp" is a DataPoint received at the instancing of Model
         * currentState receives the value of dp
         */
        public Model(DataPoint dp)
        {
            currentState = dp;
        }

        /* allows outside classes to regulate the current amount of ammonia */
        public Quad Ammonia
        {
            set
            {
                currentState.nAmmonia = (double)value;
            }

        }

        /* allows outside classes to regulate the current amount of hydrogen */
        public Quad Hydrogen
        {
            set
            {
                currentState.nHydrogen = (double)value;
            }
        }

        /* allows outside classes to regulate the current amount of nitrogen */
        public Quad Nitrogen
        {
            set
            {
                currentState.nNitrogen = (double)value;
            }
        }

        /* allows outside classes to regulate the current temperature */
        public Quad Temperature
        {
            set
            {
                currentState.temperature = (double)value;
            }
        }

        /* utilizes the ideal gas law to calculate a given gas' partial pressure */
        private Quad calculatePartialPressure(Quad temperature, Quad nSubstance, Quad volume)
        {
            Quad partialPressure = (nSubstance * gasConstant * temperature) / volume;

            return partialPressure;
        }

        /* calculates the equillibrium constant */
        private Quad calculateEquillibriumConstant(Quad temperature)
        {
            Quad equiConst = Quad.Pow(Math.E, (double)(-(entalpi / (temperature * gasConstant)) + (entropi / gasConstant)));

            return equiConst;
        }

        /* uses the equillibrium fraction to isolate the pressure of ammonia */
        private Quad calculateAmmoniaAtEquillibrium(Quad pHydrogen, Quad pNitrogen, Quad equiConst)
        {
            Quad pAmmonia = equiConst * Quad.Pow(pHydrogen, 3) * pNitrogen;

            return Math.Sqrt((double)pAmmonia);
        }

        /* takes a bool to decide which activation energy is used */
        private Quad calculateActivationEnergy(bool catalyst)
        {
            Quad activationEnergy = 0;

            if (catalyst)
                activationEnergy = 60;
            else if (!catalyst) // can also be written as else instead of else if (...)
                activationEnergy = 1100;

            return activationEnergy;
        }

        /* calulates the reactionrate constant */
        private Quad calculateReactionRateConstant(Quad temperature, Quad activationEnergy)
        {
            Quad RRConst = preExpontentialFactor * Quad.Pow(Math.E, (double)(-activationEnergy / (gasConstant * temperature)));
            return RRConst;
        }

        /* uses the pressure of a reagent to calculate the molar amount */
        private Quad calculateMolarAmount(Quad pReagent)
        {
            Quad molarAmount = (pReagent * volume) / (gasConstant * currentState.temperature);

            return molarAmount;
        }

        /* calculates the pressure of the mixture */
        private Quad calculateActualPressure(Quad pAmmonia, Quad pNitrogen, Quad pHydrogen)
        {
            Quad pressure = pAmmonia + pNitrogen + pHydrogen;

            return pressure;
        }

        /* checks if the reaction has reached equilllibrium
         * "atEquillibrium" is a boolean that is true if we reached equillibrium
         * "equiFrac" is a variable calculating the equillibrium fraction
         */
        private bool isAtEquillibrium(Quad pAmmonia, Quad pNitrogen, Quad pHydrogen, Quad equiConst)
        {
            bool atEquillibrium = false;
            Quad equiFrac = Quad.Pow(pAmmonia, 2) / (pNitrogen * Quad.Pow(pHydrogen, 3));

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
        private Quad calculateNextNitrogen(Quad pNitrogen, Quad RRConst, Quad deltaTime)
        {
            return pNitrogen * Quad.Pow(Math.E, (double)(-RRConst * deltaTime));
        }

        private void UpdatePartialPressures(Quad nextNitrogen, ref Quad pNitrogen, ref Quad pHydrogen, ref Quad pAmmonia)
        {
            Console.WriteLine("Før: " + pNitrogen); //tell
            Quad deltaNitrogenTest = pNitrogen - nextNitrogen;
            pNitrogen = nextNitrogen;
            Console.WriteLine("Efter: " + pNitrogen); //tell
            pAmmonia += 2 * nextNitrogen;
            pHydrogen -= 3 * nextNitrogen;
        }

        /* calculates a DataPoint
         * "deltaTime" is a variable describing the current time
         * "nextState" is a DataPoint containing the upcoming state of the simulation
         * the "local variables" are used as place holders, until they are finally
         * added to the DataPoint "nextState"
         */
        public DataPoint calculateDataPoint(Quad deltaTime)
        {
            DataPoint nextState = new DataPoint(currentState);
            Quad equiConst = 0, equiAmmonia = 0,
            pHydrogen = calculatePartialPressure(currentState.temperature, currentState.nHydrogen, volume),
            pNitrogen = calculatePartialPressure(currentState.temperature, currentState.nNitrogen, volume),
            pAmmonia = calculatePartialPressure(currentState.temperature, currentState.nAmmonia, volume),
            nextNitrogen = 0, reactionRateconst = 0, activationEnergy = 0;

            /* calculate equillibrium constant */
            equiConst = calculateEquillibriumConstant(currentState.temperature);

            /* calculate amount of ammonia */
            equiAmmonia = calculateAmmoniaAtEquillibrium(pHydrogen, pNitrogen, equiConst);

            //Calculates activationenergy and reactionrate
            activationEnergy = calculateActivationEnergy(nextState.catalyst);
            reactionRateconst = calculateReactionRateConstant(currentState.temperature, activationEnergy);

            //Calculate the difference in nitrogen and update partial-pressures
            nextNitrogen = calculateNextNitrogen(pNitrogen, reactionRateconst, deltaTime);
            UpdatePartialPressures(nextNitrogen, ref pNitrogen, ref pHydrogen, ref pAmmonia);

            //Update the combined pressure
            nextState.pressure = (double)calculateActualPressure(pAmmonia, pNitrogen, pHydrogen);

            /* calculate the molar amounts */

            nextState.nHydrogen = (double)calculateMolarAmount(pHydrogen);
            nextState.nNitrogen = (double)calculateMolarAmount(pNitrogen);
            nextState.nAmmonia = (double)calculateMolarAmount(pAmmonia);

            Console.WriteLine("currentState.nNitrogen: " + currentState.nNitrogen); //tell
            Console.WriteLine("nextState.nNitrogen: " + nextState.nNitrogen); //tell
            Console.WriteLine("currentState.nHydrogen: " + currentState.nHydrogen); //tell
            Console.WriteLine("nextState.nHydrogen: " + nextState.nHydrogen); //tell
            Console.WriteLine("currentState.nAmmonia: " + currentState.nAmmonia); //tell
            Console.WriteLine("nextState.nAmmonia: " + nextState.nAmmonia); //tell
            Console.WriteLine("deltaNitrogen: " + nextNitrogen); //tell

            //Update time
            nextState.time += (double)deltaTime;

            //Update current state
            currentState = nextState;

            return currentState;
        }
    }
}