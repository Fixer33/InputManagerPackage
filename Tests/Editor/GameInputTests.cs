using NUnit.Framework;
using UnityEngine;

namespace Fixer33.InputManager.Tests
{
    public class GameInputTests
    {
        private sealed class FakeSource : IInputSource
        {
            public bool Jump;
            public Vector2 Move;

            public bool TryReadButton(string action, out bool pressed)
            {
                pressed = action == "Jump" && Jump;
                return action == "Jump";
            }

            public bool TryReadVector(string action, out Vector2 value)
            {
                value = action == "Move" ? Move : Vector2.zero;
                return action == "Move";
            }
        }

        [SetUp]
        public void SetUp() => GameInput.Reset();

        [TearDown]
        public void TearDown() => GameInput.Reset();

        [Test]
        public void InjectedPress_IsPressed()
        {
            GameInput.Virtual.SetButton("Jump", true);
            Assert.IsTrue(GameInput.IsPressed("Jump"));

            GameInput.Virtual.SetButton("Jump", false);
            Assert.IsFalse(GameInput.IsPressed("Jump"));
        }

        [Test]
        public void FirstInjectedPress_IsDetectedAsEdge()
        {
            GameInput.Virtual.SetButton("Jump", true);
            GameInput.Tick();
            Assert.IsTrue(GameInput.WasPressedThisFrame("Jump"));

            GameInput.Tick();
            Assert.IsFalse(GameInput.WasPressedThisFrame("Jump"), "edge lasts one frame");

            GameInput.Virtual.SetButton("Jump", false);
            GameInput.Tick();
            Assert.IsTrue(GameInput.WasReleasedThisFrame("Jump"));
        }

        [Test]
        public void ButtonIsPressed_WhenAnySourcePressesIt()
        {
            var source = new FakeSource { Jump = true };
            GameInput.AddSource(source);
            Assert.IsTrue(GameInput.IsPressed("Jump"));

            source.Jump = false;
            Assert.IsFalse(GameInput.IsPressed("Jump"));
        }

        [Test]
        public void Vector_TakesStrongestSource()
        {
            GameInput.AddSource(new FakeSource { Move = new Vector2(0.2f, 0f) });
            GameInput.Virtual.SetVector("Move", new Vector2(0f, 0.9f));
            Assert.AreEqual(new Vector2(0f, 0.9f), GameInput.GetVector("Move"));
        }

        [Test]
        public void Disabled_ReadsNothing()
        {
            GameInput.Virtual.SetButton("Jump", true);
            GameInput.Virtual.SetVector("Move", Vector2.up);
            GameInput.Enabled = false;
            Assert.IsFalse(GameInput.IsPressed("Jump"));
            Assert.AreEqual(Vector2.zero, GameInput.GetVector("Move"));
        }

        [Test]
        public void UnknownAction_IsReleasedAndZero()
        {
            Assert.IsFalse(GameInput.IsPressed("Nothing"));
            Assert.AreEqual(Vector2.zero, GameInput.GetVector("Nothing"));
        }
    }
}
