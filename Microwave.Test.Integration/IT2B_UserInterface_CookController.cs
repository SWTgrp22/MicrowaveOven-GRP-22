using System;
using System.Collections.Generic;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class IT2B_UserInterface_CookController
    {
        private Door sut_door;
        private Button sut_startButton;
        private Button sut_timeButton;
        private Button sut_powerButton;
        private UserInterface userInterface;
        private CookController cookController;

        private ILight light;
        private IPowerTube powertube;
        private IDisplay display;
        private ITimer timer;

        [SetUp]
        public void Setup()
        {
            light = Substitute.For<ILight>();
            powertube = Substitute.For<IPowerTube>();
            display = Substitute.For<IDisplay>();
            timer = Substitute.For<ITimer>();

            sut_door = new Door();
            sut_powerButton = new Button();
            sut_timeButton = new Button();
            sut_startButton = new Button();

            cookController = new CookController(timer, display, powertube);
            userInterface = new UserInterface(sut_powerButton, sut_timeButton, sut_startButton, sut_door, display,
                light, cookController);
        }

        #region Extension 3

        [Test]
        public void startButton_IsPushedsDuringCooking_TimerStopRecivesACall()
        {
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();

            sut_startButton.Press();

            timer.Received(1).Stop();
        }

        [Test]
        public void StartButton_IsPushedDuringCooking_PowerTubeTurnOffRecivesACall()
        {
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();

            sut_startButton.Press();

            powertube.Received(1).TurnOff();
        }

        #endregion

        #region Extension 4
        [Test]
        public void Door_DoorOpensDuringCooking_TimerStopRecivesACall()
        {
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();

            sut_door.Open();

            timer.Received(1).Stop();
        }

        [Test]
        public void Door_DoorOpensDuringCooking_PowerTubeTurnOffRecivesACall()
        {
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();

            sut_door.Open();

            powertube.Received(1).TurnOff();
        }

        #endregion
    }
}
