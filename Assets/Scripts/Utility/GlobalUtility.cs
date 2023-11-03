using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalUtility
{
	public static List<string> CreateEncounter()
	{
		List<string> encounter = new List<string>();

		int enemyCount = Random.Range(0, 4);

		for(int i = 0; i <= enemyCount; i++)
		{
			int enemyType = Random.Range(0, 3);

			switch(enemyType)
			{
				case 0:
					encounter.Add("Shambler");
					break;
				case 1:
					encounter.Add("Shrieker");
					break;
				case 2:
					encounter.Add("Strider");
					break;
			}
		}
		
		return encounter;
	}
}
