using System;
using System.Collections.Generic;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class IT2A_Button_CookControler
    {
        private Button sut_powerButton;
        private Button sut_startButton;
        private Button sut_timeButton;
        private CookController _cookController;
        private UserInterface _UI;

        private ILight light;
        private IDoor door;
        private IDisplay display;
        private ITimer timer;
        private IPowerTube powerTube;
        

        [SetUp]
        public void Setup()
        {
            light = Substitute.For<ILight>();
            display = Substitute.For<IDisplay>();
            door = Substitute.For<IDoor>();
            timer = Substitute.For<ITimer>();
            powerTube = Substitute.For<IPowerTube>();

            
            sut_powerButton = new Button();
            sut_startButton = new Button();
            sut_timeButton = new Button();

            _cookController = new CookController(timer, display, powerTube);
            _UI = new UserInterface(sut_powerButton, sut_timeButton, sut_startButton, door, display, light, _cookController);

            // Opretter den doublete association mellem CookController og UI 
            _cookController.UI = _UI; 
        }

        #region Start-Channel Button

        [Test]
        public void StartButton_IsPressed_TimerStartRecivesACall()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();

            timer.Received(1).Start(60);
        }

        [Test]
        public void StartButton_IsPressed_PowerTubeTurnOnRecivesACall()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();

            powerTube.Received(1).TurnOn(50);
        }

        #endregion

        #region Cookning state

        [Test]
        public void StartButton_IsPressed_DisplayShowTimeRecivesACall()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();

            display.Received(1).ShowTime(1,0);
        }

        [Test]
        public void StartButton_IsPressed_PowerTubeTurnOffRecivesACall()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();
            sut_timeButton.Press();
            sut_startButton.Press();

            // Rejser Expired event, som simulerer at tiden er gået 
            timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            powerTube.Received(1).TurnOff();
        }

        [Test]
        public void StartButton_IsPressed_DisplayClearRecivesACall()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();
            sut_timeButton.Press();
            sut_startButton.Press();

            // Rejser Expired event, som simulerer at tiden er gået 
            timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            display.Received(1).Clear();
        }

        [Test]
        public void StartButton_IsPressed_LightTurnOffRecivesACall()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();
            sut_timeButton.Press();
            sut_startButton.Press();

            // Rejser Expired event, som simulerer at tiden er gået 
            timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            light.Received(1).TurnOff();
        }
        
        #endregion

    }
}
