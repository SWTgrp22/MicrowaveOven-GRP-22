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
    public class IT1B_Button_UserInterface
    {
        private Button sut_powerButton;
        private Button sut_startButton;
        private Button sut_timeButton;
        private UserInterface _UI;

        private ILight light;
        private Door sut_door;
        private IDisplay display;
        private ICookController cookController;

        [SetUp]
        public void Setup()
        {
            light = Substitute.For<ILight>();
            display = Substitute.For<IDisplay>();
            cookController = Substitute.For<ICookController>();

            sut_door = new Door();
            sut_powerButton = new Button();
            sut_startButton = new Button();
            sut_timeButton = new Button();

            _UI = new UserInterface(sut_powerButton, sut_timeButton, sut_startButton, sut_door, display, light, cookController);
        }

        #region Power button
       [TestCase(1,50)]
       [TestCase(2,100)]
        [TestCase(8,400)]
        [TestCase(14,700)]
        public void PowerButton_IsPressedMultiple_displayShowPowerRecivesACall(int NumberOfPresses, int powerLevel)
        {
            for (int i = 0; i < NumberOfPresses; i++)
            {
                sut_powerButton.Press();
            }

            display.Received(1).ShowPower(powerLevel);
        }

        [Test]
        public void PowerButton_IsPressed15Times_displayShowPowerRecivesTwoCalls()
        {
            for (int i = 0; i < 15; i++)
            {
                sut_powerButton.Press();
            }

            display.Received(2).ShowPower(50);

        }


        #endregion

        #region Time button

        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(8, 8)]
        [TestCase(14, 14)]
        [TestCase(60, 60)]
        [TestCase(120, 120)]
        public void TimeButton_IsPressedMultiple_DisplayShowTimeRecivesACall(int NumberOfPresses, int time)
        {
            //State maskinen kræver at der trykkes på power-knappen før man kan begynde at indstille tiden
            sut_powerButton.Press();

            for (int i = 0; i < NumberOfPresses; i++)
            {
                sut_timeButton.Press();
            }

            display.Received(1).ShowTime(time,0);
            //jf. UC beskrivelse er det kun minutterne der stiger og ikke sekunder - derfor er seconds = 0
        }

        #endregion

        #region Start-Cancel button

        [Test]
        public void StartButton_IsPressed_LightTurnOnRecivesACall()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();

            light.Received(1).TurnOn();
        }

        [Test]
        public void StartButton_IsPressed_CookControllerStartCookingRecivesACall()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();

            cookController.Received(1).StartCooking(50,60);
        }

        #region Extension 1

        [Test]
        public void StartButton_IsPressedDuringSetUp_DisplayClearRecivesACall()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();

            sut_startButton.Press();

            display.Received(1).Clear();
        }

        #endregion

        #region Extension 3

        [Test]
        public void StartButton_IsPressedTwice_CookControllerStopCookingRecivesACall()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();
            sut_startButton.Press();

            cookController.Received(1).Stop();
        }

        [Test]
        public void StartButton_IsPressedTwice_LightTurnOffRecivesACall()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();
            sut_startButton.Press();

            light.Received(1).TurnOff();
        }

        [Test]
        public void StartButton_IsPressedTwice_DisplayClearRecivesACall()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();
            sut_startButton.Press();

            display.Received(1).Clear();
        }
        #endregion
        
        #endregion

        #region Door

        #region Extension 2

        [Test]
        public void Door_DoorOpensDuringSetPower_LightTurnOnRecivesACall()
        {
            sut_powerButton.Press();

            sut_door.Open();

            light.Received(1).TurnOn();
        }
        [Test]
        public void Door_DoorOpensDuringSetPower_DisplayClearRecivesACall()
        {
            sut_powerButton.Press();

            sut_door.Open();

            display.Received(1).Clear();
        }

        [Test]
        public void Door_DoorOpensDuringSetTime_LightTurnOnRecivesACall()
        {
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_door.Open();

            light.Received(1).TurnOn();
        }

        [Test]
        public void Door_DoorOpensDuringSetTime_DisplayClearRecivesACall()
        {
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_door.Open();

            display.Received(1).Clear();
        }

        #endregion


        #endregion
    }
}
