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
        
        #region PowerTube
        [Test]
        public void CookController_StartIsPressed_OutputRecivesACallFromPowerTubeTurnOn()
        {
            powerButton.Press();

            timeButton.Press();

            startButton.Press();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("PowerTube works")));
        }

        [Test]
        public void CookController_StartIsPressed_OutputRecivesACallFromPowerTubeTurnOff()
        {
            powerButton.Press();

            timeButton.Press();

            startButton.Press();

            //Simulere at tiden går
            Thread.Sleep(60500);

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        #endregion

        #region Display

        [Test]
        public void CookController_StartIsPressed_OutputRecivesACallFromDisplayShowTime()
        {
            powerButton.Press();

            timeButton.Press();

            startButton.Press();

            //Simulere at tiden går
            Thread.Sleep(10100);

            output.Received(10).OutputLine(Arg.Is<string>(str =>str.Contains("00:")));
        }
        #endregion

        #endregion
    }
}
