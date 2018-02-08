using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using algebra;

public class algebraScript : MonoBehaviour
{
		public KMBombInfo Bomb;
		public KMSelectable but1;
		public KMSelectable but2;
		public KMSelectable but3;
		public KMSelectable but4;
		public KMSelectable but5;
		public KMSelectable but6;
		public KMSelectable but7;
		public KMSelectable but8;
		public KMSelectable but9;
		public KMSelectable but0;
		public KMSelectable negativeBut;
		public KMSelectable decimalBut;
		public KMSelectable submitBut;
		public KMSelectable clearBut;

		public KMAudio Audio;

		public Renderer pass1rend;
		public Renderer pass2rend;
		public Renderer pass3rend;
		public Renderer formulaScreen;
		public TextMesh inputText;

		public Texture passLight;
		public Texture screenOff;
		public Texture[] level1Options;
		public Texture[] level2Options;
		public Texture[] level3Options;
		Texture level1Equation;
		Texture level2Equation;
		Texture level3Equation;

		int stage = 1;
		static int moduleIdCounter = 1;
		int moduleId;

		string press0;
		string press1;
		string press2;
		string press3;
		string press4;
		string press5;
		string press6;
		string press7;
		string press8;
		string press9;
		string pressDecimal;
		string pressNegative;
		string pressClear;
		string pressSubmit;

		decimal valueX;
		decimal valueY;
		decimal valueZ;
		decimal valueA;
		decimal valueB;
		decimal valueC;

	//TwitchPlays Code
	private string TwitchHelpMessage = "Example: Type '!{0} submit <value>' OR '<value> submit'. Type 'clear' to clear the screen.";
	protected KMSelectable[] ProcessTwitchCommand(string input)
{
			Dictionary<char, KMSelectable> buttons = new Dictionary<char, KMSelectable>()
			{
					{ '0', but0 },
					{ '1', but1 },
					{ '2', but2 },
					{ '3', but3 },
					{ '4', but4 },
					{ '5', but5 },
					{ '6', but6 },
					{ '7', but7 },
					{ '8', but8 },
					{ '9', but9 },
					{ '-', negativeBut },
					{ '.', decimalBut }
			};
			string lowerinput = input.ToLowerInvariant();
			string [] split = lowerinput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			List<KMSelectable> Select = new List<KMSelectable>();
			string command = split[0];
			if (lowerinput == "submit")
			{
					return new KMSelectable[1] { submitBut };
			}
			else if (lowerinput == "clear")
			{
					return new KMSelectable[1] { clearBut };
			}
			else if (split.Length == 2 && split[0] == "submit")
			{
					command = split[1];
			}
			else if (split.Length > 2)
			{
					return null;
			}
			foreach (char c in command)
			{
					if (buttons.ContainsKey(c))
					{
							Select.Add(buttons[c]);
					}
					else
					{
							return null;
					}
			}
			if (split.Contains("submit"))
			{
					Select.Add(submitBut);
			}
			KMSelectable[] SelectArray = Select.ToArray();
			return SelectArray;
	}


		void Awake ()
		{
				moduleId = moduleIdCounter++;
				but0.OnInteract += delegate () { Onbut0(); return false; };
				but1.OnInteract += delegate () { Onbut1(); return false; };
				but2.OnInteract += delegate () { Onbut2(); return false; };
				but3.OnInteract += delegate () { Onbut3(); return false; };
				but4.OnInteract += delegate () { Onbut4(); return false; };
				but5.OnInteract += delegate () { Onbut5(); return false; };
				but6.OnInteract += delegate () { Onbut6(); return false; };
				but7.OnInteract += delegate () { Onbut7(); return false; };
				but8.OnInteract += delegate () { Onbut8(); return false; };
				but9.OnInteract += delegate () { Onbut9(); return false; };
				negativeBut.OnInteract += delegate () { OnnegativeBut(); return false; };
				decimalBut.OnInteract += delegate () { OndecimalBut(); return false; };
				clearBut.OnInteract += delegate () { OnclearBut(); return false; };
				submitBut.OnInteract += delegate () { OnsubmitBut(); return false; };
		}


		void Start ()
		{
				//Pick the three equations + debug
				int level1Pick = UnityEngine.Random.Range(0, 12);
				level1Equation = level1Options[level1Pick];

				int level2Pick = UnityEngine.Random.Range(0, 10);
				level2Equation = level2Options[level2Pick];

				int level3Pick = UnityEngine.Random.Range(0, 8);
				level3Equation = level3Options[level3Pick];

				Debug.LogFormat("[Algebra #{0}] Equation 1 is {1}.", moduleId, level1Equation.name);
				Debug.LogFormat("[Algebra #{0}] Equation 2 is {1}.", moduleId, level2Equation.name);
				Debug.LogFormat("[Algebra #{0}] Equation 3 is {1}.", moduleId, level3Equation.name);

				//Set the displays
				formulaScreen.GetComponent<Renderer>().material.mainTexture = level1Equation;
				inputText.text = "";

				//Pick the base values of x, y & z
				int allModuleCount = Bomb.GetModuleNames().Count;
				int indicatorCount = Bomb.GetIndicators().Count();

				int baseX = Bomb.GetSerialNumberNumbers().Sum();
				int baseY = indicatorCount - Bomb.GetPortCount();
				int baseZ = allModuleCount + ((Bomb.GetBatteryCount(Battery.D)) * (Bomb.GetBatteryCount(Battery.AA) + Bomb.GetBatteryCount(Battery.AAx3) + Bomb.GetBatteryCount(Battery.AAx4)));

				//Find the actual values of x, y & z
				if (Bomb.GetBatteryHolderCount() > 2)
				{
						valueX = valueX +2;
				}
				if (Bomb.GetPortCount(Port.RJ45) >=1)
				{
						valueX = valueX -1;
				}
				if (Bomb.IsIndicatorOn("BOB"))
				{
						valueX = valueX +4;
				}
				if (Bomb.GetSerialNumberLetters().Any(x => x == 'A' || x == 'E' || x == 'I' || x == 'O' || x == 'U'))
				{
						valueX = valueX -3;
				}
				valueX = valueX + baseX;


				if (Bomb.GetBatteryHolderCount() < 3)
				{
						valueY = valueY -2;
				}
				if (Bomb.GetPortCount(Port.Serial) >=1)
				{
						valueY = valueY +3;
				}
				if (Bomb.IsIndicatorOff("FRQ"))
				{
						valueY = valueY -5;
				}
				if (new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31 }.Contains(Bomb.GetSerialNumberNumbers().Sum()))
				{
						valueY = valueY +4;
				}
				valueY = valueY + baseY;


				if (Bomb.GetBatteryHolderCount() < 1)
				{
						valueZ = valueZ +3;
				}
				if (Bomb.GetPortCount(Port.Parallel) >=1)
				{
						valueZ = valueZ -6;
				}
				if (Bomb.IsIndicatorOn("MSA"))
				{
						valueZ = valueZ +2;
				}
				if (Bomb.GetSerialNumberNumbers().Sum() % 3  == 0)

				{
						valueZ = valueZ +1;
				}
				valueZ = valueZ + baseZ;

				//Debug for base & actual values of x, y & z
				Debug.LogFormat("[Algebra #{0}] The base value of X is {1}.", moduleId, baseX);
				Debug.LogFormat("[Algebra #{0}] The true value of X is {1}.", moduleId, valueX);

				Debug.LogFormat("[Algebra #{0}] The base value of Y is {1}.", moduleId, baseY);
				Debug.LogFormat("[Algebra #{0}] The true value of Y is {1}.", moduleId, valueY);

				Debug.LogFormat("[Algebra #{0}] The base value of Z is {1}.", moduleId, baseZ);
				Debug.LogFormat("[Algebra #{0}] The true value of Z is {1}.", moduleId, valueZ);

				//Level 1 formulae logic begins
				if (level1Pick == 0)
				{
						valueA = valueX + 1;
				}
				else if (level1Pick == 1)
				{
						valueA = 6 - valueX;
				}
				else if (level1Pick == 2)
				{
						valueA = 7 * valueX;
				}
				else if (level1Pick == 3)
				{
						valueA = valueX / 2;
				}
				else if (level1Pick == 4)
				{
						valueA = 5 + valueY;
				}
				else if (level1Pick == 5)
				{
						valueA = valueY - 2;
				}
				else if (level1Pick == 6)
				{
						valueA = 8 * valueY;
				}
				else if (level1Pick == 7)
				{
						valueA = valueY / 4;
				}
				else if (level1Pick == 8)
				{
						valueA = 9 + valueZ;
				}
				else if (level1Pick == 9)
				{
						valueA = valueZ - 7;
				}
				else if (level1Pick == 10)
				{
						valueA = 3 * valueZ;
				}
				else if (level1Pick == 11)
				{
						valueA = valueZ / 10;
				}

				//Level 2 formulae logic begins
				if (level2Pick == 0)
				{
						valueB = (valueX * valueY) - (2 + valueX);
				}
				else if (level2Pick == 1)
				{
						valueB = ((2 * valueX)/10) - valueY;
				}
				else if (level2Pick == 2)
				{
						valueB = (valueZ - valueY) / 2;
				}
				else if (level2Pick == 3)
				{
						valueB = valueX * valueY * valueZ;
				}
				else if (level2Pick == 4)
				{
						valueB = (valueY / 2) - valueZ;
				}
				else if (level2Pick == 5)
				{
						valueB = (valueZ * valueY) - (2 * valueX);
				}
				else if (level2Pick == 6)
				{
						valueB = (valueX + valueY) - (valueZ / 2);
				}
				else if (level2Pick == 7)
				{
						valueB = (7 * valueX) * valueY;
				}
				else if (level2Pick == 8)
				{
						valueB = (2 * valueZ) + 7;
				}
				else if (level2Pick == 9)
				{
						valueB = 2 * (valueZ + 7);
				}

				//Level 3 formulae logic begins
				if (level3Pick == 0)
				{
						valueC = valueX - (2 * valueY) + valueZ;
				}
				else if (level3Pick == 1)
				{
						valueC = ((valueX * valueY) - valueZ) * 10;
				}
				else if (level3Pick == 2)
				{
						valueC = ((valueY / 2 + 7) - valueZ) / 4;
				}
				else if (level3Pick == 3)
				{
						valueC = (valueX * 8) - valueZ + valueY;
				}
				else if (level3Pick == 4)
				{
						valueC = (2 + valueY) / 10 - (valueX * 3) + (valueZ / 4);
				}
				else if (level3Pick == 5)
				{
						valueC = 9 * (valueY / 2) + (valueX * valueY) / 4;
				}
				else if (level3Pick == 6)
				{
						valueC = (valueX * (valueY / 2) + 11) * (valueY * 2) - 4;
				}
				else if (level3Pick == 7)
				{
						valueC = ((valueZ / 2) - (valueX / 4) + valueZ) / 4;
				}

				//Debug for formulae solutions (values of a, b & c)
				Debug.LogFormat("[Algebra #{0}] The value of A is {1}.", moduleId, valueA.ToString("G0"));
				Debug.LogFormat("[Algebra #{0}] The value of B is {1}.", moduleId, valueB.ToString("G0"));
				Debug.LogFormat("[Algebra #{0}] The value of C is {1}.", moduleId, valueC.ToString("G0"));
		}


		public void Onbut0()
		{
				if (inputText.text.Length < 7)
				{
						inputText.text += "0";
						Audio.PlaySoundAtTransform("keyStroke", transform);
				}
		}

		public void Onbut1()
		{
				if (inputText.text.Length < 7)
				{
						inputText.text += "1";
						Audio.PlaySoundAtTransform("keyStroke", transform);
				}
		}

		public void Onbut2()
		{
				if (inputText.text.Length < 7)
				{
						inputText.text += "2";
						Audio.PlaySoundAtTransform("keyStroke", transform);
				}
		}

		public void Onbut3()
		{
				if (inputText.text.Length < 7)
				{
						inputText.text += "3";
						Audio.PlaySoundAtTransform("keyStroke", transform);
				}
		}

		public void Onbut4()
		{
				if (inputText.text.Length < 7)
				{
						inputText.text += "4";
						Audio.PlaySoundAtTransform("keyStroke", transform);
				}
		}

		public void Onbut5()
		{
				if (inputText.text.Length < 7)
				{
						inputText.text += "5";
						Audio.PlaySoundAtTransform("keyStroke", transform);
				}
		}

		public void Onbut6()
		{
				if (inputText.text.Length < 7)
				{
						inputText.text += "6";
						Audio.PlaySoundAtTransform("keyStroke", transform);
				}
		}

		public void Onbut7()
		{
				if (inputText.text.Length < 7)
				{
						inputText.text += "7";
						Audio.PlaySoundAtTransform("keyStroke", transform);
				}
		}

		public void Onbut8()
		{
				if (inputText.text.Length < 7)
				{
						inputText.text += "8";
						Audio.PlaySoundAtTransform("keyStroke", transform);
				}
		}

		public void Onbut9()
		{
				if (inputText.text.Length < 7)
				{
						inputText.text += "9";
						Audio.PlaySoundAtTransform("keyStroke", transform);
				}
		}

		public void OnnegativeBut()
		{
				if (inputText.text.Length < 7)
				{
						inputText.text = "-" + inputText.text;
						Audio.PlaySoundAtTransform("keyStroke", transform);
				}
		}

		public void OndecimalBut()
		{
				if (inputText.text.Length < 7)
				{
						inputText.text += ".";
						Audio.PlaySoundAtTransform("keyStroke", transform);
				}
		}

		public void OnclearBut()
		{
			inputText.text = "";
			Audio.PlaySoundAtTransform("keyStroke", transform);
		}

		public void OnsubmitBut()
		{
				Audio.PlaySoundAtTransform("keyStroke", transform);
				switch (stage)
				{
							case 1:
							decimal textValue1;
							if (decimal.TryParse(inputText.text, out textValue1) && valueA == textValue1)
							{
									pass1rend.material.mainTexture = passLight;
									Audio.PlaySoundAtTransform("correct", transform);
									stage++;
									inputText.text = "";
									formulaScreen.material.mainTexture = level2Equation;
									Debug.LogFormat("[Algebra #{0}] Your equation 1 response was {1}. This is correct.", moduleId, textValue1.ToString("G0"));
							}
							else
							{
									GetComponent<KMBombModule>().HandleStrike();
									inputText.text = "";
									Debug.LogFormat("[Algebra #{0}] Strike! Your equation 1 response was {1}. I was expecting {2}.", moduleId, textValue1.ToString("G0"), valueA.ToString("G0"));
							}
							break;

							case 2:
							decimal textValue2;
							if (decimal.TryParse(inputText.text, out textValue2) && valueB == textValue2)
							{
									pass2rend.material.mainTexture = passLight;
									Audio.PlaySoundAtTransform("correct", transform);
									stage++;
									inputText.text = "";
									formulaScreen.material.mainTexture = level3Equation;
									Debug.LogFormat("[Algebra #{0}] Your equation 2 response was {1}. This is correct.", moduleId, textValue2.ToString("G0"));
							}
							else
							{
									GetComponent<KMBombModule>().HandleStrike();
									inputText.text = "";
									Debug.LogFormat("[Algebra #{0}] Strike! Your equation 2 response was {1}. I was expecting {2}.", moduleId, textValue2.ToString("G0"), valueB.ToString("G0"));
							}
							break;

							case 3:
							decimal textValue3;
							if (decimal.TryParse(inputText.text, out textValue3) && valueC == textValue3)
							{
									pass3rend.material.mainTexture = passLight;
									Audio.PlaySoundAtTransform("correct", transform);
									GetComponent<KMBombModule>().HandlePass();
									inputText.text = "";
									formulaScreen.material.mainTexture = screenOff;
									Debug.LogFormat("[Algebra #{0}] Your equation 3 response was {1}. This is correct. Module disarmed.", moduleId, textValue3.ToString("G0"));
									stage++;
							}
							else
							{
									GetComponent<KMBombModule>().HandleStrike();
									inputText.text = "";
									Debug.LogFormat("[Algebra #{0}] Strike! Your equation 3 response was {1}. I was expecting {2}.", moduleId, textValue3.ToString("G0"), valueC.ToString("G0"));
							}
							break;

							default:
							{
									GetComponent<KMBombModule>().HandleStrike();
							}
							break;
				}
		}
}
