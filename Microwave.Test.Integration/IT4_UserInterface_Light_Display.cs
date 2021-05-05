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
    public class IT4_UserInterface_Light_Display
    {
        private Door sut_Door;
        private UserInterface _userInterface;
        private CookController _cookController;
        private Display _display;
        private Timer _timer;
        private PowerTube _powerTube;
        private Light _light;

        private Button sut_powerButton;
        private Button sut_startButton;
        private Button sut_timeButton;
        private IOutput output;


        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();

            sut_Door = new Door();
            sut_powerButton = new Button();
            sut_startButton = new Button();
            sut_timeButton = new Button();

            _light = new Light(output);
            _display = new Display(output);
            _powerTube = new PowerTube(output);
            _timer = new Timer();
            
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface =
                new UserInterface(sut_powerButton, sut_timeButton, sut_startButton, sut_Door, _display, _light, _cookController);

            // Implementerer dobbelt association:
            _cookController.UI = _userInterface;
        }

        #region Light

        #region Door

        [Test]
        public void door_doorIsOpen_OutputRecivesOneCallFromLightTurnOn()
        {
            sut_Door.Open();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void door_doorIsClosed_OutputRecivesOneCallFromLightTurnOff()
        {
            //Ifølge state maskinen skal døren åbnes for at kunne blive lukket:

            sut_Door.Open();
            sut_Door.Close();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        #endregion

        #region Start-Chanel Button

        [Test]
        public void StartButton_IsPressed_OutputRecivesOneCallFromLightTurnOn()
        {
            //State maskinen kræver at der trykkes på power-knappen og time-knappen før man kan starte cooking
            sut_powerButton.Press();
            sut_timeButton.Press();

            sut_startButton.Press();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        #endregion


        #endregion






    }
}
