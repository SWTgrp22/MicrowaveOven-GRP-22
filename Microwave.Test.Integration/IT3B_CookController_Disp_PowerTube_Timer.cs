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
    public class IT3B_CookController_Disp_PowerTube_Timer
    {
        private Button sut_powerButton;
        private Button sut_startButton;
        private Button sut_timeButton;
        private Door sut_door;
        private CookController _cookController;
        private UserInterface _UI;
        private Display _display;
        private Timer _timer;
        private PowerTube _powerTube;

        private ILight light;
        private IOutput output; 


        [SetUp]
        public void Setup()
        {
            light = Substitute.For<ILight>();
            output = Substitute.For<IOutput>();
            
            sut_powerButton = new Button();
            sut_startButton = new Button();
            sut_timeButton = new Button();
            sut_door = new Door();
            _display = new Display(output); 
            _timer = new Timer();
            _powerTube = new PowerTube(output);

            _cookController = new CookController(_timer, _display, _powerTube);
            _UI = new UserInterface(sut_powerButton, sut_timeButton, sut_startButton, sut_door, _display, light, _cookController);

            // Opretter den doublete association mellem CookController og UI 
            _cookController.UI = _UI;
        }


        #region Extention 3

        [Test]
        public void StartButton_IsPushedDuringCooking_OutputRecivesACallfromPowerTubeTurnOff()
        {
            sut_powerButton.Press();
            sut_timeButton.Press();
            sut_startButton.Press();

            //Simulere at tiden går
            Thread.Sleep(10500);

            //Start-Channel butten trykkes under Cooking
            sut_startButton.Press();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("PowerTube") && str.Contains("off")));
            
        }
        #endregion

        #region Extention 4

        [Test]
        public void Door_DoorOpensDuringCooking_OutputRecivesACallfromPowerTubeTurnOff()
        {
            sut_powerButton.Press();
            sut_timeButton.Press();
            sut_startButton.Press();

            //Døren åbnes under Cooking-state
            sut_door.Open();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("PowerTube") && str.Contains("off")));
            
        }

        #endregion

    }
}
