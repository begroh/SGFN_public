using System;


namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class LogitechF310ModeXMacProfile : UnityInputDeviceProfile
	{
		public LogitechF310ModeXMacProfile()
		{
			Name = "Logitech F310 Controller";
			Meta = "Logitech F310 Controller on Mac (Mode X)";
			
			SupportedPlatforms = new[] {
				"OS X"
			};
			
			JoystickNames = new[] {
				"Logitech Gamepad F310"
			};
			
			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "A",
					Target = InputControlTarget.Action1,
					Source = Button16
				},
				new InputControlMapping {
					Handle = "B",
					Target = InputControlTarget.Action2,
					Source = Button17
				},
				new InputControlMapping {
					Handle = "X",
					Target = InputControlTarget.Action3,
					Source = Button18
				},
				new InputControlMapping {
					Handle = "Y",
					Target = InputControlTarget.Action4,
					Source = Button19
				},
				new InputControlMapping {
					Handle = "DPad Up",
					Target = InputControlTarget.DPadUp,
					Source = Button5
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlTarget.DPadDown,
					Source = Button6
				},
				new InputControlMapping {
					Handle = "DPad Left",
					Target = InputControlTarget.DPadLeft,
					Source = Button7
				},
				new InputControlMapping {
					Handle = "DPad Right",
					Target = InputControlTarget.DPadRight,
					Source = Button8
				},
				new InputControlMapping {
					Handle = "Left Bumper",
					Target = InputControlTarget.LeftBumper,
					Source = Button13
				},
				new InputControlMapping {
					Handle = "Right Bumper",
					Target = InputControlTarget.RightBumper,
					Source = Button14
				},
				new InputControlMapping {
					Handle = "Left Stick Button",
					Target = InputControlTarget.LeftStickButton,
					Source = Button11
				},
				new InputControlMapping {
					Handle = "Right Stick Button",
					Target = InputControlTarget.RightStickButton,
					Source = Button12
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlTarget.Start,
					Source = Button9
				},
				new InputControlMapping {
					Handle = "Back",
					Target = InputControlTarget.Select,
					Source = Button10
				},
				new InputControlMapping {
					Handle = "System",
					Target = InputControlTarget.System,
					Source = Button15
				}
			};
			
			AnalogMappings = new[] {
				new InputControlMapping {
					Handle = "Left Stick X",
					Target = InputControlTarget.LeftStickX,
					Source = Analog0
				},
				new InputControlMapping {
					Handle = "Left Stick Y",
					Target = InputControlTarget.LeftStickY,
					Source = Analog1,
					Invert = true
				},
				new InputControlMapping {
					Handle = "Right Stick X",
					Target = InputControlTarget.RightStickX,
					Source = Analog2
				},
				new InputControlMapping {
					Handle = "Right Stick Y",
					Target = InputControlTarget.RightStickY,
					Source = Analog3,
					Invert = true
				},
				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlTarget.LeftTrigger,
					Source = Analog4,
					TargetRange = InputControlMapping.Range.Positive,
					IgnoreInitialZeroValue = true
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlTarget.RightTrigger,
					Source = Analog5,
					TargetRange = InputControlMapping.Range.Positive,
					IgnoreInitialZeroValue = true
				}
			};
		}
	}
}

