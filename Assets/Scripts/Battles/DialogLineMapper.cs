using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;

public static class DialogLineMapper
{
	public static TMP_Text dialogLine;
	private static Queue<string> lineQueue = new Queue<string>();
	public static bool hasLines = false;

	public static void QueueLine(string line)
	{
		lineQueue.Enqueue(line);
		Debug.Log("Added Line to the queue");

		if (!hasLines)
		{
			hasLines = true;
		}
	}

	public static string GetNextLine()
	{
		if(lineQueue.Count > 0)
		{
			return lineQueue.Dequeue();
		}
		else {
			hasLines = false;
			return "";
		}
	}

	public static IEnumerator Type(System.Action<bool> done, string message, TMP_Text output)
	{
		bool doneTyping = false;
		output.text = "";

		int index;

		for (index = 0; index < message.Length; index++)
		{
			output.text += message[index];

			if (Input.GetKey(KeyCode.Z))
			{
				output.text = message;
				break;
			}
			else if(message[index] != '.' || message[index] != '!' || message[index] != '?')
			{
				yield return new WaitForSeconds(0.1f);
			}
            else
            {
				yield return new WaitForSeconds(0.5f);
            }
        }

		doneTyping = true;

		yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

		done(doneTyping);
	}
}
