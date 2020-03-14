using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Madsbangh.EnumValueMapping;
using System;

namespace Madsbangh.EnumValueMapping.Examples
{
	public enum Faction
	{
		EvilGuys,
		GoodGuys,
		WeirdGuys,
		LargeGuys
	}

	[Serializable]
	public class FactionToFlagPrefabMapper : EnumValueMapping<Faction, GameObject> { }

	[Serializable]
	public class FactionToFlagTextureMapper : EnumValueMapping<Faction, Texture2D> { }

	/// <summary>
	/// This example shows how to use EnumValueMapping to easily create a serializeable field
	/// for each possible value in an enum. Note that the class is generic and needs to be
	/// subclassed into something specific so it can be serialized by Unity.
	/// </summary>
	public class EnumValueMappingExample : MonoBehaviour
	{
		[SerializeField]
		private FactionToFlagPrefabMapper prefabs = default;

		[SerializeField]
		private FactionToFlagTextureMapper textures = default;

		// Example on how to access the values of a given EnumValueMapping
		private void SpawnFlag(Faction faction, Vector3 position)
		{
			Instantiate(prefabs.GetValue(faction), position, Quaternion.identity);
		}

		private IEnumerator Start()
		{
			var waitOneSecond = new WaitForSeconds(1f);
			for (int i = 0; i < 10; i++)
			{
				yield return waitOneSecond;
				SpawnFlag(Faction.EvilGuys, UnityEngine.Random.insideUnitSphere * 5f);
				yield return waitOneSecond;
				SpawnFlag(Faction.GoodGuys, UnityEngine.Random.insideUnitSphere * 5f);
				yield return waitOneSecond;
				SpawnFlag(Faction.LargeGuys, UnityEngine.Random.insideUnitSphere * 5f);
				yield return waitOneSecond;
				SpawnFlag(Faction.WeirdGuys, UnityEngine.Random.insideUnitSphere * 5f);
			}
		}
	}
}
