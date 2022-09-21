using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public class BoardGenerator
{
	public string name;

	[SerializeField]
	public List<int[]> map;
	public static int[,] MapFromJsonString(string json)
	{
		var gen = JsonConvert.DeserializeObject<BoardGenerator>(json);
		var totalRows = gen.map.Count;
		var totalCols = gen.map[0].Length;
		var map = new int[totalRows, totalCols];

		gen.map.Reverse();
		var index = 0;
		gen.map.ForEach(x =>
		{
			for (int i = 0; i < totalCols; i++)
			{
				map[index, i] = x[i];
			}
			index++;
		});

		return map;
	}
}
