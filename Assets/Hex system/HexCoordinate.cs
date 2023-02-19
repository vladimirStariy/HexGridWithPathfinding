using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HexCoordinate
{
	public static Vector3Int OffsetToCude(Vector2Int offset)
	{
		var q = offset.x - (offset.y + (offset.y % 2)) / 2;
		var r = offset.y;
		return new Vector3Int(q, r, -q-r);
	}
}
