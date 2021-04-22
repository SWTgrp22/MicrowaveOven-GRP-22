using System.Net.NetworkInformation;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class Tests
    {
        private Door _sut;
        private UserInterface _UI;

        private ILight light;
        private IButton powerButton;
        private IButton startButton;
        private IButton timeButton;
        private IDisplay display;
        private ICookController cookController;

        [SetUp]
        public void Setup()
        {
            light = Substitute.For<ILight>();
            powerButton = Substitute.For<IButton>();
            startButton = Substitute.For<IButton>();
            timeButton = Substitute.For<IButton>();
            display = Substitute.For<IDisplay>();
            cookController = Substitute.For<ICookController>();

            _sut = new Door();
            _UI = new UserInterface(powerButton,timeButton,startButton,_sut,display,light,cookController);
        }

        [Test]
        public void door_doorIsOpen_lightTurnOnRecivesOneCall()
        {
            _sut.Open();

            light.Received(1).TurnOn();
        }

        [Test]
        public void door_doorIsClosed_lightTurnOffRecivesOneCall()
        {
            //Ifølge state maskinen skal døren åbnes for at kunne blive lukket:
            _sut.Open();
            _sut.Close();

            light.Received(1).TurnOff();
        }
    }
}