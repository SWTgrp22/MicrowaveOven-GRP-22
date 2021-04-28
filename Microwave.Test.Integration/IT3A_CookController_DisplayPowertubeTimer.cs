using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = Microwave.Classes.Boundary.Timer;

namespace Microwave.Test.Integration
{
    public class IT3A_CookController_DisplayPowertubeTimer
    {
        private Button startButton;
        private Button powerButton;
        private Button timeButton;
        private UserInterface userInterface;
        private CookController cookController;
        private PowerTube powerTube;
        private Display display;
        private Timer timer;

        private IDoor door;
        private ILight light;
        private IOutput output;
        [SetUp]
        public void Setup()
        {
            door = Substitute.For<IDoor>();
            light = Substitute.For<ILight>();
            output = Substitute.For<IOutput>();

            startButton = new Button();
            timeButton = new Button();
            powerButton = new Button();

            powerTube = new PowerTube(output);
            display = new Display(output);
            timer = new Timer();

            cookController = new CookController(timer, display, powerTube);

            userInterface =
                new UserInterface(powerButton, timeButton, startButton, door, display, light, cookController);

            // Implementerer dobbelt association:
            cookController.UI = userInterface;
        }

        #region Power button
        [TestCase(1,50)]
        [TestCase(2, 100)]
        [TestCase(8, 400)]
        [TestCase(14, 700)]
        public void CookController_PowerButtonIsPressed_OutpuLogLineRecivesACall(int numberOfPush,int powerLevel)
        {
            for (int i = 0; i < numberOfPush; i++)
            {
                powerButton.Press();
            }
            
            output.Received(1).OutputLine("Display shows: "+powerLevel+" W");
        }

        [Test]
        public void PowerButton_IsPressed15Times_OutpuLogLineRecivesTwoCalls()
        {
            for (int i = 0; i < 15; i++)
            {
                powerButton.Press();
            }

            output.Received(2).OutputLine("Display shows: " + 50 + " W");
        }

        #endregion

        #region Time button

        [TestCase(1, "01")]
        [TestCase(2, "02")]
        [TestCase(8, "08")]
        [TestCase(14, "14")]
        [TestCase(60, "60")]
        [TestCase(120, "120")]
        public void TimeButton_IsPressedMultiple_OutputLogLineRecivesACall(int NumberOfPresses, string time)
        {
            //State maskinen kræver at der trykkes på power-knappen før man kan begynde at indstille tiden
            powerButton.Press();

            for (int i = 0; i < NumberOfPresses; i++)
            {
                timeButton.Press();
            }

            output.Received(1).OutputLine("Display shows: "+time+":00");
            //jf. UC beskrivelse er det kun minutterne der stiger og ikke sekunder - derfor er seconds = 0
        }

        #endregion

        #region Start Button

        [TestCase(1,50)]
        [TestCase(2, 100)]
        [TestCase(8, 400)]
        [TestCase(14, 700)]
        public void CookController_StartIsPressed_OutputLogLinRecivesACallWithCorrectPowerLevel(int numberOfPress,int powerLevel)
        {
            for (int i = 0; i < numberOfPress; i++)
            {
                powerButton.Press();
            }
            
            timeButton.Press();

            startButton.Press();

            output.Received(1).OutputLine("PowerTube works with "+powerLevel);
            
        }




        #endregion

        #region When cooking

        [Test]
        public void CookController_StartIsPressed_OutputRecivesACallFromDisplayShowTime()
        {
            //powerButton.Press();

            //timeButton.Press();

            //startButton.Press();

            ////Simulere at tiden går
            //Thread.Sleep(1000);

            //output.Received(2).OutputLine("Display shows: 00:59");
        }

        [Test]
        public void CookController_StartIsPressed_OutputRecivesACallFromPowerTube()
        {

        }

        [Test]
        public void CookController_StartIsPressed_OutputRecivesACallFromDisplayClear()
        {

        }

        [Test]
        public void CookController_StartIsPressed_OutputRecivesACallFromLight()
        {

        }

        #endregion
    }
}
